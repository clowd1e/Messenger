import { Component, input } from '@angular/core';
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

  getMessages() {
    return this.chat()?.messages || [];
  }
}
