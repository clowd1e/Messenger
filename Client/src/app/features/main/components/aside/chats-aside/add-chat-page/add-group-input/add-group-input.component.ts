import { Component, forwardRef, input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-add-group-input',
  standalone: true,
  imports: [],
  templateUrl: './add-group-input.component.html',
  styleUrl: './add-group-input.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AddGroupInputComponent),
      multi: true
    }
  ]
})
export class AddGroupInputComponent {
  name = input.required<string>();
  inputType = input.required<string>();
  inputPlaceholder = input.required<string>();

  value: string | File | null = '';

  private onChange = (value: string | File | null) => {};
  onTouched = () => {};

  writeValue(value: string | File | null): void {
    this.value = value;
  }

  registerOnChange(fn: (value: string | File | null) => void): void {
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

  onFileChange($event: Event) {
    const target = $event.target as HTMLInputElement;
    const file = target.files?.[0] ?? null;

    this.value = file;
    this.onChange(this.value);
  }
}
