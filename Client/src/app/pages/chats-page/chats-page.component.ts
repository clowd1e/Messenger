import { Component, inject, input, signal } from '@angular/core';
import { ChatListComponent } from "../../components/chat/chat-list/chat-list.component";
import { ChatComponent } from "../../components/chat/chat/chat.component";
import { SignalrService } from '../../services/signalr/signalr.service';
import { ChatItem } from '../../models/ChatItem';
import { ActivatedRoute, Router } from '@angular/router';
import { SendMessageCommand } from '../../models/DTO/SendMessageCommand';
import { MessageResponse } from '../../models/DTO/MessageResponse';
import { UserContextService } from '../../services/auth/user-context.service';
import { DefaultButtonComponent } from "../../components/buttons/default-button/default-button.component";
import { Error } from '../../models/error/Error';
import { UserListComponent } from "../../components/user/user-list/user-list.component";
import { CommonModule } from '@angular/common';
import { AddChatComponent } from "../../components/chat/add-chat/add-chat.component";
import { TimeInterval } from 'rxjs/internal/operators/timeInterval';
import { ApiService } from '../../services/api/api.service';
import { CreateChatCommand } from '../../models/chat/CreateChatCommand';
import { map } from 'rxjs';

@Component({
  selector: 'app-chats-page',
  standalone: true,
  imports: [CommonModule, ChatListComponent, ChatComponent, DefaultButtonComponent, AddChatComponent],
  providers: [SignalrService],
  templateUrl: './chats-page.component.html',
  styleUrl: './chats-page.component.scss'
})
export class ChatsPageComponent {
  addChatVisible = signal(false);

  userChats: Array<ChatItem> = [];

  userChatsLoading: boolean = false;

  selectedChat = signal<ChatItem | undefined>(undefined);

  apiService = inject(ApiService);
  signalrService = inject(SignalrService);
  route = inject(ActivatedRoute);
  router = inject(Router);
  userContextService = inject(UserContextService);

  currentUserId = this.userContextService.getCurrentUserId();

  ngOnInit() {
    this.connectSignalR();

    this.route.url.pipe(map(urlSegments => urlSegments)).subscribe(urlSegments => {
      const isAddRoute = urlSegments.some(segment => segment.path === 'add');
      if (isAddRoute) {
        this.handleAddRoute();
      } else {
        this.handleChatRoute();
      }
    });

    this.signalrService.listenForMessages(this.handleMessageReceived.bind(this));
    this.signalrService.listenForErrors(this.handleErrorReceived.bind(this));
  }

  private handleChatRoute() {
    this.addChatVisible.set(false);
    const chatId = this.route.snapshot.paramMap.get('chatId');
    if (chatId) {
      this.signalrService.joinChat(chatId);
      this.selectedChat.set(this.userChats.find(chat => chat.id === chatId));
    } else {
      this.selectedChat.set(undefined);
    }
  }

  private handleAddRoute() {
    const userId = this.route.snapshot.paramMap.get('userId');
    if (userId) {
      let chatItem: ChatItem = {
        id: '',
        creationDate: '',
        messages: [],
        users: [this.currentUserId, userId]
      };

      this.addChatVisible.set(false);
      this.selectedChat.set(chatItem);
    } else {
      this.addChatVisible.set(true);
      this.selectedChat.set(undefined);
    }
  }

  navigateToAddChat() {
    this.router.navigateByUrl('chats/add');
  }

  private connectSignalR() : void {
    this.userChatsLoading = true;

    this.signalrService.connect().then(() => {
      this.signalrService.getHubConnection().on('ReceiveUserChats',
        (chats: Array<ChatItem>) => {
          this.userChats = chats;
          this.userChatsLoading = false;
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

    const isAddRoute = this.route.snapshot.url.some(segment => segment.path === 'add');
    const userId = this.route.snapshot.paramMap.get('userId');

    if (isAddRoute && userId) {
      let command: CreateChatCommand = {
        inviteeId: userId
      }

      this.apiService.createChat(command).subscribe({
        next: (chatId: string) => {
          this.apiService.getChatById(chatId).subscribe({
            next: (chat: ChatItem) => {
              this.selectedChat = signal(chat);

              this.sendMessage(message);
  
              this.router.navigateByUrl(`/chats/${chat.id}`);
            }
          });
        },
        error: (httpError: any) => {
          console.log(httpError);
        }
      });
    } else {
      this.sendMessage(message);
    }
  }

  private sendMessage(message: string) {
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
