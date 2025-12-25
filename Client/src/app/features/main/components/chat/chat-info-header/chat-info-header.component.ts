import { Component, computed, inject } from '@angular/core';
import { MainStorageService } from '../../../services/main-storage.service';
import { PrivateChat } from '../../../models/private-chat';
import { GroupChat } from '../../../models/group-chat';

@Component({
  selector: 'app-chat-info-header',
  standalone: true,
  imports: [],
  templateUrl: './chat-info-header.component.html',
  styleUrl: './chat-info-header.component.scss'
})
export class ChatInfoHeaderComponent {
  mainStorage = inject(MainStorageService);

  chatIcon = computed(() => {
    if (this.mainStorage.SelectedChat()!.type === 'private') {
      let privateChat = this.mainStorage.SelectedChat() as PrivateChat;
      return privateChat.participants.find(user => user.id !== this.mainStorage.CurrentUserId)?.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png";
    } else if (this.mainStorage.SelectedChat()!.type === 'group') {
      let groupChat = this.mainStorage.SelectedChat() as GroupChat;
      return groupChat.iconUri || "https://cdn-icons-png.flaticon.com/512/2352/2352167.png";
    } else {
      return '';
    }
  });

  chatName = computed(() => {
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
