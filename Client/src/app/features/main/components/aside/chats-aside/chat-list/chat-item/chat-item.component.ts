import { Component, computed, effect, inject, input } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ChatItem } from '../../../../../models/chat-item';
import { DatePipe } from '@angular/common';
import { PrivateChatItem } from '../../../../../models/private-chat-item';
import { GroupChatItem } from '../../../../../models/group-chat-item';

@Component({
  selector: 'app-chat-item',
  standalone: true,
  imports: [RouterModule, DatePipe],
  templateUrl: './chat-item.component.html',
  styleUrl: './chat-item.component.scss'
})
export class ChatItemComponent {
  chatItem = input.required<ChatItem>();
  selected = input<boolean | undefined>(false);
  currentUserId = input.required<string>();

  router = inject(Router);

  chatName = computed(() => {
    if (this.chatItem().type === 'private') {
      let privateChat = this.chatItem() as PrivateChatItem;
      return privateChat.participants.find(user => user.id !== this.currentUserId())?.name || 'Unknown';
    } else if (this.chatItem().type === 'group') {
      let groupChat = this.chatItem() as GroupChatItem;
      return groupChat.name;
    } else {
      return 'Unknown';
    }
  });

  chatLastMessageContent = computed(() => this.truncateMessageContent(this.chatLastMessage().content));

  chatIcon = computed(() => {
    if (this.chatItem().type === 'private') {
      let privateChat = this.chatItem() as PrivateChatItem;
      return privateChat.participants.find(user => user.id !== this.currentUserId())?.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png";
    } else if (this.chatItem().type === 'group') {
      let groupChat = this.chatItem() as GroupChatItem;
      return groupChat.iconUri || "https://cdn-icons-png.flaticon.com/512/2352/2352167.png";
    } else {
      return '';
    }
  });

  chatLastMessage = () => {
    return this.chatItem().messages[this.chatItem().messages.length - 1];
  }

  openChat(chatId: string) {
    this.router.navigateByUrl(`/chats/${chatId}`);
  }

  private truncateMessageContent(message: string) {
    const maxLength = 30;
    return message.length > maxLength ? message.substring(0, maxLength - 3) + '...' : message;
  }
}
