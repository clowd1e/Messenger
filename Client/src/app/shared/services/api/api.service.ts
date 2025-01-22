import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { LoginRequest } from '../../../features/login/models/LoginRequest';
import { RegisterRequest } from '../../../features/signup/models/RegisterRequest';
import { CreateChatCommand } from '../../../features/chats-page/models/CreateChatCommand';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
private readonly apiUrl = environment.API_BASE_URL;

  httpClient = inject(HttpClient);

  login(loginRequest: LoginRequest): any {
    return this.httpClient.post(`${this.apiUrl}/auth/login`, loginRequest);
  }

  register(registerRequest: RegisterRequest): any {
    return this.httpClient.post(`${this.apiUrl}/auth/register`, registerRequest);
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
