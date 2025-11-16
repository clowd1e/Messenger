import { Component, EventEmitter, input, Output } from '@angular/core';
import { SelectGroupUserState } from '../models/select-group-user-state';
import { UserItemComponent } from "../user-item/user-item.component";
import { UserItem } from '../models/user-item';

@Component({
  selector: 'app-selected-users-list',
  standalone: true,
  imports: [UserItemComponent],
  templateUrl: './selected-users-list.component.html',
  styleUrl: './selected-users-list.component.scss'
})
export class SelectedUsersListComponent {
  users = input.required<UserItem[]>();

  @Output() userItemSelected = new EventEmitter<SelectGroupUserState | null>();

  onUserItemSelected($event: SelectGroupUserState | null): void {
    this.userItemSelected.emit($event);
  }
}
