import { Component, inject, input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MainStorageService } from '../../../../services/main-storage.service';
import { MessageDto } from '../models/message-dto';

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

  mainStorage = inject(MainStorageService);

  isCurrentUser() {
    return this.messageDto().message.sender.id === this.mainStorage.CurrentUserId;
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
