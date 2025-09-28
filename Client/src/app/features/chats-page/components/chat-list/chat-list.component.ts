import { Component, computed, inject, input } from '@angular/core';
import { ChatItem } from '../../models/chat-item';
import { UuidHelperService } from '../../../../shared/services/uuid-helper.service';
import { ChatItemComponent } from './chat-item/chat-item.component';
import { InfiniteScrollDirective } from 'ngx-infinite-scroll';
import { ErrorHandlerService } from '../../../../shared/services/error-handler.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MapChatToChatItem } from '../../mappers/chat-to-chat-item.mapper';
import { PaginatedChatsResponse } from '../../models/paginated-chats-response';
import { ApiService } from '../../../../shared/services/api.service';
import { PrivateChatItem } from '../../models/private-chat-item';
import { GroupChatItem } from '../../models/group-chat-item';

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

  mappedChatList = computed(() => this.chatList()?.map(chatItem => {
    if (chatItem.type == 'private') {
      let privateChat = chatItem as PrivateChatItem;
      return {
        id: this.uuidHelper.toShortUuid(privateChat.id),
        creationDate: privateChat.creationDate,
        participants: privateChat.participants,
        messages: privateChat.messages,
        type: 'private'
      } as PrivateChatItem;
    } else if (chatItem.type == 'group') {
      let groupChat = chatItem as GroupChatItem;
      return {
        id: this.uuidHelper.toShortUuid(groupChat.id),
        creationDate: groupChat.creationDate,
        name: groupChat.name,
        description: groupChat.description,
        iconUri: groupChat.iconUri,
        participants: groupChat.participants,
        messages: groupChat.messages,
        type: 'group'
      } as GroupChatItem;
    } else {
      throw new Error('Unknown chat type');
    }
  }));

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
