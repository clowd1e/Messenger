import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  setAccessTokenToLocalStorage(accessToken: string) {
    localStorage.setItem('accessToken', accessToken);
  }

  setSessionIdToLocalStorage(sessionId: string) {
    localStorage.setItem('sessionId', sessionId);
  }

  getSessionIdFromLocalStorage() : string | null {
    return localStorage.getItem('sessionId') || null;
  }

  removeSessionIdFromLocalStorage() {
    localStorage.removeItem('sessionId');
  }

  getAccessTokenFromLocalStorage() : string {
    return localStorage.getItem('accessToken') || '';
  }

  removeAccessTokenFromLocalStorage() {
    localStorage.removeItem('accessToken');
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

  setEmailConfirmErrorToLocalStorage(userId: string, error: string) {
    localStorage.setItem(`EmailConfirmError:${userId}`, error);
  }

  getEmailConfirmErrorFromLocalStorage(userId: string): string | null {
    return localStorage.getItem(`EmailConfirmError:${userId}`);
  }

  setThemePreference(theme: 'light' | 'dark'): void {
    localStorage.setItem('theme', theme);
  }

  getThemePreference(): 'light' | 'dark' {
    const theme = localStorage.getItem('theme');
    return theme === 'light' || theme === 'dark' ? (theme as 'light' | 'dark') : 'light';
  }
}
