import { Component, CUSTOM_ELEMENTS_SCHEMA, effect, EventEmitter, inject, input, Output } from '@angular/core';
import { UserItemComponent } from '../user-item/user-item.component';
import { SelectGroupUserState } from '../models/select-group-user-state';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { UserItem } from '../models/user-item';

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [UserItemComponent, NgxSpinnerModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './users-list.component.html',
  styleUrl: './users-list.component.scss'
})
export class UsersListComponent {
  users = input.required<UserItem[]>();
  isGroupState = input.required<boolean>();
  isLoading = input.required<boolean>();
  usersNotFound = input.required<boolean>();

  @Output() userItemSelected = new EventEmitter<SelectGroupUserState | null>();

  spinner = inject(NgxSpinnerService);
  
  showSpinnerEffect = effect(() => {
    if (this.isLoading()) {
      this.spinner.show();
    } else {
      this.spinner.hide();
    }
  });

  onUserItemSelected($event: SelectGroupUserState | null): void {
    this.userItemSelected.emit($event);
  }
}
