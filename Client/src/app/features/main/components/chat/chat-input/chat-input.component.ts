import { Component, computed, effect, EventEmitter, inject, input, Output, untracked } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputComponent } from '../../input/input.component';
import { EditMessageModalComponent } from './edit-message-modal/edit-message-modal.component';
import { ApiService } from '../../../../../shared/services/api.service';
import { ErrorHandlerService } from '../../../../../shared/services/error-handler.service';
import { MainStorageService } from '../../../services/main-storage.service';

@Component({
  selector: 'app-chat-input',
  standalone: true,
  imports: [FormsModule, InputComponent, EditMessageModalComponent],
  templateUrl: './chat-input.component.html',
  styleUrl: './chat-input.component.scss'
})
export class ChatInputComponent {
  message: string = '';
  editingMessageId = input<string | null>(null);
  editingMessageContent = input<string | null>(null);

  @Output() messageSubmitted = new EventEmitter<string>();
  @Output() editCancelled = new EventEmitter<void>();

  apiService = inject(ApiService);
  errorHandler = inject(ErrorHandlerService);
  mainStorage = inject(MainStorageService);

  isEditingMessageMode = computed(() => !!this.editingMessageId());

  editingEffect = effect(() => {
    const content = this.editingMessageContent();
    if (content) {
      untracked(() => {
        this.message = content;
      });
    }
  });

  onSubmit() {
    if (!this.message.trim()) {
      return;
    }

    if (this.isEditingMessageMode()) {
      this.onUpdateMessage();
    } else {
      this.onSendMessage();
    }
  }

  private onSendMessage() {
    this.messageSubmitted.emit(this.message);
    this.message = '';
  }

  private onUpdateMessage() {
    const messageId = this.editingMessageId();
    const selectedChat = this.mainStorage.SelectedChat();

    if (!messageId || !selectedChat) {
      return;
    }

    if (this.message === this.editingMessageContent()) {
      this.onCancelEdit();
      return;
    }

    const command = {
      messageId: messageId,
      chatId: selectedChat.id,
      newContent: this.message
    };

    this.apiService.updateMessage(command).subscribe({
      next: () => {
        this.message = '';
        this.onCancelEdit();
      },
      error: (error: any) => {
        this.errorHandler.handleHttpError(error);
      }
    });
  }

  onCancelEdit() {
    this.message = '';
    this.editCancelled.emit();
  }
}
