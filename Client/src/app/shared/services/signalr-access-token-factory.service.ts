import { inject, Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { StorageService } from './storage.service';
import { Router } from '@angular/router';
import { RefreshTokenRequest } from '../models/requests/refresh-token-request';

@Injectable({
  providedIn: 'root'
})
export class SignalrAccessTokenFactoryService {
  apiService = inject(ApiService);
  storageService = inject(StorageService);
  router = inject(Router);

  async getAccessToken(): Promise<string> {
    const accessToken = this.storageService.getAccessTokenFromLocalStorage();
    if (accessToken && !this.isExpired(accessToken)) {
      return accessToken;
    }

    let refreshToken = this.storageService.getRefreshTokenFromLocalStorage();
    let request: RefreshTokenRequest = {
      accessToken: accessToken,
      refreshToken: refreshToken
    }

    this.apiService.refresh(request).subscribe({
      next: (response) => {
        this.storageService.setAccessTokenToLocalStorage(response.accessToken);
        this.storageService.setRefreshTokenToLocalStorage(response.refreshToken);
        return response.accessToken;
      },
      error: (err) => {
        this.storageService.removeAccessTokenFromLocalStorage();
        this.storageService.removeRefreshTokenFromLocalStorage();
        this.router.navigate(['/login']);
        return '';
      }
    });

    return Promise.resolve('');
  }

  private isExpired(token: string): boolean {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const exp = payload.exp * 1000;
    return Date.now() > exp;
  }
}
