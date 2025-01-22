import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { StorageService } from '../../../../shared/services/storage/storage.service';
import { SendMessageCommand } from '../../models/SendMessageCommand';
import { MessageResponse } from '../../models/MessageResponse';

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
      .configureLogging(environment.production ? signalR.LogLevel.None : signalR.LogLevel.Information)
      .build();
  }

  getHubConnection(): HubConnection {
    return this.hubConnection;
  }

  async connect(): Promise<void> {
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

  async joinChat(chatId: string): Promise<void> {
    return this.hubConnection.invoke('JoinChat', chatId);
      // .then(() => console.log('Joined chat successfully'))
      // .catch(err => console.error('Error joining chat:', err));
  }
  
  listenForMessages(onMessageReceived: (message: MessageResponse) => void): void {
    this.hubConnection.on('ReceiveUserMessage', onMessageReceived);
  }
  
  listenForErrors(onErrorReceived: (error: Error) => void): void {
    this.hubConnection.on('ReceiveError', onErrorReceived);
  }
}
