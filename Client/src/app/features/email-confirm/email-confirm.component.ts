import { Component, CUSTOM_ELEMENTS_SCHEMA, inject } from '@angular/core';
import { BreakpointObserver } from '@angular/cdk/layout';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../shared/services/api/api.service';
import { CommonButtonComponent } from "../../shared/components/common-button/common-button.component";
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { StorageService } from '../../shared/services/storage/storage.service';

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

    this.route.queryParams.subscribe(params => {
      const userId = params['userId'];
      const token = params['token'];

      if (!userId || !token) {
        this.finishLoading();
        return;
      } else {
        const command = {
          userId: userId,
          token: token
        };

        if (this.storage.getUserEmailConfirmedFromLocalStorage(userId)) {
          this.isSuccess = false;
          this.message = 'Your email has already been verified.';
          this.finishLoading();
          return;
        }

        this.apiService.confirmEmail(command).subscribe({
          next: () => {
            this.isSuccess = true;
            this.message = 'Your email has been successfully verified.';
            this.storage.setUserEmailConfirmedToLocalStorage(userId);
            this.finishLoading();
          },
          error: (error) => {
            this.isSuccess = false;
            this.handleError(error, userId);
            this.finishLoading();
          },
        });
      }
    })
  }

  toMainPage() {
    this.router.navigateByUrl('/login');
  }

  private handleError(error: any, userId: string) {
    let errorCode = error.error.errors.code;

    if (errorCode == 'User.EmailAlreadyConfirmed') {
      this.message = 'Your email has already been verified. You can login now.';
      this.storage.setUserEmailConfirmedToLocalStorage(userId);
    } else {
      this.message = 'Something went wrong. Please try again later.';
    }
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

  private checkIsMobileM(): void {
    this.observer
      .observe(['(max-width: 375px)'])
      .subscribe(result => {
        this.isMobileM = result.matches;
      });
  }
}
