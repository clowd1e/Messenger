import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LoginRequest } from './models/LoginRequest';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ApiService } from '../../shared/services/api/api.service';
import { StorageService } from '../../shared/services/storage/storage.service';
import { LoginResponse } from './models/LoginResponse';
import { HttpValidationError } from '../../shared/models/errors/HttpValidationError';
import { HttpError } from '../../shared/models/errors/HttpError';
import { AuthInputComponent } from '../../shared/components/auth/auth-input/auth-input.component';
import { AuthButtonComponent } from '../../shared/components/auth/auth-button/auth-button.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [AuthInputComponent, AuthButtonComponent, RouterLink, FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginRequest: LoginRequest = {
    email: '',
    password: ''
  };

  errorMessage: string = '';
  emailErrorMessage: string = '';
  passwordErrorMessage: string = '';

  httpClient = inject(HttpClient);
  apiService = inject(ApiService);
  storageService = inject(StorageService);
  router = inject(Router);

  onSubmit() {
    this.apiService.login(this.loginRequest).subscribe({
      next: (response: LoginResponse) => {
        this.storageService.setAccessTokenToSessionStorage(response.accessToken);
        this.storageService.setRefreshTokenToLocalStorage(response.refreshToken);
        
        this.router.navigateByUrl('chats/#');
      },
      error: (httpError: HttpErrorResponse) => {
        this.resetErrorMessages();

        if (this.isHttpValidationError(httpError.error)) {
          let error: HttpValidationError = httpError.error;
          
          error.errors.forEach(error => {
            switch (error.code.toLowerCase()) {
              case 'email':
                this.emailErrorMessage = 'Invalid email';
                break;
              case 'password':
                this.passwordErrorMessage = 'Invalid password';
                break;
              default:
                this.errorMessage = error.description;
                break;
            }
          });
        } else {
          let error: HttpError = httpError.error;
          
          this.errorMessage = error.errors.description
        }

      }
    });
  }

  private isHttpValidationError(error: any): error is HttpValidationError {
    return error && Array.isArray(error.errors);
  }

  private resetErrorMessages() {
    this.errorMessage = '';
    this.emailErrorMessage = '';
    this.passwordErrorMessage = '';
  }
}
