import { Component, computed, inject, input } from '@angular/core';
import { MainStorageService } from '../../../services/main-storage.service';
import { PrivateChat } from '../../../models/private-chat';
import { GroupChat } from '../../../models/group-chat';
import { GroupCreationStore } from '../../aside/chats-aside/add-chat-page/services/group-creation-store.service';

@Component({
  selector: 'app-chat-info-header',
  standalone: true,
  imports: [],
  templateUrl: './chat-info-header.component.html',
  styleUrl: './chat-info-header.component.scss'
})
export class ChatInfoHeaderComponent {
  isAddGroupChatRoute = input.required<boolean>();

  mainStorage = inject(MainStorageService);
  groupCreationStore = inject(GroupCreationStore);

  chatIcon = computed(() => {
    if (this.isAddGroupChatRoute()) {
      let groupIcon = this.groupCreationStore.getGroupChatIcon();
      if (groupIcon) {
        return URL.createObjectURL(groupIcon);
      }
      return "https://cdn-icons-png.flaticon.com/512/2352/2352167.png";
    }
    if (this.mainStorage.SelectedChat()?.type === 'private') {
      let privateChat = this.mainStorage.SelectedChat() as PrivateChat;
      return privateChat.participants.find(user => user.id !== this.mainStorage.CurrentUserId)?.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png";
    } else if (this.mainStorage.SelectedChat()?.type === 'group') {
      let groupChat = this.mainStorage.SelectedChat() as GroupChat;
      return groupChat.iconUri || "https://cdn-icons-png.flaticon.com/512/2352/2352167.png";
    }

    return "https://cdn-icons-png.flaticon.com/512/149/149071.png";
  });

  chatName = computed(() => {
    if (this.isAddGroupChatRoute()) {
      let groupChat = this.groupCreationStore.getGroupChatName();
      return groupChat ? groupChat : 'New Group';
    }
    if (this.mainStorage.SelectedChat()!.type === 'private') {
      let privateChat = this.mainStorage.SelectedChat() as PrivateChat;
      return privateChat.participants.find(user => user.id !== this.mainStorage.CurrentUserId)?.name || 'Unknown';
    } else if (this.mainStorage.SelectedChat()!.type === 'group') {
      let groupChat = this.mainStorage.SelectedChat() as GroupChat;
      return groupChat.name;
    } else {
      return 'Unknown';
    }
  });
}
