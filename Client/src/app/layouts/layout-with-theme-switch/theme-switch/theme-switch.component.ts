import { Component, HostListener, inject, OnInit } from '@angular/core';
import { StorageService } from '../../../shared/services/storage.service';

@Component({
  selector: 'app-theme-switch',
  standalone: true,
  imports: [],
  templateUrl: './theme-switch.component.html',
  styleUrl: './theme-switch.component.scss'
})
export class ThemeSwitchComponent implements OnInit {
  private isSwitchingTheme: boolean = false;
  isDarkTheme: boolean = false;
  hideSwitch: boolean = false;

  storageService = inject(StorageService);

  ngOnInit(): void {
    let themePreference = this.storageService.getThemePreference();
    this.isDarkTheme = themePreference === 'dark';
    document.documentElement.classList.toggle('dark-theme', this.isDarkTheme);
  }

  @HostListener('window:scroll', [])
  onScroll(): void {
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    this.hideSwitch = scrollTop > 1;
  }

  toggleTheme(): void {
    if (this.isSwitchingTheme) return;

    this.isSwitchingTheme = true;
    this.isDarkTheme = !this.isDarkTheme;
    document.documentElement.classList.toggle('dark-theme', this.isDarkTheme);
    this.storageService.setThemePreference(this.isDarkTheme ? 'dark' : 'light');

    setTimeout(() => {
      this.isSwitchingTheme = false;
    }, 300);
  }
}
