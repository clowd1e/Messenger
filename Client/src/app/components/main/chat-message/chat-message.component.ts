import { Component, input } from '@angular/core';
import { Message } from '../../../Models/Message';
import { MessageDto } from '../../../Models/DTO/MessageDto';

@Component({
  selector: 'app-chat-message',
  standalone: true,
  imports: [],
  templateUrl: './chat-message.component.html',
  styleUrl: './chat-message.component.scss'
})
export class ChatMessageComponent {
  messageDto = input.required<MessageDto>();
  currentUserId = input.required<string>();

  isCurrentUser() {
    return this.messageDto().message.userId === this.currentUserId();
  }

  userIconVisible() {
    return this.messageDto().userIconVisible;
  }

  messageContent() {
    return this.messageDto().message.content;
  }

  messageTime() {
    let messageTime: Date = new Date(this.messageDto().message.timestamp);
    let result: string = this.messageTimeToString(messageTime);
    
    return result;
  }

  private messageTimeToString(messageTime: Date) {
    let hours = messageTime.getHours().toString().length == 1 ? `0${messageTime.getHours()}` : messageTime.getHours();
    let minutes = messageTime.getMinutes().toString().length == 1 ? `0${messageTime.getMinutes()}` : messageTime.getMinutes();

    return `${hours}:${minutes}`;
  }
}
