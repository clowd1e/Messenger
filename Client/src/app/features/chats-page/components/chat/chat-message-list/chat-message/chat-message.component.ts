import { Component, input } from '@angular/core';
import { MessageDto } from '../../models/MessageDto';
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

  userIcon() {
    return this.messageDto().iconUri;
  }

  messageTimestamp() {
    return this.messageDto().message.timestamp;
  }
}
