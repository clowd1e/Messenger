import { Component, inject, input, output, signal } from '@angular/core';
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
  isCurrentlyEditing = input<boolean>(false);

  openContextMenu = output<{ messageId: string, isSendersMessage: boolean, x: number, y: number }>();

  mainStorage = inject(MainStorageService);
  isLongPressing = signal(false);
  private longPressTimer: any;
  private longPressDuration = 500;

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

  messageUpdatedAt() {
    return this.messageDto().message.updatedAt;
  }

  isMessageEdited() {
    return !!this.messageDto().message.updatedAt;
  }

  onMessageRightClick(event: MouseEvent) {
    event.preventDefault();

    this.openContextMenu.emit({
      messageId: this.messageDto().message.id,
      isSendersMessage: this.isCurrentUser(),
      x: event.clientX,
      y: event.clientY
    });
  }

  onTouchStart(event: TouchEvent) {
    this.isLongPressing.set(true);
    this.longPressTimer = setTimeout(() => {
      const touch = event.touches[0];
      this.openContextMenu.emit({
        messageId: this.messageDto().message.id,
        isSendersMessage: this.isCurrentUser(),
        x: touch.clientX,
        y: touch.clientY
      });
      this.isLongPressing.set(false);
    }, this.longPressDuration);
  }

  onTouchEnd() {
    this.isLongPressing.set(false);
    if (this.longPressTimer) {
      clearTimeout(this.longPressTimer);
    }
  }

  onTouchMove() {
    if (this.longPressTimer) {
      clearTimeout(this.longPressTimer);
      this.isLongPressing.set(false);
    }
  }
}
