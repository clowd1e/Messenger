import { Component, inject, signal, WritableSignal } from '@angular/core';
import { AuthInputComponent } from '../../shared/components/auth/auth-input/auth-input.component';
import { AuthButtonComponent } from '../../shared/components/auth/auth-button/auth-button.component';
import { Router, RouterLink } from '@angular/router';
import { FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthErrorBoxComponent } from '../../shared/components/auth/auth-error-box/auth-error-box.component';
import { emailIcon } from './forgot-password-icons';
import { FormControlConfiguration } from '../../shared/models/configurations/forms/form-control-configuration';
import { forgotPasswordFormConfiguration } from './forgot-password-form-configuration';
import { FormWithErrors } from '../../shared/components/form-with-errors/form-with-errors';
import { ApiService } from '../../shared/services/api.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs/internal/Subscription';
import { RequestPasswordRecoveryRequest } from './models/request-password-recovery-request';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [AuthInputComponent, AuthButtonComponent, RouterLink, ReactiveFormsModule, CommonModule, AuthErrorBoxComponent],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent extends FormWithErrors {
  emailIcon = emailIcon;
  
  fb = inject(NonNullableFormBuilder);
  forgotPasswordForm = this.fb.group({
    email: this.fb.control('', { validators: [
      Validators.required,
      Validators.minLength(3),
      Validators.email,
      Validators.maxLength(50)
    ] }),
  });
  override formConfiguration: Record<string, FormControlConfiguration> = forgotPasswordFormConfiguration;
  override form: FormGroup<any> = this.forgotPasswordForm;
  
  formStatusSubscription?: Subscription;
  submitButtonDisabled: WritableSignal<boolean> = signal(true);

  apiService = inject(ApiService);
  router = inject(Router);
  errorHandler = inject(ErrorHandlerService);
  toastr = inject(ToastrService);
  
  override onInit(): void {
    this.formStatusSubscription = this.forgotPasswordForm.statusChanges.subscribe(() => {
      this.submitButtonDisabled.set(this.forgotPasswordForm.invalid);
    });
  }

  override onDestroy(): void {
    if (this.formStatusSubscription) {
      this.formStatusSubscription.unsubscribe();
    }
  }

  onSubmit() {
    if (this.forgotPasswordForm.invalid) {
      this.toastr.error('Invalid form.');
    }

    let requestPasswordRecoveryRequest: RequestPasswordRecoveryRequest = {
      email: this.forgotPasswordForm.value.email || ''
    }

    this.apiService.requestPasswordRecovery(requestPasswordRecoveryRequest).subscribe({
      next: () => {
        this.router.navigateByUrl('forgot-password/success');
      },
      error: (httpError: HttpErrorResponse) => {
        this.errorHandler.handleHttpError(httpError);
      }
    });
  }
}
