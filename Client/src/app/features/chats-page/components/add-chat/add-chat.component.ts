import { Component, inject } from '@angular/core';
import { ApiService } from '../../../../shared/services/api/api.service';
import { Router } from '@angular/router';
import { UserItem } from './models/UserItem';
import { InputComponent } from '../input/input.component';
import { UserListComponent } from './user-list/user-list.component';

@Component({
  selector: 'app-add-chat',
  standalone: true,
  imports: [InputComponent, UserListComponent],
  templateUrl: './add-chat.component.html',
  styleUrl: './add-chat.component.scss'
})
export class AddChatComponent {
  userList: UserItem[] = [];
  
  apiService = inject(ApiService);
  router = inject(Router);

  ngOnInit() {
    this.apiService.getAllUsersExceptCurrent().subscribe({
      next: (response: UserItem[]) => {
        this.userList = response;
      }
    });
  }

  closeAddChat() {
    this.router.navigateByUrl('/chats');
  }
}
