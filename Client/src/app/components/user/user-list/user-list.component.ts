import { Component, inject, input } from '@angular/core';
import { UserItemComponent } from "../user-item/user-item.component";
import { UserItem } from '../../../models/UserItem';
import { UuidHelperService } from '../../../services/uuid-helper/uuid-helper.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [UserItemComponent],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent {
  userList = input<UserItem[]>();

  uuidHelper = inject(UuidHelperService);

  mappedUserList = () => this.userList()?.map(userItem => {
    return {
      id: this.uuidHelper.toShortUuid(userItem.id),
      username: userItem.username,
      iconUri: userItem.iconUri,
    }
  })
}
