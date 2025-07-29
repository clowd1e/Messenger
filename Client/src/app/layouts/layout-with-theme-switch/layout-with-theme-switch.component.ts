import { Component } from '@angular/core';
import { RouterModule } from "@angular/router";
import { ThemeSwitchComponent } from "./theme-switch/theme-switch.component";

@Component({
  selector: 'app-layout-with-theme-switch',
  standalone: true,
  imports: [RouterModule, ThemeSwitchComponent],
  templateUrl: './layout-with-theme-switch.component.html',
  styleUrl: './layout-with-theme-switch.component.scss'
})
export class LayoutWithThemeSwitchComponent {

}
