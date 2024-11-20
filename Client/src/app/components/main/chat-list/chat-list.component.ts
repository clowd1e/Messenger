import { Component } from '@angular/core';
import { ChatItemComponent } from "../chat-item/chat-item.component";
import { ChatItem } from '../../../Models/ChatItem';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [ChatItemComponent],
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.scss'
})
export class ChatListComponent {
  chatList: Array<ChatItem> = [
    { chatId: '1', chatCreationDate: new Date(), users: [
      { userId: '1', userName: 'User 1', email: 'user1@gmail.com' },
      { userId: '2', userName: 'User 2', email: 'user2@gmail.com' },
      ], messages: [
        { messageContent: 'Hello', messageTimestamp: new Date(2024, 11, 20, 12, 14, 32), senderId: '1' },
        { messageContent: 'Hi', messageTimestamp: new Date(2024, 11, 20, 14, 12, 40), senderId: '2' },
      ]
    },
    { chatId: '2', chatCreationDate: new Date(), users: [
      { userId: '1', userName: 'User 1', email: 'user1@gmail.com' },
      { userId: '3', userName: 'User 3', email: 'user3@gmail.com' },
      ], messages: [
        { messageContent: 'Hello', messageTimestamp: new Date(2024, 11, 20, 12, 14, 32), senderId: '1' },
        { messageContent: 'Hi', messageTimestamp: new Date(2024, 11, 20, 14, 12, 40), senderId: '3' },
        { messageContent: 'How are you?', messageTimestamp: new Date(2024, 11, 20, 14, 13, 43), senderId: '1' },
        { messageContent: 'Well well well', messageTimestamp: new Date(2024, 11, 20, 14, 14, 50), senderId: '3' },
      ]
   },
    { chatId: '3', chatCreationDate: new Date(), users: [
      { userId: '1', userName: 'User 1', email: 'user1@gmail.com' },
      { userId: '4', userName: 'User 4', email: 'user4@gmail.com' },
      ], messages: [
        { messageContent: 'Hello', messageTimestamp: new Date(2024, 11, 20, 12, 14, 32), senderId: '1' },
        { messageContent: 'Hi', messageTimestamp: new Date(2024, 11, 20, 14, 12, 40), senderId: '4' },
        { messageContent: 'Yes, I would certainly like to discuss that.', messageTimestamp: new Date(2024, 11, 20, 16, 13, 2), senderId: '4' },
      ]
    },
  ];
}
