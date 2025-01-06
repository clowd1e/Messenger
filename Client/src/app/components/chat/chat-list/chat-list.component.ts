import { Component, inject, input } from '@angular/core';
import { ChatItemComponent } from "../chat-item/chat-item.component";
import { ChatItem } from '../../../models/ChatItem';
import { UuidHelperService } from '../../../services/uuid-helper/uuid-helper.service';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [ChatItemComponent],
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.scss'
})
export class ChatListComponent {
  currentUserId = input.required<string>();
  chatList = input<ChatItem[]>();
  chatListLoading = input<boolean>();
  selectedChatId = input<string>();

  uuidHelper = inject(UuidHelperService);

  mappedChatList = () => this.chatList()?.map(chatItem => {
    return {
      id: this.uuidHelper.toShortUuid(chatItem.id),
      creationDate: chatItem.creationDate,
      users: chatItem.users,
      messages: chatItem.messages,
    }
  })
}