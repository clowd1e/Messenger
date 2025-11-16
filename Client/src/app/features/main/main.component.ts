import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UuidHelperService } from '../../shared/services/uuid-helper.service';
import { map } from 'rxjs';
import { Chat } from './models/chat';
import { ChatItem } from './models/chat-item';
import { CreatePrivateChatCommand } from './models/create-private-chat-command';
import { SendMessageCommand } from './models/send-message-command';
import { CommonModule } from '@angular/common';
import { ChatComponent } from './components/chat/chat.component';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MapChatToChatItem } from './mappers/chat-to-chat-item.mapper';
import { Message } from './models/message';
import { PaginatedChatsResponse } from './models/paginated-chats-response';
import { UserContextService } from '../../shared/services/user-context.service';
import { ApiService } from '../../shared/services/api.service';
import { SignalrService } from './services/signalr.service';
import { ChatsAsideComponent } from "./components/aside/chats-aside/chats-aside.component";
import { PrivateChatItem } from './models/private-chat-item';
import { ChatExistsResponse } from './components/aside/chats-aside/add-chat-page/models/chat-exists-response';
import { GroupCreationStore } from './components/aside/chats-aside/add-chat-page/services/group-creation-store.service';
import { GroupChatItem } from './models/group-chat-item';
import { CreateGroupChatCommand } from './models/create-group-chat-command';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [ChatComponent, CommonModule, ChatsAsideComponent],
  providers: [SignalrService],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent implements OnInit {
  chats: ChatItem[] = [];
  selectedChat = signal<ChatItem | undefined>(undefined);

  isAddPrivateChatRoute = signal<boolean>(false);
  isAddGroupChatRoute = signal<boolean>(false);
  chatsLoading: boolean = false;
  chatRetrievalCutoff = new Date();

  apiService = inject(ApiService);
  signalrService = inject(SignalrService);
  route = inject(ActivatedRoute);
  router = inject(Router);
  userContextService = inject(UserContextService);
  uuidHelper = inject(UuidHelperService);
  errorHandler = inject(ErrorHandlerService);
  groupCreationStore = inject(GroupCreationStore);

  currentUserId = this.userContextService.getCurrentUserId();

  ngOnInit() {
    this.connectSignalR();
    this.loadUserChats();

    this.route.url.pipe(map(urlSegments => urlSegments)).subscribe(urlSegments => {
      const isAddRoute = urlSegments.some(segment => segment.path === 'add');
      if (isAddRoute) {
        if (urlSegments.some(segment => segment.path === 'group')) {
          this.isAddGroupChatRoute.set(true);
          this.isAddPrivateChatRoute.set(false);
          this.handleAddGroupRoute();
        } else if (urlSegments.some(segment => segment.path === 'private')) {
          this.isAddPrivateChatRoute.set(true);
          this.isAddGroupChatRoute.set(false);
          this.handleAddPrivateRoute();
        }
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

  private handleAddGroupRoute() {
    let groupChat = this.groupCreationStore.getGroupChat();
    if (!groupChat) {
      this.router.navigate(['chats']);
    }

    let groupChatItem: GroupChatItem = {
      id: 'temp-group-chat-id',
      type: 'group',
      name: groupChat!.name,
      description: groupChat!.description,
      creationDate: '',
      iconUri: null,
      messages: [],
      participants: []
    }

    this.selectedChat.set(groupChatItem);
  }

  private handleAddPrivateRoute() {
    const userId = this.uuidHelper.toUuid(this.route.snapshot.paramMap.get('userId'));

    this.apiService.getPrivateChatExistsBetweenUsers(userId).subscribe({
      next: (response: ChatExistsResponse) => {
        if (response.chatId) {
          let chatId = this.uuidHelper.toShortUuid(response.chatId);
          this.router.navigate(['chats', chatId]);
        }
      },
      error: (error: HttpErrorResponse) => {
        this.errorHandler.handleHttpError(error);
      }
    });

    if (userId) {
      let chatItem: PrivateChatItem = {
        id: 'temp-private-chat-id',
        type: 'private',
        creationDate: '',
        messages: [],
        participants: []
      };

      this.selectedChat.set(chatItem);
    } else {
      this.selectedChat.set(undefined);
    }
  }

  sendMessageToChat(message: string) {
    if (!this.selectedChat) {
      return;
    }

    const userId = this.uuidHelper.toUuid(this.route.snapshot.paramMap.get('userId'));

    if (this.isAddPrivateChatRoute() && userId) {
      let command: CreatePrivateChatCommand = {
        inviteeId: userId,
        message: message
      }

      this.apiService.createPrivateChat(command).subscribe({
        next: (chatId: string) => {
          this.apiService.getChatById(chatId).subscribe({
            next: (chat: Chat) => {
              let chatItem: ChatItem = MapChatToChatItem(chat);
              this.chats = [chatItem, ...this.chats];
              this.selectedChat.set(chatItem);
              
              let shortChatId = this.uuidHelper.toShortUuid(chat.id);
              this.router.navigateByUrl(`/chats/${shortChatId}`);
            }
          });
        },
        error: (httpError: any) => {
          this.errorHandler.handleHttpError(httpError);
        }
      });
    } else if (this.isAddGroupChatRoute()) {
      let groupChat = this.groupCreationStore.getGroupChat();
      if (!groupChat) {
        return;
      }

      let command: CreateGroupChatCommand = {
        invitees: groupChat.participantIds,
        name: groupChat.name,
        description: groupChat.description,
        icon: groupChat.icon,
        message: message
      }

      this.apiService.createGroupChat(command).subscribe({
        next: (chatId: string) => {
          this.apiService.getChatById(chatId).subscribe({
            next: (chat: Chat) => {
              let chatItem: ChatItem = MapChatToChatItem(chat);
              this.chats = [chatItem, ...this.chats];
              this.selectedChat.set(chatItem);
              
              let shortChatId = this.uuidHelper.toShortUuid(chat.id);
              this.router.navigateByUrl(`/chats/${shortChatId}`);
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
    if (this.selectedChat()) {
      this.selectedChat()!.messages = [...this.selectedChat()!.messages, message];
    }
  }

  private handleErrorReceived(error: any): void {
    this.errorHandler.handleError(error);
  }
}

