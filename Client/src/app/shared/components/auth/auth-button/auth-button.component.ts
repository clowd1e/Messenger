import { Component, input } from '@angular/core';

@Component({
  selector: 'app-auth-button',
  standalone: true,
  imports: [],
  templateUrl: './auth-button.component.html',
  styleUrl: './auth-button.component.scss'
})
export class AuthButtonComponent {
  buttonText = input.required<string>();
}
