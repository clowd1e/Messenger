import { Component, inject } from '@angular/core';
import { UserListComponent } from "../../user/user-list/user-list.component";
import { UserItem } from '../../../models/UserItem';
import { DefaultInputComponent } from "../../inputs/default-input/default-input.component";
import { Router } from '@angular/router';
import { ApiService } from '../../../services/api/api.service';

@Component({
  selector: 'app-add-chat',
  standalone: true,
  imports: [UserListComponent, DefaultInputComponent],
  templateUrl: './add-chat.component.html',
  styleUrl: './add-chat.component.scss'
})
export class AddChatComponent {
  userList: Array<UserItem> = [];
  
  apiService = inject(ApiService);
  router = inject(Router);

  ngOnInit() {
    this.apiService.getAllUsers().subscribe({
      next: (response: Array<UserItem>) => {
        this.userList = response;
      }
    });
  }

  closeAddChat() {
    this.router.navigateByUrl('/chats');
  }
}
