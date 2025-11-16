import { Component, EventEmitter, inject, Output, signal, WritableSignal } from '@angular/core';
import { SearchBarComponent } from "../../../../../../shared/components/search-bar/search-bar.component";
import { CommonButtonComponent } from "../../../../../../shared/components/common-button/common-button.component";
import { User } from '../../../../models/user';
import { UsersListComponent } from './users-list/users-list.component';
import { SelectGroupUserState } from './models/select-group-user-state';
import { AuthErrorBoxComponent } from "../../../../../../shared/components/auth/auth-error-box/auth-error-box.component";
import { AddGroupInputComponent } from "./add-group-input/add-group-input.component";
import { FormGroup, FormsModule, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormWithErrors } from '../../../../../../shared/components/form-with-errors/form-with-errors';
import { FormControlConfiguration } from '../../../../../../shared/models/configurations/forms/form-control-configuration';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { addGroupChatFormConfiguration } from './add-group-chat-form-configuration';
import { maxFileSizeValidator } from './validators/max-file-size.validator';
import { fileDimensionsValidator } from './validators/file-dimensions.validator';
import { ApiService } from '../../../../../../shared/services/api.service';
import { ErrorHandlerService } from '../../../../../../shared/services/error-handler.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SelectedUsersListComponent } from "./selected-users-list/selected-users-list.component";
import { UserItem } from './models/user-item';
import { MapUserToUserItem } from './mappers/user-to-user-item.mapper';
import { CreateGroupChat } from './models/create-group-chat';
import { GroupCreationStore } from './services/group-creation-store.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-chat-page',
  host: {
    '[class.group-state]': 'isGroupState()',
    '(click)': 'onClosed()'
  },
  standalone: true,
  imports: [
    SearchBarComponent,
    CommonButtonComponent,
    UsersListComponent,
    AuthErrorBoxComponent,
    AddGroupInputComponent,
    ReactiveFormsModule,
    CommonModule,
    FormsModule,
    SelectedUsersListComponent
  ],
  templateUrl: './add-chat-page.component.html',
  styleUrl: './add-chat-page.component.scss'
})
export class AddChatPageComponent extends FormWithErrors {
  private debounceTimer: any;

  fb = inject(NonNullableFormBuilder);
  groupForm = this.fb.group({
    groupName: this.fb.control('', { validators: [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(50)
    ] }),
    groupDescription: this.fb.control<string | null>(null, { validators: [
      Validators.minLength(1),
      Validators.maxLength(200),
    ] }),
    groupIcon: this.fb.control<File | null>(null, {
      validators: [
        maxFileSizeValidator
      ],
      asyncValidators: [fileDimensionsValidator]
    })
  });
  override formConfiguration: Record<string, FormControlConfiguration> = addGroupChatFormConfiguration;
  override form: FormGroup<any> = this.groupForm;

  formStatusSubscription?: Subscription;
  submitButtonDisabled: WritableSignal<boolean> = signal(true);

  @Output() closed = new EventEmitter<void>();
  isLoading: WritableSignal<boolean> = signal<boolean>(false);
  usersNotFound: WritableSignal<boolean> = signal<boolean>(false);
  isGroupState = signal<boolean>(false);
  selectedGroupUsers = signal<UserItem[]>([]);
  users = signal<UserItem[]>([]);
  searchQuery: WritableSignal<string> = signal<string>('');

  groupCreationStore = inject(GroupCreationStore);
  router = inject(Router);
  apiService = inject(ApiService);
  errorHandler = inject(ErrorHandlerService);

  override onInit(): void {
    this.formStatusSubscription = this.groupForm.statusChanges.subscribe(() => {
      this.submitButtonDisabled.set(this.groupForm.invalid);
    });    
  }

  override onDestroy(): void {
    if (this.formStatusSubscription) {
      this.formStatusSubscription.unsubscribe();
    }
  }

  onSubmit() {
    if (this.selectedGroupUsers().length < 2) {
      this.groupForm.setErrors({ userSelectedCount: 'At least 2 users must be selected to create a group chat.' });
      return;
    }
    if (this.selectedGroupUsers().length > 19) {
      this.groupForm.setErrors({ userSelectedCount: 'A maximum of 19 users can be selected to create a group chat.' });
      return;
    }

    let groupChat: CreateGroupChat = {
      name: this.groupForm.controls['groupName'].value,
      description: this.groupForm.controls['groupDescription'].value,
      icon: this.groupForm.controls['groupIcon'].value,
      participantIds: this.selectedGroupUsers().map(user => user.id)
    }

    this.groupCreationStore.setGroupChat(groupChat);
    this.router.navigate(['chats', 'add', 'group']);
  }

  onClosed(): void {
    this.isLoading.set(false);
    this.closed.emit();
  }

  onCreateGroupChat(): void {
    this.isGroupState.set(!this.isGroupState());
  }

  // Handle user item selection for group chat
  onUserItemSelected($event: SelectGroupUserState | null): void {
    this.clearUserSelectedCountError();

    if (this.isGroupState()) {
      if (!$event) return;
      let selectedUser = this.users().find(user => user.id === $event.id);
      // If user is already selected and isSelected is false, remove from selectedGroupUsers
      if (selectedUser && !$event.isSelected) {
        this.selectedGroupUsers.update(users => users.filter(user => user.id !== $event.id));
        return;
      }
      // If user is not selected and isSelected is true, add to selectedGroupUsers
      this.selectedGroupUsers.update(users => [...users, selectedUser!]);
    } else {
      this.closed.emit();
    }
  }

  onSelectedUserItemSelected($event: SelectGroupUserState | null): void {
    this.clearUserSelectedCountError();

    if (this.isGroupState()) {
      if (!$event) return;
      this.selectedGroupUsers.update(users => users.filter(user => user.id !== $event.id));
    } else {
      this.closed.emit();
    }
  }

  onSearchQueryChange($event: any) {
    this.isLoading.set(true);
    clearTimeout(this.debounceTimer);

    if (!$event || $event.trim() === '') {
      this.users.set([]);
      this.usersNotFound.set(false);
      this.isLoading.set(false);
      return;
    }
    
    this.debounceTimer = setTimeout(() => {
      this.performSearch($event);
    }, 1000);
  }

  private performSearch(query: string): void {
    this.apiService.searchUsers(query).subscribe({
      next: (response: User[]) => {
        let userItems: UserItem[] = response.map(user => MapUserToUserItem(user, false));

        if (this.isGroupState() && this.selectedGroupUsers().length > 0 && userItems.length > 0) {
          // Mark users as selected if they are in selectedGroupUsers
          userItems.forEach(element => {
            if (this.selectedGroupUsers().some(user => user.id === element.id)) {
              element.isSelected = true;
            }
          });
        }

        this.users.set(userItems);
        if (response.length === 0) {
          this.usersNotFound.set(true);
        } else {
          this.usersNotFound.set(false);
        }
      },
      error: (error: HttpErrorResponse) => {
        this.errorHandler.handleHttpError(error);
      }
    });

    this.isLoading.set(false);
  }

  private clearUserSelectedCountError() {
    const errors = { ...this.groupForm.errors };

    if (errors['userSelectedCount']) {
      delete errors['userSelectedCount'];
    }
    
    // If no errors remain, pass null, otherwise pass the updated error object
    this.groupForm.setErrors(Object.keys(errors).length ? errors : null);
  }
}
