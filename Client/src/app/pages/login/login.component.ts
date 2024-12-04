import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputComponent } from "../../components/auth/input/input.component";
import { ButtonComponent } from "../../components/auth/button/button.component";
import { LoginRequest } from '../../Models/auth/LoginRequest';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { LoginResponse } from '../../Models/auth/LoginResponse';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api/api.service';
import { HttpError } from '../../Models/error/HttpError';
import { HttpValidationError } from '../../Models/error/HttpValidationError';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [InputComponent, ButtonComponent, FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginRequest: LoginRequest = {
    email: 'Abanent@gmail.com',
    password: 'Abanent123!.'
  };

  errorMessage: string = '';
  emailErrorMessage: string = '';
  passwordErrorMessage: string = '';

  httpClient = inject(HttpClient);
  apiService = inject(ApiService);
  router = inject(Router);

  onSubmit() {
    this.apiService.login(this.loginRequest).subscribe({
      next: (response: LoginResponse) => {
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
        
        this.router.navigateByUrl('chats/#');
      },
      error: (httpError: HttpErrorResponse) => {
        this.resetErrorMessage();

        console.log(httpError);

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

  private resetErrorMessage() {
    this.errorMessage = '';
    this.emailErrorMessage = '';
    this.passwordErrorMessage = '';
  }
}
