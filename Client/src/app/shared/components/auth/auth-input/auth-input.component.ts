import { Component, forwardRef, input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-auth-input',
  standalone: true,
  imports: [],
  templateUrl: './auth-input.component.html',
  styleUrl: './auth-input.component.scss',
  providers: [
      {
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => AuthInputComponent),
        multi: true
      }
    ]
})
export class AuthInputComponent {
  inputType = input.required<string>();
  imageSource = input.required<string>();
  inputPlaceholder = input.required<string>();

  value: string = '';

  private onChange = (value: string) => {};
  onTouched = () => {};

  writeValue(value: string): void {
    this.value = value || '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void { 
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void { }

  onInputChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.value = target.value;
    this.onChange(this.value);
  }
}
