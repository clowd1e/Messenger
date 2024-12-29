import { Component, input } from '@angular/core';
import { UserItemComponent } from "../user-item/user-item.component";
import { UserItem } from '../../../models/UserItem';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [UserItemComponent],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent {
  userList = input<UserItem[]>();
}
