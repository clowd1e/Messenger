import { Component, EventEmitter, inject, input, model, Output, signal } from '@angular/core';
import { SelectGroupUserState } from '../models/select-group-user-state';
import { Router } from '@angular/router';
import { UuidHelperService } from '../../../../../../../shared/services/uuid-helper.service';
import { ApiService } from '../../../../../../../shared/services/api.service';
import { ErrorHandlerService } from '../../../../../../../shared/services/error-handler.service';
import { UserItem } from '../models/user-item';

@Component({
  selector: 'app-user-item',
  standalone: true,
  imports: [],
  templateUrl: './user-item.component.html',
  styleUrl: './user-item.component.scss'
})
export class UserItemComponent {
  user = model.required<UserItem>();
  isGroupState = input.required<boolean>();

  @Output() userItemSelected = new EventEmitter<SelectGroupUserState | null>();

  router = inject(Router);
  uuidHelper = inject(UuidHelperService);
  apiService = inject(ApiService);
  errorHandler = inject(ErrorHandlerService);

  onUserItemClicked($event: Event): void {
    let selectedText = window.getSelection()?.toString();
    if (selectedText && selectedText.length > 0) {
      return;
    }
    $event.stopPropagation();
    if (this.isGroupState()) {
      this.user().isSelected = !this.user().isSelected;
      let groupUserSelectedState: SelectGroupUserState = {
        id: this.user().id,
        isSelected: this.user().isSelected,
        isGroupState: true
      };

      this.userItemSelected.emit(groupUserSelectedState);
    } else {
      this.userItemSelected.emit(null);
      let userId = this.uuidHelper.toShortUuid(this.user().id);
      this.router.navigate(['chats', 'add', 'private', userId]);
    }
  }
}
