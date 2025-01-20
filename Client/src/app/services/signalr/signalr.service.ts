import { inject, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { SendMessageCommand } from '../../models/DTO/SendMessageCommand';
import { MessageResponse } from '../../models/DTO/MessageResponse';
import { Error } from '../../models/error/Error';
import { StorageService } from '../storage/storage.service';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private readonly hubUrl = environment.HUB_BASE_URL;
  private readonly hubConnection: HubConnection;

  storageService = inject(StorageService);

  constructor() {
    const accessToken = this.storageService.getAccessTokenFromSessionStorage();

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}/chat`, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
        accessTokenFactory: () => `${accessToken}` })
      .build();
  }

  getHubConnection(): HubConnection {
    return this.hubConnection;
  }

  async connect(): Promise<void> {
    try {
      console.log(`Connecting to SignalR hub: ${this.hubConnection.baseUrl}`);
      await this.hubConnection.start();
      console.log('SignalR connection started');
    } catch (err) {
      console.error('Error while establishing connection.', err);
    }
  }

  async sendMessage(command: SendMessageCommand): Promise<void> {
    return this.hubConnection.invoke('SendMessage', command)
      .then(() => console.log('Message sent successfully'))
      .catch(err => console.error('Error sending message:', err));
  }

  async joinChat(chatId: string): Promise<void> {
    return this.hubConnection.invoke('JoinChat', chatId)
      .then(() => console.log('Joined chat successfully'))
      .catch(err => console.error('Error joining chat:', err));
  }
  
  listenForMessages(onMessageReceived: (message: MessageResponse) => void): void {
    this.hubConnection.on('ReceiveUserMessage', onMessageReceived);
  }
  
  listenForErrors(onErrorReceived: (error: Error) => void): void {
    this.hubConnection.on('ReceiveError', onErrorReceived);
  }
}