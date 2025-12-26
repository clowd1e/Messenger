import { Component, ElementRef, HostListener, inject, input, model, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HamburgerComponent } from "./hamburger/hamburger.component";
import { AddChatButtonComponent } from "./add-chat-button/add-chat-button.component";
import { ModalComponent } from "../../../../../shared/components/modal/modal.component";
import { AddChatPageComponent } from "./add-chat-page/add-chat-page.component";
import { ChatListComponent } from './chat-list/chat-list.component';
import { MainStorageService } from '../../../services/main-storage.service';
import { SettingsMenuComponent } from "./settings-menu/settings-menu.component";

@Component({
  selector: 'app-chats-aside',
  standalone: true,
  imports: [CommonModule, ChatListComponent, HamburgerComponent, AddChatButtonComponent, ModalComponent, AddChatPageComponent, SettingsMenuComponent],
  templateUrl: './chats-aside.component.html',
  styleUrl: './chats-aside.component.scss'
})
export class ChatsAsideComponent {
  @ViewChild('settingsMenu', { static: false })
  settingsMenu!: SettingsMenuComponent;

  isSettingsMenuOpen = false;
  chatsLoading = input.required<boolean>();
  chatRetrievalCutoff = input.required<Date>();

  mainStorage = inject(MainStorageService);

  showModal = false;

  openModal(): void {
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  toggleSettingsMenu(event: MouseEvent): void {
    event.stopPropagation();
    this.isSettingsMenuOpen = !this.isSettingsMenuOpen;
  }

  @HostListener('document:click', ['$event'])
  onOutsideClick(event: MouseEvent): void {
    // Close settings menu if clicked outside
    if (!this.isSettingsMenuOpen) return;
    if (!this.settingsMenu.menuRoot.nativeElement.contains(event.target as Node)) {
      this.isSettingsMenuOpen = false;
    }
  }

  @HostListener('document:keydown.escape', [])
  onEscapePress(): void {
    if (this.isSettingsMenuOpen) {
      this.isSettingsMenuOpen = false;
    }
  }
}
