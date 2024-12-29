import { Component, EventEmitter, input, model, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-default-input',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './default-input.component.html',
  styleUrl: './default-input.component.scss'
})
export class DefaultInputComponent {
  placeholder = input.required<string>();

  inputValue = model<string>('');
}
