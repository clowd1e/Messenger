import { Component, EventEmitter, model, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat-input',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './chat-input.component.html',
  styleUrl: './chat-input.component.scss'
})
export class ChatInputComponent {
  message: string = '';

  @Output() messageSubmitted = new EventEmitter<string>();

  onSubmit() {
    if (this.message.trim()) {
      this.messageSubmitted.emit(this.message);
      this.message = '';
    }
  }
}
