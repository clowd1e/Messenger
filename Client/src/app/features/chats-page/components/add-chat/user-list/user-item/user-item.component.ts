import { Component, input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserItem } from '../../models/UserItem';

@Component({
  selector: 'app-user-item',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './user-item.component.html',
  styleUrl: './user-item.component.scss'
})
export class UserItemComponent {
  userItem = input.required<UserItem>();

  username = () => this.truncateUsername(this.userItem().username);

  private truncateUsername(username: string) {
    const maxLength = 30;
    return username.length > maxLength ? username.substring(0, maxLength - 3) + '...' : username;
  }
}
