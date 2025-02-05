import { Component, inject, input } from '@angular/core';
import { UserItem } from '../models/UserItem';
import { UuidHelperService } from '../../../../../shared/services/uuid-helper/uuid-helper.service';
import { UserItemComponent } from './user-item/user-item.component';

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
      name: userItem.name,
      iconUri: userItem.iconUri,
    }
  })
}
