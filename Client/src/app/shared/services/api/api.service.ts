import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { LoginRequest } from '../../../features/login/models/LoginRequest';
import { CreateChatCommand } from '../../../features/chats-page/models/CreateChatCommand';
import { Observable } from 'rxjs';
import { LoginResponse } from '../../../features/login/models/LoginResponse';
import { UserItem } from '../../../features/chats-page/components/add-chat/models/UserItem';
import { ChatItem } from '../../../features/chats-page/models/ChatItem';
import { RegisterRequest } from '../../../features/register/models/RegisterRequest';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
private readonly apiUrl = environment.API_BASE_URL;

  httpClient = inject(HttpClient);

  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(`${this.apiUrl}/auth/login`, loginRequest);
  }

  register(registerRequest: RegisterRequest): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/auth/register`, registerRequest);
  }

  getAllUsersExceptCurrent(): Observable<UserItem[]> {
    return this.httpClient.get<UserItem[]>(`${this.apiUrl}/users/except-current`);
  }

  createChat(createChatCommand: CreateChatCommand): Observable<string> {
    return this.httpClient.post<string>(`${this.apiUrl}/chats`, createChatCommand);
  }

  getChatById(chatId: string): Observable<ChatItem> {
    return this.httpClient.get<ChatItem>(`${this.apiUrl}/chats/${chatId}`);
  }
}
