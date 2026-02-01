import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { CommonButtonComponent } from '../../shared/components/common-button/common-button.component';
import { BreakpointObserver } from '@angular/cdk/layout';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';

@Component({
  selector: 'app-forgot-password-success',
  standalone: true,
  imports: [CommonButtonComponent],
  templateUrl: './forgot-password-success.component.html',
  styleUrl: './forgot-password-success.component.scss'
})
export class ForgotPasswordSuccessComponent implements OnInit, OnDestroy {
  isMobileM: boolean = false;
  private breakpointObserverSub: Subscription | null = null;

  router = inject(Router);
  observer = inject(BreakpointObserver);

  ngOnInit(): void {
    this.checkIsMobileM();
  }

  ngOnDestroy(): void {
    this.breakpointObserverSub?.unsubscribe();
  }

  toMainPage() {
    this.router.navigateByUrl('/login');
  }

  private checkIsMobileM(): void {
    this.breakpointObserverSub = this.observer
      .observe(['(max-width: 375px)'])
      .subscribe(result => {
        this.isMobileM = result.matches;
      });
  }
}
