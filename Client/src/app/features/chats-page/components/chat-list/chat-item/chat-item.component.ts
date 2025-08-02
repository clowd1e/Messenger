import { Component, input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ChatItem } from '../../../models/chat-item';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-chat-item',
  standalone: true,
  imports: [RouterModule, DatePipe],
  templateUrl: './chat-item.component.html',
  styleUrl: './chat-item.component.scss'
})
export class ChatItemComponent {
  chatItem = input.required<ChatItem>();

  selected = input<boolean>(false);

  currentUserId = input.required<string>();

  chatName = () => this.chatItem().users.find(user => user.id !== this.currentUserId())?.name || 'Unknown';
  
  chatLastMessageContent = () => this.truncateMessageContent(this.chatLastMessage().content);

  chatIcon = () => this.chatItem().users.find(user => user.id !== this.currentUserId())?.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png";

  chatLastMessage = () => {
    return this.chatItem().messages[this.chatItem().messages.length - 1];
  }

  private truncateMessageContent(message: string) {
    const maxLength = 30;
    return message.length > maxLength ? message.substring(0, maxLength - 3) + '...' : message;
  }
}
