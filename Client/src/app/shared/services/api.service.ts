import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LoginRequest } from '../../features/login/models/login-request';
import { Observable } from 'rxjs';
import { LoginResponse } from '../../features/login/models/login-response';
import { RegisterRequest } from '../../features/register/models/register-request';
import { ConfirmEmailCommand } from '../../features/email-confirm/models/confirm-email-command';
import { ValidateEmailConfirmationResponse } from '../../features/email-confirm/models/validate-email-confirmation-response';
import { environment } from '../../../environments/environment';
import { RefreshTokenRequest } from '../models/requests/refresh-token-request';
import { RefreshTokenResponse } from '../models/responses/refresh-token-response';
import { Chat } from '../../features/main/models/chat';
import { PaginatedChatsResponse } from '../../features/main/models/paginated-chats-response';
import { CreatePrivateChatCommand } from '../../features/main/models/create-private-chat-command';
import { PaginatedMessagesResponse } from '../../features/main/models/paginated-messages-response';
import { User } from '../../features/main/models/user';
import { ChatExistsResponse } from '../../features/main/components/aside/chats-aside/add-chat-page/models/chat-exists-response';
import { CreateGroupChatCommand } from '../../features/main/models/create-group-chat-command';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly apiUrl = environment.API_BASE_URL;

  httpClient = inject(HttpClient);

  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(`${this.apiUrl}/login`, loginRequest);
  }

  register(registerRequest: RegisterRequest): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/register`, registerRequest);
  }

  refresh(refreshTokenRequest: RefreshTokenRequest): Observable<RefreshTokenResponse> {
    return this.httpClient.post<RefreshTokenResponse>(`${this.apiUrl}/refresh`, refreshTokenRequest);
  }

  getUserChats(): Observable<Chat[]> {
    return this.httpClient.get<Chat[]>(`${this.apiUrl}/chats`);
  }

  getUserChatsPaginated(page: number, pageSize: number, retrievalCutoff: Date): Observable<PaginatedChatsResponse> {
    const options = {
      params: new HttpParams()
        .set('page', page)
        .set('pageSize', pageSize)
        .set('retrievalCutoff', retrievalCutoff.toISOString())
    }

    return this.httpClient.get<PaginatedChatsResponse>(`${this.apiUrl}/chats/paginated`, options);
  }

  createPrivateChat(command: CreatePrivateChatCommand): Observable<string> {
    return this.httpClient.post<string>(`${this.apiUrl}/private-chats`, command);
  }

  createGroupChat(command: CreateGroupChatCommand): Observable<string> {
    let formData = new FormData();
    const request = {
      invitees: command.invitees,
      name: command.name,
      description: command.description,
      message: command.message
    }

    formData.append('request', JSON.stringify(request));
    if (command.icon) {
      formData.append('icon', command.icon);
    }

    return this.httpClient.post<string>(`${this.apiUrl}/group-chats`, formData);
  }

  getChatById(chatId: string): Observable<Chat> {
    return this.httpClient.get<Chat>(`${this.apiUrl}/chats/${chatId}`);
  }

  searchUsers(query: string): Observable<User[]> {
    const options = {
      params: new HttpParams()
        .set('searchTerm', query)
    };

    return this.httpClient.get<User[]>(`${this.apiUrl}/users/search`, options);
  }

  getPrivateChatExistsBetweenUsers(userId: string) : Observable<ChatExistsResponse> {
    const options = {
      params: new HttpParams()
        .set('userId', userId)
    };

    return this.httpClient.get<ChatExistsResponse>(`${this.apiUrl}/private-chats/exists`, options);
  }

  getChatMessagesPaginated(chatId: string, page: number, pageSize: number, retrievalCutoff: Date): Observable<PaginatedMessagesResponse> {
    const options = { 
      params: new HttpParams()
        .set('page', page)
        .set('pageSize', pageSize)
        .set('retrievalCutoff', retrievalCutoff.toISOString())
    };

    return this.httpClient.get<PaginatedMessagesResponse>(`${this.apiUrl}/chats/${chatId}/messages`, options);
  }

  confirmEmail(confirmEmailCommand: ConfirmEmailCommand) : Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/confirm-email`, confirmEmailCommand);
  }

  validateEmailConfirmation(userId: string, tokenId: string): Observable<ValidateEmailConfirmationResponse> {
    const options = { 
      params: new HttpParams()
        .set('userId', userId)
        .set('tokenId', tokenId)
    };

    return this.httpClient.get<ValidateEmailConfirmationResponse>(`${this.apiUrl}/validate-email-confirmation`, options);
  }
}
