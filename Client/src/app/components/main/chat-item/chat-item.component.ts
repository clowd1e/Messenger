import { Component, input } from '@angular/core';
import { ChatItem } from '../../../Models/ChatItem';

@Component({
  selector: 'app-chat-item',
  standalone: true,
  imports: [],
  templateUrl: './chat-item.component.html',
  styleUrl: './chat-item.component.scss'
})
export class ChatItemComponent {
  chatItem = input.required<ChatItem>();

  chatName = () => this.chatItem().users.map(user => user.userName).join(', ');
  
  chatLastMessageContent = () => this.truncateMessageContent(this.chatLastMessage().messageContent);

  chatLastMessageTimeFormated() {
    let messageTime : Date = this.chatLastMessage().messageTimestamp;
    let result: string = `${messageTime.getHours()}:${messageTime.getMinutes()}`;
    return result;
  }

  private chatLastMessage = () => this.chatItem().messages[this.chatItem().messages.length - 1];

  private truncateMessageContent(message: string) { 
    const maxLength = 30;
    return message.length > maxLength ? message.substring(0, maxLength - 3) + '...' : message;
  }
}
