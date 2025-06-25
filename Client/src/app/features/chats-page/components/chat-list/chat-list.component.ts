import { Component, inject, input } from '@angular/core';
import { ChatItem } from '../../models/ChatItem';
import { UuidHelperService } from '../../../../shared/services/uuid-helper/uuid-helper.service';
import { ChatItemComponent } from './chat-item/chat-item.component';
import { InfiniteScrollDirective } from 'ngx-infinite-scroll';
import { ApiService } from '../../../../shared/services/api/api.service';
import { ErrorHandlerService } from '../../../../shared/services/error-handler/error-handler.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MapChatToChatItem } from '../../mappers/ChatToChatItemMapper';
import { PaginatedChatsResponse } from '../../models/PaginatedChatsResponse';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [ChatItemComponent, InfiniteScrollDirective],
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.scss'
})
export class ChatListComponent {
  currentUserId = input.required<string>();
  chatList = input<ChatItem[]>();
  chatListLoading = input<boolean>();
  selectedChatId = input<string>();
  currentPage = 1;
  itemsPerPage = 10;
  retrieveCutoff = new Date();
  isLastPage = false;

  uuidHelper = inject(UuidHelperService);
  apiService = inject(ApiService);
  errorHandler = inject(ErrorHandlerService);

  mappedChatList = () => this.chatList()?.map(chatItem => {
    return {
      id: this.uuidHelper.toShortUuid(chatItem.id),
      creationDate: chatItem.creationDate,
      users: chatItem.users,
      messages: chatItem.messages,
    }
  })

  onScroll() {
    this.currentPage++;
    this.loadNextChats();
  }

  loadNextChats() {
    this.apiService.getUserChatsPaginated(this.currentPage, this.itemsPerPage, this.retrieveCutoff).subscribe({
      next: (response: PaginatedChatsResponse) => {
        this.chatList()?.push(...response.chats.map(MapChatToChatItem));
        this.isLastPage = response.isLastPage;
      },
      error: (error: HttpErrorResponse) => {
        this.errorHandler.handleHttpError(error);
      }
    });
  }

  chatListEndReached() {
    return this.isLastPage;
  }
}
