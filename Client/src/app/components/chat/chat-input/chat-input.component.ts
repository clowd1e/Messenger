import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DefaultInputComponent } from "../../inputs/default-input/default-input.component";

@Component({
  selector: 'app-chat-input',
  standalone: true,
  imports: [FormsModule, DefaultInputComponent],
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