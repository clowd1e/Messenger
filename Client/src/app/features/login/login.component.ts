import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ApiService } from '../../shared/services/api/api.service';
import { StorageService } from '../../shared/services/storage/storage.service';
import { LoginResponse } from './models/LoginResponse';
import { AuthInputComponent } from '../../shared/components/auth/auth-input/auth-input.component';
import { AuthButtonComponent } from '../../shared/components/auth/auth-button/auth-button.component';
import { ErrorHandlerService } from '../../shared/services/error-handler/error-handler.service';
import { ToastrService } from 'ngx-toastr';
import { LoginRequest } from './models/LoginRequest';
import { FormHelperService } from '../../shared/services/form-helper/form-helper.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [AuthInputComponent, AuthButtonComponent, RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  fb = inject(NonNullableFormBuilder);
  loginForm = this.fb.group({
    email: this.fb.control('', { validators: [
      Validators.required,
      Validators.email,
      Validators.maxLength(50)
    ] }),
    password: this.fb.control('', { validators: [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(30)
    ] }),
  });

  httpClient = inject(HttpClient);
  apiService = inject(ApiService);
  storageService = inject(StorageService);
  router = inject(Router);
  errorHandler = inject(ErrorHandlerService);
  toastr = inject(ToastrService);
  formHelper = inject(FormHelperService);

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
        this.storageService.setAccessTokenToSessionStorage(response.accessToken);
        this.storageService.setRefreshTokenToLocalStorage(response.refreshToken);
        
        this.router.navigateByUrl('chats/#');
      },
      error: (httpError: HttpErrorResponse) => {
        this.errorHandler.handleHttpError(httpError);
      }
    });
  }

  formControlInvalid(controlName: keyof typeof this.loginForm.controls): boolean {
    return this.formHelper.formControlInvalid(this.loginForm, controlName);
  }

  formControlContainsError(controlName: keyof typeof this.loginForm.controls, errorName: string): boolean {
    return this.formHelper.formControlContainsError(this.loginForm, controlName, errorName);
  }
}
