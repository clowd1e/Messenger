import { Component, inject } from '@angular/core';
import { RegisterRequest } from './models/RegisterRequest';
import { Router } from '@angular/router';
import { ApiService } from '../../shared/services/api/api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthInputComponent } from '../../shared/components/auth/auth-input/auth-input.component';
import { AuthButtonComponent } from '../../shared/components/auth/auth-button/auth-button.component';
import { HttpErrorResponse } from '@angular/common/http';
import { HttpValidationError } from '../../shared/models/errors/HttpValidationError';
import { HttpError } from '../../shared/models/errors/HttpError';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [AuthInputComponent, AuthButtonComponent, FormsModule, CommonModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss'
})
export class SignupComponent {
  signupRequest: RegisterRequest = {
    username: '',
    email: '',
    password: '',
  };
  repeatPassword: string = '';

  errorMessage: string = '';
  usernameErrorMessage: string = '';
  emailErrorMessage: string = '';
  passwordErrorMessage: string = '';
  repeatPasswordErrorMessage: string = '';

  router = inject(Router);
  apiService = inject(ApiService);

  onSubmit() {
    this.resetErrorMessages();
    
    if (this.repeatPassword !== this.signupRequest.password) {
      this.repeatPasswordErrorMessage = 'Passwords do not match';
      return;
    }

    this.apiService.register(this.signupRequest).subscribe({
      next: () => {
        this.router.navigateByUrl('login');
      },
      error: (httpError: HttpErrorResponse) => {
        if (this.isHttpValidationError(httpError.error)) {
          let error: HttpValidationError = httpError.error;
          
          error.errors.forEach(error => {
            switch (error.code.toLowerCase()) {
              case 'username':
                this.usernameErrorMessage = 'Invalid username';
                break;
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
    this.usernameErrorMessage = '';
    this.emailErrorMessage = '';
    this.passwordErrorMessage = '';
    this.repeatPasswordErrorMessage = '';
  }
}
