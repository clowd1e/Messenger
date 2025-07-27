import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthButtonComponent } from '../../shared/components/auth/auth-button/auth-button.component';
import { AuthInputComponent } from '../../shared/components/auth/auth-input/auth-input.component';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { ApiService } from '../../shared/services/api/api.service';
import { RegisterRequest } from './models/RegisterRequest';
import { passwordPatternValidator } from './validators/PasswordPatternValidator';
import { repeatPasswordValidator } from './validators/RepeatPasswordValidator';
import { ErrorHandlerService } from '../../shared/services/error-handler/error-handler.service';
import { ToastrService } from 'ngx-toastr';
import { FormHelperService } from '../../shared/services/form-helper/form-helper.service';
import { emailIcon, nameIcon, passwordIcon, usernameIcon } from './register-icons';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    RouterLink, AuthInputComponent,
    AuthButtonComponent, ReactiveFormsModule,
    CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  usernameIcon = usernameIcon;
  nameIcon = nameIcon;
  emailIcon = emailIcon;
  passwordIcon = passwordIcon;

  fb = inject(NonNullableFormBuilder);
  registerForm = this.fb.group({
    username: this.fb.control('', { validators: [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(30),
      Validators.pattern('^[a-zA-Z0-9]+$')
    ] }),
    name: this.fb.control('', { validators: [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(30)
    ] }),
    email: this.fb.control('', { validators: [
      Validators.required,
      Validators.email,
      Validators.maxLength(50)
    ] }),
    password: this.fb.control('', { validators: [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(30),
      passwordPatternValidator
    ] }),
    repeatPassword: this.fb.control('', { validators: [
      Validators.required,
      repeatPasswordValidator
    ] })
  });

  router = inject(Router);
  apiService = inject(ApiService);
  errorHandler = inject(ErrorHandlerService);
  toastr = inject(ToastrService);
  formHelper = inject(FormHelperService);

  onSubmit() {
    if (this.registerForm.invalid) {
      this.toastr.error('Invalid form.');
    }

    let registerRequest: RegisterRequest = {
      username: this.registerForm.value.username || '',
      name: this.registerForm.value.name || '',
      email: this.registerForm.value.email || '',
      password: this.registerForm.value.password || '',
    };

    this.apiService.register(registerRequest).subscribe({
      next: () => {
        this.router.navigateByUrl('login');
        this.toastr.success('Registration successful! Please confirm your email.');
      },
      error: (httpError: HttpErrorResponse) => {
        this.errorHandler.handleHttpError(httpError);
      }
    });
  }

  formControlInvalid(controlName: keyof typeof this.registerForm.controls): boolean {
    return this.formHelper.formControlInvalid(this.registerForm, controlName);
  }

  formControlContainsError(controlName: keyof typeof this.registerForm.controls, errorName: string): boolean {
    return this.formHelper.formControlContainsError(this.registerForm, controlName, errorName);
  }
}
