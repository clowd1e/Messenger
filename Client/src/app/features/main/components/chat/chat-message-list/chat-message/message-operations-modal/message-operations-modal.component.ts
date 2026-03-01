import { Component, HostListener, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageOperationsModalItemComponent } from './message-operations-modal-item/message-operations-modal-item.component';
import { editIcon, trashIcon } from './icons/icons';

@Component({
  selector: 'app-message-operations-modal',
  standalone: true,
  imports: [CommonModule, MessageOperationsModalItemComponent],
  templateUrl: './message-operations-modal.component.html',
  styleUrl: './message-operations-modal.component.scss'
})
export class MessageOperationsModalComponent {
  messageId = input.required<string>();
  isVisible = input.required<boolean>();
  isSendersMessage = input.required<boolean>();
  position = input<{ x: number, y: number } | null>(null);

  deleteForMe = output<string>();
  deleteForEveryone = output<string>();
  edit = output<string>();
  close = output<void>();

  trashIcon = trashIcon;
  editIcon = editIcon;

  onEdit() {
    this.edit.emit(this.messageId());
    this.close.emit();
  }

  onDeleteForMe() {
    this.deleteForMe.emit(this.messageId());
    this.close.emit();
  }

  onDeleteForEveryone() {
    this.deleteForEveryone.emit(this.messageId());
    this.close.emit();
  }

  onBackdropClick(event: MouseEvent) {
    event.stopPropagation();
    this.close.emit();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (!this.isVisible()) return;
    this.close.emit();
  }

  @HostListener('document:contextmenu', ['$event'])
  onDocumentContextMenu(event: MouseEvent) {
    if (!this.isVisible()) return;
    
    const target = event.target as HTMLElement;
    if (target.closest('.modal-menu')) return;

    this.close.emit();
  }

}
