import { Component, input } from '@angular/core';
import { SvgConfiguration } from '../../../../../../../shared/models/configurations/UI/svg-configuration';

@Component({
  selector: 'app-settings-menu-item',
  standalone: true,
  imports: [],
  templateUrl: './settings-menu-item.component.html',
  styleUrl: './settings-menu-item.component.scss'
})
export class SettingsMenuItemComponent {
  svgConfig = input.required<SvgConfiguration>();
  itemLabel = input.required<string>();
}
