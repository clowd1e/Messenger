import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  setAccessTokenToSessionStorage(accessToken: string) {
    sessionStorage.setItem('accessToken', accessToken);
  }

  getAccessTokenFromSessionStorage() : string {
    return sessionStorage.getItem('accessToken') || '';
  }

  removeAccessTokenFromSessionStorage() {
    sessionStorage.removeItem('accessToken');
  }

  setRefreshTokenToLocalStorage(refreshToken: string) {
    localStorage.setItem('refreshToken', refreshToken);
  }

  getRefreshTokenFromLocalStorage() : string {
    return localStorage.getItem('refreshToken') || '';
  }

  removeRefreshTokenFromLocalStorage() {
    localStorage.removeItem('refreshToken');
  }

  setUserEmailConfirmedToLocalStorage(userId: string) {
    localStorage.setItem(`EmailConfirmed:${userId}`, JSON.stringify(true));
  }

  getUserEmailConfirmedFromLocalStorage(userId: string): boolean {
    const value = localStorage.getItem(`EmailConfirmed:${userId}`);
    return value ? JSON.parse(value) : false;
  }
}
