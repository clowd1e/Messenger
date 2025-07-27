import { Component, computed, input } from '@angular/core';
import { ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-auth-error-box',
  standalone: true,
  imports: [],
  templateUrl: './auth-error-box.component.html',
  styleUrl: './auth-error-box.component.scss'
})
export class AuthErrorBoxComponent {
  inputTouchedOrDirty = input<boolean>(false);
  errors = input<ValidationErrors | null>(null);
  errorMessagesConfig = input<Record<string, string>>({});

  errorKeys = computed(() => Object.keys(this.errors() || {}));
  firstError = computed(() => {
    return { key: this.errorKeys()[0], message: this.errorMessagesConfig()[this.errorKeys()[0]] } 
  });

  get success(): boolean {
    return !this.errors();
  }
}
