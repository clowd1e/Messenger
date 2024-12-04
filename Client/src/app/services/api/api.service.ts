import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LoginRequest } from '../../Models/auth/LoginRequest';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly apiUrl = environment.apiBaseUrl;

  httpClient = inject(HttpClient);

  login(LoginRequest: LoginRequest): any {
    return this.httpClient.post(`${this.apiUrl}/auth/login`, LoginRequest);
  }
}
