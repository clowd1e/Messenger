import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LoginRequest } from '../../models/auth/LoginRequest';
import { environment } from '../../../environments/environment';
import { CreateChatCommand } from '../../models/chat/CreateChatCommand';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly apiUrl = environment.apiBaseUrl;

  httpClient = inject(HttpClient);

  login(loginRequest: LoginRequest): any {
    return this.httpClient.post(`${this.apiUrl}/auth/login`, loginRequest);
  }

  getAllUsers(): any {
    return this.httpClient.get(`${this.apiUrl}/users`);
  }

  createChat(createChatCommand: CreateChatCommand): any {
    return this.httpClient.post(`${this.apiUrl}/chats`, createChatCommand);
  }

  getChatById(chatId: string): any {
    return this.httpClient.get(`${this.apiUrl}/chats/${chatId}`);
  }
}
