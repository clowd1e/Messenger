import { Component, computed, inject, input } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MainStorageService } from '../../../../../services/main-storage.service';
import { Chat } from '../../../../../models/chat';
import { PrivateChat } from '../../../../../models/private-chat';
import { GroupChat } from '../../../../../models/group-chat';

@Component({
  selector: 'app-chat-item',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './chat-item.component.html',
  styleUrl: './chat-item.component.scss',
  providers: [DatePipe]
})
export class ChatItemComponent {
  chat = input.required<Chat>();
  selected = input<boolean | undefined>(false);

  router = inject(Router);
  mainStorage = inject(MainStorageService);
  datePipe = inject(DatePipe);

  chatName = computed(() => {
    if (this.chat().type === 'private') {
      let privateChat = this.chat() as PrivateChat;
      return privateChat.participants.find(user => user.id !== this.mainStorage.CurrentUserId)?.name || 'Unknown';
    } else if (this.chat().type === 'group') {
      let groupChat = this.chat() as GroupChat;
      return groupChat.name;
    } else {
      return 'Unknown';
    }
  });

  chatLastMessageContent = computed(() => this.truncateMessageContent(this.chatLastMessage().content));

  chatIcon = computed(() => {
    if (this.chat().type === 'private') {
      let privateChat = this.chat() as PrivateChat;
      return privateChat.participants.find(user => user.id !== this.mainStorage.CurrentUserId)?.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png";
    } else if (this.chat().type === 'group') {
      let groupChat = this.chat() as GroupChat;
      return groupChat.iconUri || "https://cdn-icons-png.flaticon.com/512/2352/2352167.png";
    } else {
      return '';
    }
  });

  chatLastMessage = () => {
    return this.chat().lastMessage;
  }

  chatLastMessageTime = () => {
    const date = new Date(this.chatLastMessage().timestamp);
    const today = new Date();
    if (date.toDateString() === today.toDateString()) {
      return this.datePipe.transform(date, 'HH:mm');
    }
    
    if (date.getFullYear() === today.getFullYear()) {
      return date.toLocaleDateString("en-US", { day: '2-digit', month: 'short' });
    } else {
      return date.toLocaleDateString("en-US", { day: '2-digit', month: 'short', year: 'numeric' });
    }
  }

  openChat(chatId: string) {
    this.router.navigateByUrl(`/chats/${chatId}`);
  }

  private truncateMessageContent(message: string) {
    const maxLength = 30;
    return message.length > maxLength ? message.substring(0, maxLength - 3) + '...' : message;
  }
}
