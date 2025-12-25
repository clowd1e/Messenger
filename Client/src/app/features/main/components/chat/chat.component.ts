import { CommonModule } from '@angular/common';
import { Component, computed, effect, EventEmitter, inject, input, Output } from '@angular/core';
import { ChatInputComponent } from './chat-input/chat-input.component';
import { ChatMessageListComponent } from './chat-message-list/chat-message-list.component';
import { ChatInfoHeaderComponent } from "./chat-info-header/chat-info-header.component";
import { MainStorageService } from '../../services/main-storage.service';
import { Chat } from '../../models/chat';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [ChatMessageListComponent, ChatInputComponent, CommonModule, ChatInfoHeaderComponent],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent {
  isAddPrivateChatRoute = input.required<boolean>();
  isAddGroupChatRoute = input.required<boolean>();

  selectedChat: Chat | null = null;

  selectedChatEffect = effect(() => {
    this.selectedChat = this.mainStorage.SelectedChat();
  });
  
  @Output() messageSubmitted = new EventEmitter<string>();

  mainStorage = inject(MainStorageService);

  onMessageSubmit(message: string) {
    this.messageSubmitted.emit(message);
  }
}
