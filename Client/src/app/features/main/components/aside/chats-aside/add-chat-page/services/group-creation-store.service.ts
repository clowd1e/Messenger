import { Injectable } from '@angular/core';
import { CreateGroupChat } from '../models/create-group-chat';

@Injectable({
  providedIn: 'root'
})
export class GroupCreationStore {
  private groupChatData: CreateGroupChat | null = null;

  setGroupChat(data: CreateGroupChat): void {
    this.groupChatData = data;
  }

  getGroupChat(): CreateGroupChat | null {
    return this.groupChatData;
  }
}
