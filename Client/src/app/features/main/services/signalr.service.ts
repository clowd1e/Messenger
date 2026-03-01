import { inject, Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../../environments/environment';
import { StorageService } from '../../../shared/services/storage.service';
import { SendMessageCommand } from '../models/send-message-command';
import { SignalrAccessTokenFactoryService } from '../../../shared/services/signalr-access-token-factory.service';
import { MessageHubResponse } from '../models/message-hub-response';
import { Chat } from '../models/chat';
import { Subject } from 'rxjs/internal/Subject';
import { DeleteMessageHubResponse } from '../models/delete-message-hub-response';
import { UpdateMessageHubResponse } from '../models/update-message-hub-response';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private readonly hubUrl = environment.HUB_BASE_URL;
  private readonly production = environment.production;
  private readonly hubConnection: HubConnection;

  private messageSubject = new Subject<MessageHubResponse>();
  private errorSubject = new Subject<any>();
  private chatCreatedSubject = new Subject<Chat>();
  private deletedMessageSubject = new Subject<DeleteMessageHubResponse>();
  private updatedMessageSubject = new Subject<UpdateMessageHubResponse>();
  
  messages$ = this.messageSubject.asObservable();
  errors$ = this.errorSubject.asObservable();
  chatCreated$ = this.chatCreatedSubject.asObservable();
  deletedMessage$ = this.deletedMessageSubject.asObservable();
  updatedMessage$ = this.updatedMessageSubject.asObservable();

  storageService = inject(StorageService);
  accessTokenFactory = inject(SignalrAccessTokenFactoryService);

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}/chat`, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
        accessTokenFactory: async () => `${await this.accessTokenFactory.getAccessToken()}`
      })
      .withAutomaticReconnect()
      .configureLogging(this.production ? signalR.LogLevel.None : signalR.LogLevel.Information)
      .build();

    this.hubConnection.on('ReceiveUserMessage', (messageResponse: MessageHubResponse) => {
      this.messageSubject.next(messageResponse);
    });
    this.hubConnection.on('ReceiveError', (error: any) => {
      this.errorSubject.next(error);
    });
    this.hubConnection.on('ReceiveChat', (chatResponse: Chat) => {
      this.chatCreatedSubject.next(chatResponse);
    });
    this.hubConnection.on('UpdateMessage', (updatedMessageResponse: UpdateMessageHubResponse) => {
      this.updatedMessageSubject.next(updatedMessageResponse);
    });
    this.hubConnection.on('DeleteMessage', (deletedMessageResponse: DeleteMessageHubResponse) => {
      this.deletedMessageSubject.next(deletedMessageResponse);
    });
  }

  getHubConnection(): HubConnection {
    return this.hubConnection;
  }

  async connect(): Promise<void> {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      //console.log('Already connected to SignalR.');
      return;
    }

    try {
      // console.log(`Connecting to SignalR hub: ${this.hubConnection.baseUrl}`);
      await this.hubConnection.start();
      // console.log('SignalR connection started');
    } catch (err) {
      // console.error('Error while establishing connection.', err);
    }
  }

  async sendMessage(command: SendMessageCommand): Promise<void> {
    return this.hubConnection.invoke('SendMessage', command);
      // .then(() => console.log('Message sent successfully'))
      // .catch(err => console.error('Error sending message:', err));
  }
}
