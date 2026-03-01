import { ChangeDetectionStrategy, Component, computed, effect, ElementRef, inject, input, Renderer2, signal, untracked, ViewChild } from '@angular/core';
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
import { MessageOperationsModalComponent } from './chat-message/message-operations-modal/message-operations-modal.component';

@Component({
  selector: 'app-chat-message-list',
  standalone: true,
  imports: [ChatMessageComponent, InfiniteScrollDirective, MessagesDateBadgeComponent, MessageOperationsModalComponent],
  templateUrl: './chat-message-list.component.html',
  styleUrl: './chat-message-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatMessageListComponent {
  @ViewChild('messageListContainer', {static: false}) 
  private messageList? : ElementRef;
  private loadingMessages = false;
  private previousChatId: string | null = null;

  activeContextMenuMessageId = signal<string | null>(null);
  isSendersMessage = signal<boolean>(false);
  contextMenuPosition = signal<{ x: number; y: number } | null>(null);
  
  editingMessageId = signal<string | null>(null);
  editingMessageContent = signal<string | null>(null);
  
  userContextService = inject(UserContextService);
  renderer = inject(Renderer2);
  apiService = inject(ApiService);
  errorHandler = inject(ErrorHandlerService);
  mainStorage = inject(MainStorageService);

  //#region Message context menu operations

  onOpenContextMenu(event: { messageId: string, isSendersMessage: boolean, x: number, y: number }) {
    this.activeContextMenuMessageId.set(event.messageId);
    this.isSendersMessage.set(event.isSendersMessage);
    this.contextMenuPosition.set({ x: event.x, y: event.y });
  }

  closeContextMenu() {
    this.activeContextMenuMessageId.set(null);
    this.contextMenuPosition.set(null);
  }

  onEditMessage(messageId: string) {
    const selectedChat = this.mainStorage.SelectedChat();
    if (!selectedChat) {
      return;
    }

    const messages = this.mainStorage.getCurrentMessages();
    if (!messages) {
      return;
    }

    const message = messages().find(m => m.id === messageId);
    if (!message) {
      return;
    }

    this.editingMessageId.set(messageId);
    this.editingMessageContent.set(message.content);
    this.closeContextMenu();
  }

  onCancelEditMessage() {
    this.editingMessageId.set(null);
    this.editingMessageContent.set(null);
  }
  
  onDeleteForMe(messageId: string) {
    const selectedChat = this.mainStorage.SelectedChat();
    if (!selectedChat) {
      return;
    }

    const command = {
      messageId: messageId,
      chatId: selectedChat.id
    };

    this.apiService.deleteMessageForUser(command).subscribe({
      error: (error: any) => {
        this.errorHandler.handleHttpError(error);
      }
    });

    this.closeContextMenu();
  }

  onDeleteForEveryone(messageId: string) {
    const selectedChat = this.mainStorage.SelectedChat();
    if (!selectedChat) {
      return;
    }

    const command = {
      messageId: messageId,
      chatId: selectedChat.id
    };

    this.apiService.deleteMessageForEveryone(command).subscribe({
      error: (error: any) => {
        this.errorHandler.handleHttpError(error);
      }
    });

    this.closeContextMenu();
  }

  //#endregion

  onChatChangeEffect = effect(() => {
    const chatId = this.mainStorage.SelectedChatId();
    if (!chatId) return;
    
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

  onMessageReceivedEffect = effect(() => {
    const lastMessageResponse = this.mainStorage.LastReceivedMessage();
    if (!lastMessageResponse) return;
    untracked(() => {
      const currentChatId = this.mainStorage.SelectedChatId();
      if (lastMessageResponse.chatId !== currentChatId) return;

      const container = this.messageList?.nativeElement;
      if (!container) return;
      const distanceFromBottom = container.scrollHeight - (container.scrollTop + container.clientHeight);
      const userIsAtBottom = distanceFromBottom < 100;
      if (userIsAtBottom) {
        this.scrollToBottom();
      } else {
        const savedScrollTop = container.scrollTop;
        requestAnimationFrame(() => {
          container.scrollTop = savedScrollTop;
        });
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
      iconUri: message.sender.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png",
      updatedAt: message.updatedAt
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

    requestAnimationFrame(() => {
      scrollContainer.scroll({
        top: scrollContainer.scrollHeight,
        behavior: 'auto'
      });
    });
  }
}
