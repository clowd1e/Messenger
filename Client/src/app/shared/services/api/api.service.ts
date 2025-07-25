import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LoginRequest } from '../../../features/login/models/LoginRequest';
import { CreateChatCommand } from '../../../features/chats-page/models/CreateChatCommand';
import { Observable } from 'rxjs';
import { LoginResponse } from '../../../features/login/models/LoginResponse';
import { UserItem } from '../../../features/chats-page/components/add-chat/models/UserItem';
import { RegisterRequest } from '../../../features/register/models/RegisterRequest';
import { Chat } from '../../../features/chats-page/models/Chat';
import { PaginatedMessagesResponse } from '../../../features/chats-page/models/PaginatedMessagesResponse';
import { PaginatedChatsResponse } from '../../../features/chats-page/models/PaginatedChatsResponse';
import { ConfirmEmailCommand } from '../../../features/email-confirm/models/ConfirmEmailCommand';
import { ValidateEmailConfirmationResponse } from '../../../features/email-confirm/models/ValidateEmailConfirmationResponse';
import { environment } from '../../../../environments/environment';

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

  getAllUsersExceptCurrent(): Observable<UserItem[]> {
    return this.httpClient.get<UserItem[]>(`${this.apiUrl}/users/except-current`);
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

  createChat(createChatCommand: CreateChatCommand): Observable<string> {
    return this.httpClient.post<string>(`${this.apiUrl}/chats`, createChatCommand);
  }

  getChatById(chatId: string): Observable<Chat> {
    return this.httpClient.get<Chat>(`${this.apiUrl}/chats/${chatId}`);
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
