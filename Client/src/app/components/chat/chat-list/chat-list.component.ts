import { Component, inject, input } from '@angular/core';
import { ChatItemComponent } from "../chat-item/chat-item.component";
import { ChatItem } from '../../../models/ChatItem';

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
}