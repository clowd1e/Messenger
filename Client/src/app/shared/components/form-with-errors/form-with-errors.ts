import { Directive, OnDestroy, OnInit } from '@angular/core';
import { FormControlConfiguration } from '../../models/configurations/forms/form-control-configuration';
import { FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';

@Directive()
export abstract class FormWithErrors implements OnInit, OnDestroy {
  private subscriptions: Record<string, Subscription | undefined> = {};
  abstract formConfiguration: Record<string, FormControlConfiguration>;
  abstract form: FormGroup<any>;

  ngOnInit(): void {
    for (const key in this.formConfiguration) {
      const control = this.form.get(key);
      if (control) {
        this.subscriptions[key] = this.form.get(key)?.valueChanges.subscribe(() => {
          this.formConfiguration[key].controlInvalid.set(control.invalid || false);
          this.formConfiguration[key].controlTouchedOrDirty.set(control.touched || control.dirty || false);
          this.formConfiguration[key].controlErrors.set(control.errors || null);
        });
      }
    }

    this.onInit();
  }

  ngOnDestroy(): void {
    for (const key in this.subscriptions) {
      this.subscriptions[key]?.unsubscribe();
    }

    for (const key in this.formConfiguration) {
      this.formConfiguration[key].controlInvalid.set(false);
      this.formConfiguration[key].controlTouchedOrDirty.set(false);
      this.formConfiguration[key].controlErrors.set(null);
    }

    this.onDestroy();
  }

  protected onInit(): void {

  }

  protected onDestroy(): void {

  }
}
