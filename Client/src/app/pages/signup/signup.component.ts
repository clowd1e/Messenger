import { Component, inject } from '@angular/core';
import { InputComponent } from "../../components/auth/input/input.component";
import { RegisterRequest } from '../../models/auth/RegisterRequest';
import { ButtonComponent } from "../../components/auth/button/button.component";
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api/api.service';
import { Router } from '@angular/router';
import { HttpValidationError } from '../../models/error/HttpValidationError';
import { HttpError } from '../../models/error/HttpError';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [InputComponent, ButtonComponent, FormsModule, CommonModule],
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
