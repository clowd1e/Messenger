import { Component, computed, input } from '@angular/core';
import { ChatItem } from '../../../models/chat-item';
import { PrivateChatItem } from '../../../models/private-chat-item';
import { GroupChatItem } from '../../../models/group-chat-item';

@Component({
  selector: 'app-chat-info-header',
  standalone: true,
  imports: [],
  templateUrl: './chat-info-header.component.html',
  styleUrl: './chat-info-header.component.scss'
})
export class ChatInfoHeaderComponent {
  chat = input.required<ChatItem | undefined>();
  currentUserId = input.required<string>();

  chatIcon = computed(() => {
    if (this.chat()!.type === 'private') {
      let privateChat = this.chat() as PrivateChatItem;
      return privateChat.participants.find(user => user.id !== this.currentUserId())?.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png";
    } else if (this.chat()!.type === 'group') {
      let groupChat = this.chat() as GroupChatItem;
      return groupChat.iconUri || "https://cdn-icons-png.flaticon.com/512/2352/2352167.png";
    } else {
      return '';
    }
  });

  chatName = computed(() => {
    if (this.chat()!.type === 'private') {
      let privateChat = this.chat() as PrivateChatItem;
      return privateChat.participants.find(user => user.id !== this.currentUserId())?.name || 'Unknown';
    } else if (this.chat()!.type === 'group') {
      let groupChat = this.chat() as GroupChatItem;
      return groupChat.name;
    } else {
      return 'Unknown';
    }
  });
}
