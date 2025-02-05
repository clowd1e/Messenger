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

  name = () => this.truncateName(this.userItem().name);

  userIcon = () => this.userItem().iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png";

  private truncateName(name: string) {
    const maxLength = 30;
    return name.length > maxLength ? name.substring(0, maxLength - 3) + '...' : name;
  }
}
