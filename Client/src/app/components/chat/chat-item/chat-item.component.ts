import { Component, input } from '@angular/core';
import { ChatItem } from '../../../models/ChatItem';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-chat-item',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './chat-item.component.html',
  styleUrl: './chat-item.component.scss'
})
export class ChatItemComponent {
  chatItem = input.required<ChatItem>();

  selected = input<boolean>(false);

  currentUserId = input.required<string>();

  chatName = () => this.chatItem().users.find(user => user.id !== this.currentUserId())?.username || 'Unknown';
  
  chatLastMessageContent = () => this.truncateMessageContent(this.chatLastMessage().content);

  chatLastMessageTimeFormated() {
    let messageTime: Date = new Date(this.chatLastMessage().timestamp);
    let result: string = this.messageTimeToString(messageTime);
    return result;
  }

  chatIcon = () => this.chatItem().users.find(user => user.id !== this.currentUserId())?.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png";

  private chatLastMessage = () => this.chatItem().messages[this.chatItem().messages.length - 1];

  private messageTimeToString(messageTime: Date) {
    let hours = messageTime.getHours().toString().length == 1 ? `0${messageTime.getHours()}` : messageTime.getHours();
    let minutes = messageTime.getMinutes().toString().length == 1 ? `0${messageTime.getMinutes()}` : messageTime.getMinutes();

    return `${hours}:${minutes}`;
  }

  private truncateMessageContent(message: string) {
    const maxLength = 30;
    return message.length > maxLength ? message.substring(0, maxLength - 3) + '...' : message;
  }
}
