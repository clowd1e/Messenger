import { Component, computed, EventEmitter, input, Output } from '@angular/core';
import { ChatInputComponent } from "../chat-input/chat-input.component";
import { ChatMessageListComponent } from "../chat-message-list/chat-message-list.component";
import { CommonModule } from '@angular/common';
import { ChatItem } from '../../../Models/ChatItem';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [ChatInputComponent, ChatMessageListComponent, CommonModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent {
  chat = input<ChatItem>();

  @Output() messageSubmitted = new EventEmitter<string>();

  onMessageSubmit(message: string) {
    this.messageSubmitted.emit(message);
  }

  getMessages() {
    return this.chat()?.messages || [];
  }
}
