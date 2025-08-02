import { inject, Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../../environments/environment';
import { StorageService } from '../../../shared/services/storage.service';
import { SendMessageCommand } from '../models/send-message-command';
import { Message } from '../models/message';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private readonly hubUrl = environment.HUB_BASE_URL;
  private readonly production = environment.production;
  private readonly hubConnection: HubConnection;

  storageService = inject(StorageService);

  constructor() {
    const accessToken = this.storageService.getAccessTokenFromSessionStorage();

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}/chat`, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
        accessTokenFactory: () => `${accessToken}` })
      .withAutomaticReconnect()
      .configureLogging(this.production ? signalR.LogLevel.None : signalR.LogLevel.Information)
      .build();
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
      //console.log(`Connecting to SignalR hub: ${this.hubConnection.baseUrl}`);
      await this.hubConnection.start();
      //console.log('SignalR connection started');
    } catch (err) {
      //console.error('Error while establishing connection.', err);
    }
  }

  async sendMessage(command: SendMessageCommand): Promise<void> {
    return this.hubConnection.invoke('SendMessage', command);
      // .then(() => console.log('Message sent successfully'))
      // .catch(err => console.error('Error sending message:', err));
  }

  async joinChat(chatId: string): Promise<void> {
    await this.ensureConnected();

    return this.hubConnection.invoke('JoinChat', chatId);
    //   .then(() => console.log('Joined chat successfully'))
    //   .catch(err => console.error('Error joining chat:', err));
  }

  private async ensureConnected(): Promise<void> {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      // console.log('SignalR is already connected.');
      return;
    }
  
    if (this.hubConnection.state === signalR.HubConnectionState.Connecting) {
      // console.log('SignalR is already connecting. Waiting...');
      while (this.hubConnection.state === signalR.HubConnectionState.Connecting) {
        await new Promise(resolve => setTimeout(resolve, 100));
      }
      return;
    }
  
    if (this.hubConnection.state !== signalR.HubConnectionState.Disconnected) {
      // console.warn(`Cannot start SignalR connection because the state is: ${this.hubConnection.state}`);
      return;
    }
  
    // console.log('Starting SignalR connection...');
    await this.connect();
  }
  
  listenForMessages(onMessageReceived: (message: Message) => void): void {
    this.hubConnection.on('ReceiveUserMessage', onMessageReceived);
  }
  
  listenForErrors(onErrorReceived: (error: any) => void): void {
    this.hubConnection.on('ReceiveError', onErrorReceived);
  }
}
