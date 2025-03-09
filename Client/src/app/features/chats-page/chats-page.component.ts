import { Component, inject, signal } from '@angular/core';
import { ApiService } from '../../shared/services/api/api.service';
import { SignalrService } from './services/signalr/signalr.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserContextService } from '../../shared/services/user-context/user-context.service';
import { UuidHelperService } from '../../shared/services/uuid-helper/uuid-helper.service';
import { map } from 'rxjs';
import { Chat } from './models/Chat';
import { ChatItem } from './models/ChatItem';
import { CreateChatCommand } from './models/CreateChatCommand';
import { SendMessageCommand } from './models/SendMessageCommand';
import { CommonModule } from '@angular/common';
import { AddChatButtonComponent } from './components/add-chat-button/add-chat-button.component';
import { ChatListComponent } from './components/chat-list/chat-list.component';
import { ChatComponent } from './components/chat/chat.component';
import { AddChatComponent } from './components/add-chat/add-chat.component';
import { ErrorHandlerService } from '../../shared/services/error-handler/error-handler.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MapChatToChatItem } from './mappers/ChatToChatItemMapper';
import { Message } from './models/Message';
import { PaginatedChatsResponse } from './models/PaginatedChatsResponse';

@Component({
  selector: 'app-chats-page',
  standalone: true,
  imports: [ChatComponent, AddChatComponent, ChatListComponent, AddChatButtonComponent, CommonModule],
  providers: [SignalrService],
  templateUrl: './chats-page.component.html',
  styleUrl: './chats-page.component.scss'
})
export class ChatsPageComponent {
  chats: ChatItem[] = [];
  selectedChat = signal<ChatItem | undefined>(undefined);

  addChatVisible = signal(false);
  chatsLoading: boolean = false;
  chatRetrievalCutoff = new Date();

  apiService = inject(ApiService);
  signalrService = inject(SignalrService);
  route = inject(ActivatedRoute);
  router = inject(Router);
  userContextService = inject(UserContextService);
  uuidHelper = inject(UuidHelperService);
  errorHandler = inject(ErrorHandlerService);

  currentUserId = this.userContextService.getCurrentUserId();

  ngOnInit() {
    this.connectSignalR();
    this.loadUserChats();

    this.route.url.pipe(map(urlSegments => urlSegments)).subscribe(urlSegments => {
      const isAddRoute = urlSegments.some(segment => segment.path === 'add');
      if (isAddRoute) {
        this.handleAddRoute();
      } else {
        this.handleChatRoute();
      }
    });
  }

  private connectSignalR() : void {
    this.signalrService.connect().then(() => {
      this.signalrService.listenForMessages(this.handleMessageReceived.bind(this));
      this.signalrService.listenForErrors(this.handleErrorReceived.bind(this));
    })
  }

  private loadUserChats() {
    this.chatsLoading = true;
    
    this.apiService.getUserChatsPaginated(1, 10, this.chatRetrievalCutoff).subscribe({
      next: (response: PaginatedChatsResponse) => {
        let chatItems: ChatItem[] = response.chats.map(MapChatToChatItem);
        this.chats = chatItems;
      },
      error: (httpError: HttpErrorResponse) => {
        this.errorHandler.handleHttpError(httpError);
      }
    });

    this.chatsLoading = false;
  }

  private handleChatRoute() {
    this.addChatVisible.set(false);
    const chatId = this.uuidHelper.toUuid(this.route.snapshot.paramMap.get('chatId'));

    if (chatId) {
      this.signalrService.joinChat(chatId);

      if (this.chats.length > 0) {
        let chat = this.chats.find(chat => chat.id === chatId);;
        this.selectedChat.set(chat);
      } else {
        const checkChatInterval = setInterval(() => {
          if (this.chats.length > 0) {
            let chat = this.chats.find(chat => chat.id === chatId);;
            this.selectedChat.set(chat);

            clearInterval(checkChatInterval);
          }
        }, 0);
      }
    } else {
      this.selectedChat.set(undefined);
    }
  }


  private handleAddRoute() {
    const userId = this.uuidHelper.toUuid(this.route.snapshot.paramMap.get('userId'));
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

  sendMessageToChat(message: string) {
    if (!this.selectedChat) {
      return;
    }

    const isAddRoute = this.route.snapshot.url.some(segment => segment.path === 'add');
    const userId = this.uuidHelper.toUuid(this.route.snapshot.paramMap.get('userId'));

    if (isAddRoute && userId) {
      let command: CreateChatCommand = {
        inviteeId: userId,
        message: message
      }

      this.apiService.createChat(command).subscribe({
        next: (chatId: string) => {
          this.apiService.getChatById(chatId).subscribe({
            next: (chat: Chat) => {
              let chatItem: ChatItem = MapChatToChatItem(chat);
              this.selectedChat = signal(chatItem);
              
              this.router.navigateByUrl(`/chats/${chat.id}`);
            }
          });
        },
        error: (httpError: any) => {
          this.errorHandler.handleHttpError(httpError);
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

    this.signalrService.sendMessage(command);
  }

  private handleMessageReceived(message: Message): void {
    console.log('Message received:', message);
    if (this.selectedChat()) {
      this.selectedChat()!.messages = [...this.selectedChat()!.messages, message];
    }
  }

  private handleErrorReceived(error: any): void {
    this.errorHandler.handleError(error);
  }

}

