import { Component, computed, input } from '@angular/core';
import { ChatItem } from '../../../models/chat-item';
import { CommonModule } from '@angular/common';
import { HamburgerComponent } from "./hamburger/hamburger.component";
import { AddChatButtonComponent } from "./add-chat-button/add-chat-button.component";
import { ModalComponent } from "../../../../../shared/components/modal/modal.component";
import { AddChatPageComponent } from "./add-chat-page/add-chat-page.component";
import { ChatListComponent } from './chat-list/chat-list.component';

@Component({
  selector: 'app-chats-aside',
  standalone: true,
  imports: [CommonModule, ChatListComponent, HamburgerComponent, AddChatButtonComponent, ModalComponent, AddChatPageComponent],
  templateUrl: './chats-aside.component.html',
  styleUrl: './chats-aside.component.scss'
})
export class ChatsAsideComponent {
  chats = input.required<ChatItem[]>();
  selectedChat = input<ChatItem | undefined>(undefined);
  chatsLoading = input.required<boolean>();
  chatRetrievalCutoff = input.required<Date>();
  currentUserId = input.required<string>();

  showModal = false;

  openModal(): void {
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }
}
