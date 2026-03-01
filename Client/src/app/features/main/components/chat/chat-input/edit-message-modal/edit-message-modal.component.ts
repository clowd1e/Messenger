import { Component, input, output, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-message-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './edit-message-modal.component.html',
  styleUrl: './edit-message-modal.component.scss'
})
export class EditMessageModalComponent {
  messageContent = input.required<string>();
  messageId = input.required<string>();
  isVisible = input.required<boolean>();

  cancel = output<void>();

  onCancel() {
    this.cancel.emit();
  }

  @HostListener('document:keydown.escape', ['$event'])
  onEscapeKey(event: KeyboardEvent) {
    if (this.isVisible()) {
      event.preventDefault();
      this.onCancel();
    }
  }
}
