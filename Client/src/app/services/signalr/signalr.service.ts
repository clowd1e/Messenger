import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private readonly hubUrl = environment.hubBaseUrl;
  private readonly hubConnection: HubConnection;

  constructor() {
    const accessToken = localStorage.getItem('accessToken') || '';

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
}