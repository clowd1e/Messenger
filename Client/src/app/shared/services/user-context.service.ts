import { inject, Injectable } from '@angular/core';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class UserContextService {
  storageService = inject(StorageService);

  private accessToken = this.storageService.getAccessTokenFromLocalStorage();

  private decodedToken = JSON.parse(window.atob(this.accessToken.split('.')[1]));

  getCurrentUserId() {
    return this.decodedToken.sub || '';
  }
}
