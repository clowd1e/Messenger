import { Component, inject, input } from '@angular/core';
import { ChatItemComponent } from "../chat-item/chat-item.component";
import { ChatItem } from '../../../Models/ChatItem';
import { UserContextService } from '../../../services/auth/user-context.service';

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
}