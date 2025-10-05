import { CommonModule } from '@angular/common';
import { Component, inject, signal, WritableSignal } from '@angular/core';
import { FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { StorageService } from '../../shared/services/storage.service';
import { LoginResponse } from './models/login-response';
import { AuthInputComponent } from '../../shared/components/auth/auth-input/auth-input.component';
import { AuthButtonComponent } from '../../shared/components/auth/auth-button/auth-button.component';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { ToastrService } from 'ngx-toastr';
import { LoginRequest } from './models/login-request';
import { emailIcon, passwordIcon } from './login-icons';
import { AuthErrorBoxComponent } from "../../shared/components/auth/auth-error-box/auth-error-box.component";
import { Subscription } from 'rxjs';
import { FormWithErrors } from '../../shared/components/form-with-errors/form-with-errors';
import { FormControlConfiguration } from '../../shared/models/configurations/forms/form-control-configuration';
import { loginFormConfiguration } from './login-form-configuration';
import { ApiService } from '../../shared/services/api.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [AuthInputComponent, AuthButtonComponent, RouterLink, ReactiveFormsModule, CommonModule, AuthErrorBoxComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent extends FormWithErrors {
  emailIcon = emailIcon;
  passwordIcon = passwordIcon;

  fb = inject(NonNullableFormBuilder);
  loginForm = this.fb.group({
    email: this.fb.control('', { validators: [
      Validators.required,
      Validators.minLength(3),
      Validators.email,
      Validators.maxLength(50)
    ] }),
    password: this.fb.control('', { validators: [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(30)
    ] }),
  });
  override formConfiguration: Record<string, FormControlConfiguration> = loginFormConfiguration;
  override form: FormGroup<any> = this.loginForm;

  formStatusSubscription?: Subscription;
  submitButtonDisabled: WritableSignal<boolean> = signal(true);

  httpClient = inject(HttpClient);
  apiService = inject(ApiService);
  storageService = inject(StorageService);
  router = inject(Router);
  errorHandler = inject(ErrorHandlerService);
  toastr = inject(ToastrService);

  override onInit(): void {
    this.formStatusSubscription = this.loginForm.statusChanges.subscribe(() => {
      this.submitButtonDisabled.set(this.loginForm.invalid);
    });
  }

  override onDestroy(): void {
    if (this.formStatusSubscription) {
      this.formStatusSubscription.unsubscribe();
    }
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      this.toastr.error('Invalid form.');
    }

    let loginRequest: LoginRequest = {
      email: this.loginForm.value.email || '',
      password: this.loginForm.value.password || ''
    }

    this.apiService.login(loginRequest).subscribe({
      next: (response: LoginResponse) => {
        this.storageService.setAccessTokenToLocalStorage(response.accessToken);
        this.storageService.setRefreshTokenToLocalStorage(response.refreshToken);
        
        this.router.navigateByUrl('chats/');
      },
      error: (httpError: HttpErrorResponse) => {
        this.errorHandler.handleHttpError(httpError);
      }
    });
  }
}
