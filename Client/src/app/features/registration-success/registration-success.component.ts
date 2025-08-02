import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { CommonButtonComponent } from "../../shared/components/common-button/common-button.component";
import { Router } from '@angular/router';
import { BreakpointObserver } from '@angular/cdk/layout';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-registration-success',
  standalone: true,
  imports: [CommonButtonComponent],
  templateUrl: './registration-success.component.html',
  styleUrl: './registration-success.component.scss'
})
export class RegistrationSuccessComponent implements OnInit, OnDestroy {
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
