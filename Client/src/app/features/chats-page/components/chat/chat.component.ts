import { CommonModule } from '@angular/common';
import { Component, computed, EventEmitter, input, Output } from '@angular/core';
import { ChatItem } from '../../models/chat-item';
import { ChatInputComponent } from './chat-input/chat-input.component';
import { ChatMessageListComponent } from './chat-message-list/chat-message-list.component';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [ChatMessageListComponent, ChatInputComponent, CommonModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent {
  chat = input.required<ChatItem | undefined>();

  @Output() messageSubmitted = new EventEmitter<string>();

  onMessageSubmit(message: string) {
    this.messageSubmitted.emit(message);
  }

  chatType = computed(() => this.chat()?.type);

  getMessages() {
    return this.chat()?.messages || [];
  }
}
