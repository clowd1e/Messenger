import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class FormHelperService {
  formControlInvalid(form: FormGroup, controlName: keyof typeof form.controls): boolean {
    return form.controls[controlName].invalid && 
          (form.controls[controlName].touched || form.controls[controlName].dirty);
  }

  formControlContainsError(form: FormGroup, controlName: keyof typeof form.controls, errorName: string): boolean {
    return form.controls[controlName].hasError(errorName);
  }
}
