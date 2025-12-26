import { ChangeDetectionStrategy, Component, computed, effect, ElementRef, inject, input, Renderer2, untracked, ViewChild } from '@angular/core';
import { ChatMessageComponent } from './chat-message/chat-message.component';
import { InfiniteScrollDirective } from 'ngx-infinite-scroll';
import { PaginatedMessagesResponse } from '../../../models/paginated-messages-response';
import { ErrorHandlerService } from '../../../../../shared/services/error-handler.service';
import { HttpErrorResponse } from '@angular/common/http';
import { UserContextService } from '../../../../../shared/services/user-context.service';
import { ApiService } from '../../../../../shared/services/api.service';
import { MainStorageService } from '../../../services/main-storage.service';
import { firstValueFrom } from 'rxjs';
import { MessagesDateBadgeComponent } from "./messages-date-badge/messages-date-badge.component";
import { Message } from '../../../models/message';
import { MessageRenderItem } from './models/message-render-item';
import { MessageDto } from './models/message-dto';

@Component({
  selector: 'app-chat-message-list',
  standalone: true,
  imports: [ChatMessageComponent, InfiniteScrollDirective, MessagesDateBadgeComponent],
  templateUrl: './chat-message-list.component.html',
  styleUrl: './chat-message-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatMessageListComponent {
  @ViewChild('messageListContainer', {static: false}) 
  private messageList? : ElementRef;
  private loadingMessages = false;
  private previousChatId: string | null = null;

  isAddPrivateChatRoute = input.required<boolean>();
  isAddGroupChatRoute = input.required<boolean>();
  
  userContextService = inject(UserContextService);
  renderer = inject(Renderer2);
  apiService = inject(ApiService);
  errorHandler = inject(ErrorHandlerService);
  mainStorage = inject(MainStorageService);

  onChatChangeEffect = effect(() => {
    const chatId = this.mainStorage.SelectedChatId();
    if (!chatId) return;
    if (this.isAddPrivateChatRoute() || this.isAddGroupChatRoute()) return;
    
    untracked(() => {
      if (this.previousChatId && this.previousChatId !== chatId) {
        const container = this.messageList?.nativeElement;
          if (container) {
            this.mainStorage.saveChatScroll(this.previousChatId, container.scrollTop);
          }
        }
    });

    this.previousChatId = chatId;

    untracked(async () => {
      const messages = this.mainStorage.getCurrentMessages();
      if (messages && messages().length == 0) {
        await this.loadNextMessages();
        this.scrollToBottom();
      } else {
        this.restoreScrollPosition();
      }
    });
  });

  isGroupChat = computed(() => this.mainStorage.SelectedChat()!.type === 'group');

  messageRenderItems = computed<MessageRenderItem[]>(() => {
    const messages = this.mainStorage.getCurrentMessages();
    const chatCreatedAt = this.getDateKey(this.mainStorage.GetCurrentChatCreatedAt());
    if (!messages) return [];

    const result: MessageRenderItem[] = [];
    let lastDate: string | null = null;

    for (const message of messages()) {
      const dateKey = this.getDateKey(message.timestamp);

      if (dateKey !== lastDate) {
        // if date key is chat creation date, label it accordingly
        if (dateKey === chatCreatedAt) {
          result.push({
            type: 'date-badge',
            label: 'Chat created'
          });
        }

        result.push({
          type: 'date-badge',
          label: this.formatDateBadge(message.timestamp)
        });
        lastDate = dateKey;
      }

      result.push({
        type: 'message',
        messageDto: this.createMessageDto(message, messages())
      });
    }

    return result;
  });

  private createMessageDto(message: Message, messages: Message[]) : MessageDto {
    const i = messages.findIndex(m => m.id === message.id);

    return {
      message,
      userNameVisible: i === 0 || messages[i - 1]?.sender.id !== message.sender.id,
      userIconVisible: i === messages.length - 1 || messages[i + 1]?.sender.id !== message.sender.id,
      iconUri: message.sender.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png"
    };
  }

  private getDateKey(timestamp: string): string {
    return new Date(timestamp).toDateString();
  }

  private formatDateBadge(timestamp: string): string {
    const date = new Date(timestamp);
    const today = new Date();
    if (date.toDateString() === today.toDateString()) {
      return 'Today';
    }
    const yesterday = new Date();
    yesterday.setDate(today.getDate() - 1);
    if (date.toDateString() === yesterday.toDateString()) {
      return 'Yesterday';
    }
    
    if (date.getFullYear() === today.getFullYear()) {
      return date.toLocaleDateString("en-US", { day: '2-digit', month: 'short' });
    } else {
      return date.toLocaleDateString("en-US", { day: '2-digit', month: 'short', year: 'numeric' });

    }
  }

  async onScroll() {
    if (this.loadingMessages) return;
    this.loadingMessages = true;
    // Prevent loading more messages if we're in the add private chat route
    if (this.isAddPrivateChatRoute()) return;
    if (this.isAddGroupChatRoute()) return;
    if (this.messageListEndReached()) return;
    const container = this.messageList?.nativeElement;
    if (!container) return;
    
    const anchorOffset = container.scrollHeight;
    await this.loadNextMessages();
    // Restore scroll position
    requestAnimationFrame(() => {
      const newScrollTop = container.scrollHeight - anchorOffset;
      container.scroll({
        top: newScrollTop,
        behavior: 'auto'
      });
    });
    this.loadingMessages = false;
  }
  
  async loadNextMessages() : Promise<void> {
    const currentChatId = this.mainStorage.SelectedChatId();
    const metadata = this.mainStorage.getCurrentMessagesMetadata();
    if (metadata?.isLastPage) {
      return;
    }

    await firstValueFrom(
      this.apiService.getChatMessagesPaginated(
        currentChatId!,
        metadata!.currentPage,
        this.mainStorage.MessagesPageSize,metadata!.retrieveCutoff)
    ).then(
      (res: PaginatedMessagesResponse) => {
        this.mainStorage.loadPreviousMessages(res.messages, res.isLastPage);
      }
    ).catch(
      (error: HttpErrorResponse) => {
        this.errorHandler.handleHttpError(error);
      }
    );
  }
  
  messageListEndReached() {
    const metadata = this.mainStorage.getCurrentMessagesMetadata();
    return metadata ? metadata.isLastPage : true;
  }

  private restoreScrollPosition(): void {
    if (!this.messageList) {
      return;
    }
    const scrollContainer = this.messageList.nativeElement;
    const metadata = this.mainStorage.getCurrentMessagesMetadata();
    if (!metadata) {
      return;
    }
    const savedScrollPosition = metadata.chatScrollPosition || 0;
    requestAnimationFrame(() => {
      scrollContainer.scroll({
        top: savedScrollPosition,
        behavior: 'auto'
      });
    });
  }

  private scrollToBottom(): void {
    if (!this.messageList) {
      return;
    }

    const scrollContainer = this.messageList.nativeElement;

    // this.renderer.setStyle(scrollContainer, 'opacity', '0');

    // scrollContainer.scrollTop = scrollContainer.scrollHeight;
    requestAnimationFrame(() => {
      scrollContainer.scroll({
        top: scrollContainer.scrollHeight,
        behavior: 'auto'
      });
    });

    // this.renderer.setStyle(scrollContainer, 'opacity', '100');
  }
}
