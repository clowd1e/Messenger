import { NgTemplateOutlet } from '@angular/common';
import { Component, forwardRef, input, TemplateRef, ViewEncapsulation } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { SvgConfiguration } from '../../../models/configurations/UI/svg-configuration';

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
  svgConfig = input.required<SvgConfiguration>();
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
