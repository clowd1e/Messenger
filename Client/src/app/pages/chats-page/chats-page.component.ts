import { Component, inject, signal } from '@angular/core';
import { ChatListComponent } from "../../components/main/chat-list/chat-list.component";
import { ChatComponent } from "../../components/main/chat/chat.component";
import { SignalrService } from '../../services/signalr/signalr.service';
import { ChatItem } from '../../Models/ChatItem';
import { Error } from '../../Models/error/Error';
import { Message } from '../../Models/Message';
import { ActivatedRoute } from '@angular/router';
import { SendMessageCommand } from '../../Models/DTO/SendMessageCommand';
import { MessageResponse } from '../../Models/DTO/MessageResponse';
import { UserContextService } from '../../services/auth/user-context.service';

@Component({
  selector: 'app-chats-page',
  standalone: true,
  imports: [ChatListComponent, ChatComponent],
  providers: [SignalrService],
  templateUrl: './chats-page.component.html',
  styleUrl: './chats-page.component.scss'
})
export class ChatsPageComponent {
  userChats: Array<ChatItem> = [];

  selectedChat = signal<ChatItem | undefined>(undefined);

  signalrService = inject(SignalrService);
  route = inject(ActivatedRoute);
  userContextService = inject(UserContextService);

  currentUserId = this.userContextService.getCurrentUserId();

  ngOnInit() {
    this.connectSignalR();

    this.route.paramMap.subscribe(params => {
      const chatId = params.get('chatId');
      if (chatId) {
        this.signalrService.joinChat(chatId);
        this.selectedChat.set(this.userChats.find(chat => chat.id === chatId));
      }
    });

    this.signalrService.listenForMessages(this.handleMessageReceived.bind(this));
    this.signalrService.listenForErrors(this.handleErrorReceived.bind(this));
  }

  private connectSignalR() : void {
    this.signalrService.connect().then(() => {
      this.signalrService.getHubConnection().on('ReceiveUserChats',
        (chats: Array<ChatItem>) => {
          this.userChats = chats;
          console.log(chats);
      });

      this.signalrService.getHubConnection().on('ReceiveError',
        (error: Error) => {
          console.log(error);
        }
      )
    })
  }

  sendMessageToChat(message: string) {
    if (!this.selectedChat) {
      console.error('No chat selected');
      return;
    }

    let command: SendMessageCommand = {
      chatId: this.selectedChat()?.id || '',
      message: message
    };
  
    this.signalrService
      .sendMessage(command)
      .then(() => console.log('Message sent'))
      .catch(err => console.error('Error sending message:', err));
  }

  private handleMessageReceived(message: MessageResponse): void {
    console.log('New message received:', message);

    if (this.selectedChat) {
      this.selectedChat()?.messages.push(message);
    }
  }

  private handleErrorReceived(error: Error): void {
    console.error('Error received from SignalR:', error);
  }

}
