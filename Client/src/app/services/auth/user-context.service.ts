import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserContextService {
  private accessToken = localStorage.getItem('accessToken') || '';

  private decodedToken = JSON.parse(window.atob(this.accessToken.split('.')[1]));

  getCurrentUserId() {
    return this.decodedToken.sub || '';
  }
}
