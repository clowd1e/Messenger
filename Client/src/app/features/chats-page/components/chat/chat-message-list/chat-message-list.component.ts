import { Component, computed, ElementRef, inject, input, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { ChatMessageComponent } from './chat-message/chat-message.component';
import { MessageDto } from '../models/message-dto';
import { InfiniteScrollDirective } from 'ngx-infinite-scroll';
import { PaginatedMessagesResponse } from '../../../models/paginated-messages-response';
import { ErrorHandlerService } from '../../../../../shared/services/error-handler.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ChatItem } from '../../../models/chat-item';
import { UserContextService } from '../../../../../shared/services/user-context.service';
import { ApiService } from '../../../../../shared/services/api.service';

@Component({
  selector: 'app-chat-message-list',
  standalone: true,
  imports: [ChatMessageComponent, InfiniteScrollDirective],
  templateUrl: './chat-message-list.component.html',
  styleUrl: './chat-message-list.component.scss'
})
export class ChatMessageListComponent {
  @ViewChild('messageListContainer', {static: false}) 
  private messageList? : ElementRef;

  chat = input.required<ChatItem | undefined>();

  currentPage = 1;
  itemsPerPage = 20;
  retrieveCutoff = new Date();
  isLastPage = false;
  
  userContextService = inject(UserContextService);
  renderer = inject(Renderer2);
  apiService = inject(ApiService);
  errorHandler = inject(ErrorHandlerService);

  currentUserId = this.userContextService.getCurrentUserId();

  ngOnInit() {
    this.chat()!.messages = [];
    this.loadNextChats();
  }

  isGroupChat = computed(() => this.chat()!.type === 'group');

  messageDtos(): MessageDto[] {
    const msgs = this.chat()!.messages;
    if (!msgs) return [];
    
    return msgs.map((message, i) => ({
      message,
      userNameVisible: i === 0 || msgs[i - 1]?.sender.id !== message.sender.id,
      userIconVisible: i === msgs.length - 1 || msgs[i + 1]?.sender.id !== message.sender.id,
      iconUri: message.sender.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png"
    }));
  }

  onScroll() {
    this.currentPage++;
    this.loadNextChats();
  }
  
  loadNextChats() {
    this.apiService.getChatMessagesPaginated(this.chat()!.id, this.currentPage, this.itemsPerPage, this.retrieveCutoff).subscribe({
      next: (response: PaginatedMessagesResponse) => {
        this.chat()!.messages.unshift(...response.messages);
        this.isLastPage = response.isLastPage;
      },
      error: (error: HttpErrorResponse) => {
        this.errorHandler.handleHttpError(error);
      }
    });
  }
  
  messageListEndReached() {
    return this.isLastPage;
  }

  ngAfterViewInit() {
    this.scrollToBottom();
  }

  ngOnChanges(changes: SimpleChanges) {
    // if (changes['messages']) {
    //   console.log("messages changed");
    //   this.scrollToBottom();
    // }
  }

  private scrollToBottom(): void {
    if (!this.messageList) {
      return;
    }

    const scrollContainer = this.messageList.nativeElement;

    this.renderer.setStyle(scrollContainer, 'opacity', '0');

    setTimeout(() => {

      scrollContainer.scrollTop = scrollContainer.scrollHeight;

      this.renderer.setStyle(scrollContainer, 'opacity', '100');
    }, 0);
  }
}
