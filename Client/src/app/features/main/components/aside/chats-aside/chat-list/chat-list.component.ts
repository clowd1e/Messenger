import { Component, computed, inject, input } from '@angular/core';
import { UuidHelperService } from '../../../../../../shared/services/uuid-helper.service';
import { ChatItemComponent } from './chat-item/chat-item.component';
import { InfiniteScrollDirective } from 'ngx-infinite-scroll';
import { ErrorHandlerService } from '../../../../../../shared/services/error-handler.service';
import { HttpErrorResponse } from '@angular/common/http';
import { PaginatedChatsResponse } from '../../../../models/paginated-chats-response';
import { ApiService } from '../../../../../../shared/services/api.service';
import { MainStorageService } from '../../../../services/main-storage.service';
import { PrivateChat } from '../../../../models/private-chat';
import { GroupChat } from '../../../../models/group-chat';
import { environment } from '../../../../../../../environments/environment';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [ChatItemComponent, InfiniteScrollDirective],
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.scss'
})
export class ChatListComponent {
  chatListLoading = input<boolean>();
  currentPage = 1;
  itemsPerPage = environment.CHATS_PAGE_SIZE;
  retrieveCutoff = new Date();
  isLastPage = false;

  uuidHelper = inject(UuidHelperService);
  apiService = inject(ApiService);
  errorHandler = inject(ErrorHandlerService);
  mainStorage = inject(MainStorageService);

  mappedChatList = computed(() => this.mainStorage.Chats()?.map(chat => {
    if (chat.type == 'private') {
      let privateChat = chat as PrivateChat;
      return {
        id: this.uuidHelper.toShortUuid(privateChat.id),
        creationDate: privateChat.creationDate,
        participants: privateChat.participants,
        lastMessage: privateChat.lastMessage,
        type: 'private'
      } as PrivateChat;
    } else if (chat.type == 'group') {
      let groupChat = chat as GroupChat;
      return {
        id: this.uuidHelper.toShortUuid(groupChat.id),
        creationDate: groupChat.creationDate,
        name: groupChat.name,
        description: groupChat.description,
        iconUri: groupChat.iconUri,
        participants: groupChat.participants,
        lastMessage: groupChat.lastMessage,
        type: 'group'
      } as GroupChat;
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
        this.mainStorage.appendChats(response.chats);
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
