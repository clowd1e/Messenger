import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LoginRequest } from '../../Models/auth/LoginRequest';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'http://localhost:5000/api';

  httpClient = inject(HttpClient);

  constructor() { }

  login(LoginRequest: LoginRequest): any {
    return this.httpClient.post(`${this.apiUrl}/auth/login`, LoginRequest);
  }
}
