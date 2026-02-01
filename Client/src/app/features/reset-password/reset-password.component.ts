import { Component, CUSTOM_ELEMENTS_SCHEMA, inject, signal, WritableSignal } from '@angular/core';
import { AuthInputComponent } from '../../shared/components/auth/auth-input/auth-input.component';
import { AuthButtonComponent } from '../../shared/components/auth/auth-button/auth-button.component';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthErrorBoxComponent } from '../../shared/components/auth/auth-error-box/auth-error-box.component';
import { FormWithErrors } from '../../shared/components/form-with-errors/form-with-errors';
import { passwordIcon } from './reset-password-icons';
import { passwordPatternValidator } from '../../shared/validators/password-pattern.validator';
import { repeatPasswordValidator } from '../../shared/validators/repeat-password.validator';
import { FormControlConfiguration } from '../../shared/models/configurations/forms/form-control-configuration';
import { resetPasswordFormConfiguration } from './reset-password-form-configuration';
import { Subscription } from 'rxjs/internal/Subscription';
import { ApiService } from '../../shared/services/api.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/internal/operators/take';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { firstValueFrom } from 'rxjs/internal/firstValueFrom';
import { ResetPasswordRequest } from './models/reset-password-request';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [AuthInputComponent, AuthButtonComponent, RouterLink, ReactiveFormsModule, CommonModule, AuthErrorBoxComponent, NgxSpinnerModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent extends FormWithErrors {
  passwordIcon = passwordIcon;
    
  fb = inject(NonNullableFormBuilder);
  resetPasswordForm = this.fb.group({
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
  override formConfiguration: Record<string, FormControlConfiguration> = resetPasswordFormConfiguration;
  override form: FormGroup<any> = this.resetPasswordForm;
  
  formStatusSubscription?: Subscription;
  submitButtonDisabled: WritableSignal<boolean> = signal(true);

  private userId: string = '';
  private tokenId: string = '';
  private token: string = '';
  isLoading: boolean = false;

  apiService = inject(ApiService);
  route = inject(ActivatedRoute);
  router = inject(Router);
  errorHandler = inject(ErrorHandlerService);
  spinner = inject(NgxSpinnerService);
  toastr = inject(ToastrService);
  
  override onInit(): void {
    this.formStatusSubscription = this.resetPasswordForm.statusChanges.subscribe(() => {
      this.submitButtonDisabled.set(this.resetPasswordForm.invalid);
    });

    this.startLoading();

    this.route.queryParams
      // take only the first emission, as we only need to read the parameters once
      .pipe(take(1))
      .subscribe(async params => {
        const userId = params['userId'];
        const tokenId = params['tokenId'];
        const token = params['token'];

        if (!userId || !tokenId || !token) {
          this.toastr.error('Invalid password reset link.');
          this.finishLoading();
          this.router.navigate(['/']);
          return;
        }

        let validationResult = await this.validatePasswordRecovery(userId, tokenId);
        console.log(validationResult);
        if (!validationResult) {
          this.finishLoading();
          this.router.navigate(['/']);
          return;
        }

        this.userId = userId;
        this.tokenId = tokenId;
        this.token = token;

        this.finishLoading();
      });
  }

  override onDestroy(): void {
    if (this.formStatusSubscription) {
      this.formStatusSubscription.unsubscribe();
    }
  }

  onSubmit() {
    if (this.resetPasswordForm.invalid) {
        this.toastr.error('Invalid form.');
        return;
      }
  
      let resetPasswordRequest: ResetPasswordRequest = {
        userId: this.userId,
        tokenId: this.tokenId,
        token: this.token,
        newPassword: this.resetPasswordForm.value.password || ''
      }
  
      this.apiService.resetPassword(resetPasswordRequest).subscribe({
        next: () => {
          this.toastr.success('Password has been reset successfully.');
          this.router.navigate(['/login']);
        },
        error: (httpError: HttpErrorResponse) => {
          this.errorHandler.handleHttpError(httpError);
        }
      });
  }

  private async validatePasswordRecovery(
    userId: string,
    tokenId: string
  ) : Promise<boolean> {
    try {
      await firstValueFrom(this.apiService.validatePasswordRecovery(userId, tokenId));
      return true;
    } catch (error: any) {
      this.errorHandler.handleError(error.error);
      return false;
    };
  }

  private startLoading() {
    this.isLoading = true;
    this.spinner.show();
  }

  private finishLoading() {
    setTimeout(() => {
      this.spinner.hide();
      this.isLoading = false;
    }, 1000);
  }
}
