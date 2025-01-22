import { CommonModule } from '@angular/common';
import { Component, EventEmitter, input, Output } from '@angular/core';
import { ChatItem } from '../../models/ChatItem';
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
  chat = input<ChatItem>();

  @Output() messageSubmitted = new EventEmitter<string>();

  onMessageSubmit(message: string) {
    this.messageSubmitted.emit(message);
  }

  getUsers() {
    return this.chat()?.users || [];
  }

  getMessages() {
    return this.chat()?.messages || [];
  }
}
