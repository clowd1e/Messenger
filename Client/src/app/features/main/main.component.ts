import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UuidHelperService } from '../../shared/services/uuid-helper.service';
import { firstValueFrom, map, Subscription } from 'rxjs';
import { Chat } from './models/chat';
import { CreatePrivateChatCommand } from './models/create-private-chat-command';
import { SendMessageCommand } from './models/send-message-command';
import { CommonModule } from '@angular/common';
import { ChatComponent } from './components/chat/chat.component';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { HttpErrorResponse } from '@angular/common/http';
import { PaginatedChatsResponse } from './models/paginated-chats-response';
import { UserContextService } from '../../shared/services/user-context.service';
import { ApiService } from '../../shared/services/api.service';
import { ChatsAsideComponent } from "./components/aside/chats-aside/chats-aside.component";
import { ChatExistsResponse } from './components/aside/chats-aside/add-chat-page/models/chat-exists-response';
import { GroupCreationStore } from './components/aside/chats-aside/add-chat-page/services/group-creation-store.service';
import { CreateGroupChatCommand } from './models/create-group-chat-command';
import { MainStorageService } from './services/main-storage.service';
import { GroupChat } from './models/group-chat';
import { DeleteMessageHubResponse } from './models/delete-message-hub-response';
import { PrivateChat } from './models/private-chat';
import { environment } from '../../../environments/environment';
import { StorageService } from '../../shared/services/storage.service';
import { MessageHubResponse } from './models/message-hub-response';
import { User } from './models/user';
import { SignalrService } from './services/signalr.service';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [ChatComponent, CommonModule, ChatsAsideComponent],
  providers: [SignalrService],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent implements OnInit, OnDestroy {
  private messagesSubscription: Subscription | null = null;
  private errorSubscription: Subscription | null = null;
  private chatCreatedSubscription: Subscription | null = null;
  private deletedMessageSubscription: Subscription | null = null;

  isAddPrivateChatRoute = signal<boolean>(false);
  isAddGroupChatRoute = signal<boolean>(false);
  chatsLoading: boolean = false;
  chatRetrievalCutoff = new Date();

  apiService = inject(ApiService);
  route = inject(ActivatedRoute);
  router = inject(Router);
  userContextService = inject(UserContextService);
  uuidHelper = inject(UuidHelperService);
  errorHandler = inject(ErrorHandlerService);
  groupCreationStore = inject(GroupCreationStore);
  storageService = inject(StorageService);
  mainStorage = inject(MainStorageService);
  signalrService = inject(SignalrService);

  async ngOnInit() {
    this.loadUserThemePreference();
    this.mainStorage.unsetSelectedChat();
    this.mainStorage.CurrentUserId = this.userContextService.getCurrentUserId();
    this.connectSignalR();
    await this.loadUserChats();

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

  ngOnDestroy() {
    this.disposeSignalRConnections();
  }

  loadUserThemePreference() {
    let themePreference = this.storageService.getThemePreference();
    let isDarkTheme = themePreference === 'dark';
    document.documentElement.classList.toggle('dark-theme', isDarkTheme);
  }

  private connectSignalR() : void {
    this.signalrService.connect();
    this.messagesSubscription = this.signalrService.messages$.subscribe(
      (messageResponse: MessageHubResponse) => {
        this.handleMessageReceived(messageResponse);
      }
    );
    this.errorSubscription = this.signalrService.errors$.subscribe(
      (error: any) => {
        this.handleErrorReceived(error);
      }
    );
    this.chatCreatedSubscription = this.signalrService.chatCreated$.subscribe(
      (chatResponse: Chat) => {
        this.handleChatCreated(chatResponse);
      }
    );
    this.deletedMessageSubscription = this.signalrService.deletedMessage$.subscribe(
      (deletedMessageResponse: DeleteMessageHubResponse) => {
        this.handleMessageDeleted(deletedMessageResponse);
      }
    );
  }

  private disposeSignalRConnections() : void {
    if (this.messagesSubscription) {
      this.messagesSubscription.unsubscribe();
      this.messagesSubscription = null;
    }
    if (this.errorSubscription) {
      this.errorSubscription.unsubscribe();
      this.errorSubscription = null;
    }
    if (this.chatCreatedSubscription) {
      this.chatCreatedSubscription.unsubscribe();
      this.chatCreatedSubscription = null;
    }
    if (this.deletedMessageSubscription) {
      this.deletedMessageSubscription.unsubscribe();
      this.deletedMessageSubscription = null;
    }
  }

  private async loadUserChats() : Promise<void> {
    this.chatsLoading = true;

    await firstValueFrom(this.apiService.getUserChatsPaginated(1, environment.CHATS_PAGE_SIZE, this.chatRetrievalCutoff))
    .then((res: PaginatedChatsResponse) => {
      this.mainStorage.setChats(res.chats);
    })
    .catch((httpError: HttpErrorResponse) => {
      this.errorHandler.handleHttpError(httpError);
    })
    .finally(() => {
      this.chatsLoading = false;
    });
  }

  private handleChatRoute() {
    const chatId = this.uuidHelper.toUuid(this.route.snapshot.paramMap.get('chatId'));

    if (chatId) {
      this.mainStorage.selectChat(chatId);
    } else {
      this.mainStorage.unsetSelectedChat();
    }
  }

  private handleAddGroupRoute() {
    let createGroupChat = this.groupCreationStore.getGroupChat();
    if (!createGroupChat) {
      this.router.navigate(['chats']);
    }

    let groupChat: GroupChat = {
      id: 'temp-group-chat-id',
      type: 'group',
      name: createGroupChat!.name,
      description: createGroupChat!.description,
      creationDate: '',
      iconUri: null,
      lastMessage: null!,
      participants: []
    }

    this.mainStorage.selectChatByItem(groupChat);
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
      this.apiService.getUserById(userId!).subscribe({
        next: (user: User) => {
          let privateChat: PrivateChat = {
            id: 'temp-private-chat-id',
            type: 'private',
            creationDate: '',
            lastMessage: null!,
            participants: [user]
          };

          this.mainStorage.selectChatByItem(privateChat);
        }
      });
    } else {
      this.mainStorage.unsetSelectedChat();
    }
  }

  sendMessageToChat(message: string) {
    if (!this.mainStorage.SelectedChat()) {
      return;
    }

    const userId = this.uuidHelper.toUuid(this.route.snapshot.paramMap.get('userId'));

    if (this.isAddPrivateChatRoute() && userId) {
      let command: CreatePrivateChatCommand = {
        inviteeId: userId,
        message: message
      }

      this.apiService.createPrivateChat(command).subscribe({
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
      chatId: this.mainStorage.SelectedChat()?.id || '',
      message: message
    };

    this.signalrService.sendMessage(command);
  }

  private handleMessageReceived(messageResponse: MessageHubResponse): void {
    this.mainStorage.appendMessageToChat(messageResponse);
  }

  private handleErrorReceived(error: any): void {
    this.errorHandler.handleError(error);
  }

  private handleChatCreated(chatResponse: Chat): void {
    this.mainStorage.insertAtStart(chatResponse);
    if (this.isAddPrivateChatRoute() || this.isAddGroupChatRoute()) {
      // this.mainStorage.selectChat(chatResponse.id);
      // this.isAddPrivateChatRoute.set(false);
      // this.isAddGroupChatRoute.set(false);
      this.router.navigate(['chats', this.uuidHelper.toShortUuid(chatResponse.id)]);
    }
  }

  private handleMessageDeleted(deletedMessageResponse: DeleteMessageHubResponse): void {
    this.mainStorage.removeMessage(
      deletedMessageResponse.chatId,
      deletedMessageResponse.messageId);
  }
}

