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

  setTokenExpirationToLocalStorage(tokenId: string, expiration: Date) {
    localStorage.setItem(`TokenExpiration:${tokenId}`, expiration.toString());
  }

  getTokenExpirationFromLocalStorage(tokenId: string): Date | null {
    const value = localStorage.getItem(`TokenExpiration:${tokenId}`);
    
    return value ? new Date(value) : null;
  }

  setUserEmailConfirmedToLocalStorage(userId: string) {
    localStorage.setItem(`EmailConfirmed:${userId}`, JSON.stringify(true));
  }

  getUserEmailConfirmedFromLocalStorage(userId: string): boolean {
    const value = localStorage.getItem(`EmailConfirmed:${userId}`);
    return value ? JSON.parse(value) : false;
  }
}
