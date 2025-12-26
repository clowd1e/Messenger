import { Component, ElementRef, HostListener, inject, ViewChild } from '@angular/core';
import { SettingsMenuItemComponent } from "./settings-menu-item/settings-menu-item.component";
import { moonIcon, settingsIcon, sunIcon } from './settings-menu-icons';
import { StorageService } from '../../../../../../shared/services/storage.service';

@Component({
  selector: 'app-settings-menu',
  standalone: true,
  imports: [SettingsMenuItemComponent],
  templateUrl: './settings-menu.component.html',
  styleUrl: './settings-menu.component.scss'
})
export class SettingsMenuComponent {
  @ViewChild('menuRoot', { static: false }) 
  menuRoot!: ElementRef<HTMLElement>;
  private isSwitchingTheme: boolean = false;
  isDarkTheme = false;

  sunIcon = sunIcon;
  moonIcon = moonIcon;
  settingsIcon = settingsIcon;
  
  storageService = inject(StorageService);

  ngOnInit(): void {
    let themePreference = this.storageService.getThemePreference();
    this.isDarkTheme = themePreference === 'dark';
    document.documentElement.classList.toggle('dark-theme', this.isDarkTheme);
  }
  
  toggleTheme(): void {
    if (this.isSwitchingTheme) return;
    this.isSwitchingTheme = true;
    const currentTheme = this.storageService.getThemePreference();
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
    this.isDarkTheme = newTheme === 'dark';
    this.storageService.setThemePreference(newTheme);
    document.documentElement.classList.toggle('dark-theme', this.isDarkTheme);
    setTimeout(() => {
      this.isSwitchingTheme = false;
    }, 300);
  }
}
