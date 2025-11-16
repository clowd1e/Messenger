import { Component, forwardRef, input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-search-bar',
  standalone: true,
  imports: [],
  templateUrl: './search-bar.component.html',
  styleUrl: './search-bar.component.scss',
  providers: [
        {
          provide: NG_VALUE_ACCESSOR,
          useExisting: forwardRef(() => SearchBarComponent),
          multi: true
        }
      ]
})
export class SearchBarComponent {
  placeholder = input.required<string>();

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
