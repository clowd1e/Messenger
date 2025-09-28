import { Component, input } from '@angular/core';
import { MessageDto } from '../../models/message-dto';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-chat-message',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './chat-message.component.html',
  styleUrl: './chat-message.component.scss'
})
export class ChatMessageComponent {
  messageDto = input.required<MessageDto>();
  isGroupChat = input.required<boolean>();
  currentUserId = input.required<string>();

  isCurrentUser() {
    return this.messageDto().message.sender.id === this.currentUserId();
  }

  userIconVisible() {
    return this.messageDto().userIconVisible;
  }

  userNameVisible() {
    return this.messageDto().userNameVisible;
  }

  messageSender() {
    return this.messageDto().message.sender.name;
  }

  messageContent() {
    return this.messageDto().message.content;
  }

  userIcon() {
    return this.messageDto().iconUri;
  }

  messageTimestamp() {
    return this.messageDto().message.timestamp;
  }
}
