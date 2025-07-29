import { CommonModule } from '@angular/common';
import { Component, inject, signal, WritableSignal } from '@angular/core';
import { FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthButtonComponent } from '../../shared/components/auth/auth-button/auth-button.component';
import { AuthInputComponent } from '../../shared/components/auth/auth-input/auth-input.component';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { RegisterRequest } from './models/register-request';
import { passwordPatternValidator } from './validators/password-pattern.validator';
import { repeatPasswordValidator } from './validators/repeat-password.validator';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { ToastrService } from 'ngx-toastr';
import { emailIcon, nameIcon, passwordIcon, usernameIcon } from './register-icons';
import { usernamePatternValidator } from './validators/username-pattern.validator';
import { FormWithErrors } from '../../shared/components/form-with-errors/form-with-errors';
import { FormControlConfiguration } from '../../shared/models/configurations/forms/form-control-configuration';
import { Subscription } from 'rxjs';
import { registerFormConfiguration } from './register-form-configuration';
import { AuthErrorBoxComponent } from "../../shared/components/auth/auth-error-box/auth-error-box.component";
import { ApiService } from '../../shared/services/api.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    RouterLink, AuthInputComponent,
    AuthButtonComponent, ReactiveFormsModule,
    CommonModule,
    AuthErrorBoxComponent
],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent extends FormWithErrors {
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
      usernamePatternValidator
    ] }),
    name: this.fb.control('', { validators: [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(30)
    ] }),
    email: this.fb.control('', { validators: [
      Validators.required,
      Validators.minLength(3),
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
  override formConfiguration: Record<string, FormControlConfiguration> = registerFormConfiguration;
  override form: FormGroup<any> = this.registerForm;

  formStatusSubscription?: Subscription;
  submitButtonDisabled: WritableSignal<boolean> = signal(true);

  router = inject(Router);
  apiService = inject(ApiService);
  errorHandler = inject(ErrorHandlerService);
  toastr = inject(ToastrService);

  override onInit(): void {
    this.formStatusSubscription = this.registerForm.statusChanges.subscribe(() => {
      this.submitButtonDisabled.set(this.registerForm.invalid);
    });
  }

  override onDestroy(): void {
    if (this.formStatusSubscription) {
      this.formStatusSubscription.unsubscribe();
    }
  }

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
}
