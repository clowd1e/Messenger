import { Component, effect, ElementRef, input, model, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss'
})
export class InputComponent {
  @ViewChild('inputElement') inputElement?: ElementRef;

  placeholder = input.required<string>();
  focus = input<boolean>(false);

  inputValue = model<string>('');

  focusEffect = effect(() => {
    if (this.focus()) {
      this.inputElement?.nativeElement.focus();
    }
  });
}
