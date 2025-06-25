import { Component, CUSTOM_ELEMENTS_SCHEMA, inject } from '@angular/core';
import { BreakpointObserver } from '@angular/cdk/layout';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../shared/services/api/api.service';
import { CommonButtonComponent } from "../../shared/components/common-button/common-button.component";
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { StorageService } from '../../shared/services/storage/storage.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-email-confirm',
  standalone: true,
  imports: [CommonButtonComponent, NgxSpinnerModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './email-confirm.component.html',
  styleUrl: './email-confirm.component.scss'
})
export class EmailConfirmComponent {
  isLoading: boolean = false;
  isSuccess: boolean = false;
  isMobileM: boolean = false;
  message: string = '';

  observer = inject(BreakpointObserver);
  route = inject(ActivatedRoute);
  router = inject(Router);
  storage = inject(StorageService);
  spinner = inject(NgxSpinnerService);
  apiService = inject(ApiService);

  ngOnInit() {
    this.checkIsMobileM();

    this.startLoading();

    this.route.queryParams.subscribe(async params => {
      const userId = params['userId'];
      const tokenId = params['tokenId'];
      const token = params['token'];

      if (!userId || !tokenId || !token) {
        this.finishLoading();
        return;
      } else {
        if (this.storage.getUserEmailConfirmedFromLocalStorage(userId)) {
          this.raiseError('Your email has already been verified.');
          this.finishLoading();
          return;
        }

        let emailConfirmErrorFromLocalStorage = this.storage.getEmailConfirmErrorFromLocalStorage(userId);
        if (emailConfirmErrorFromLocalStorage) {
          this.raiseError(emailConfirmErrorFromLocalStorage);
          this.finishLoading();
          return;
        }

        let validationResult = await this.validateEmailConfirmation(userId, tokenId);
        if (!validationResult) {
          this.finishLoading();
          return;
        }

        const command = {
          userId: userId,
          tokenId: tokenId,
          token: token
        };

        this.apiService.confirmEmail(command).subscribe({
          next: () => {
            this.raiseSuccess('Your email has been successfully verified.');
            this.storage.setUserEmailConfirmedToLocalStorage(userId);
            this.finishLoading();
          },
          error: (error) => {
            this.raiseError();
            this.handleConfirmEmailError(error, userId);
            this.finishLoading();
          },
        });
      }
    })
  }

  toMainPage() {
    this.router.navigateByUrl('/login');
  }

  private handleConfirmEmailError(error: any, userId: string) {
    let errorCode = error.error.errors.code;

    if (errorCode == 'User.EmailAlreadyConfirmed') {
      this.message = 'Your email has already been verified. You can login now.';
      this.storage.setUserEmailConfirmedToLocalStorage(userId);
    }
    else {
      this.message = 'Something went wrong. Please try again later.';
    }
  }

  private async validateEmailConfirmation(userId: string, tokenId: string): Promise<boolean> {
    let expiresAt = this.storage.getTokenExpirationFromLocalStorage(tokenId);
    if (expiresAt && expiresAt < new Date()) {
      this.raiseError('The confirmation link has expired.');
      return false;
    }

    try {
      const response = await firstValueFrom(this.apiService.validateEmailConfirmation(userId, tokenId));
      const expiresAt = new Date(response.expiresAt);
      this.storage.setTokenExpirationToLocalStorage(tokenId, expiresAt);
      if (expiresAt < new Date()) {
        this.raiseError('The confirmation link has expired.');
        return false;
      }
      return true;
    } catch (error) {
        this.handleValidateEmailConfirmationError(userId, error);
        return false;
    };
  }

  private handleValidateEmailConfirmationError(userId: string, error: any): void {
    let errorCode = error.error.errors.code;

    if (errorCode == 'ConfirmEmailToken.AlreadyUsed') {
      this.message = 'You have already used this confirmation link.';
      this.storage.setEmailConfirmErrorToLocalStorage(userId, this.message);
    }
    else if (errorCode == 'ConfirmEmailToken.Expired') {
      this.message = 'The confirmation link has expired.';
      this.storage.setEmailConfirmErrorToLocalStorage(userId, this.message);
    } 
    else {
      this.message = 'Something went wrong. Please try again later.';
    }
  }

  // View methods 

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

  private checkIsMobileM(): void {
    this.observer
      .observe(['(max-width: 375px)'])
      .subscribe(result => {
        this.isMobileM = result.matches;
      });
  }

  // Logic methods

  private raiseError(message: string = ''): void {
    this.isSuccess = false;
    this.message = message;
  }

  private raiseSuccess(message: string): void {
    this.isSuccess = true;
    this.message = message;
  }
}

