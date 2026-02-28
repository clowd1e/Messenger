import { Component, input } from '@angular/core';
import { SvgConfiguration } from '../../../../../../../../shared/models/configurations/UI/svg-configuration';

@Component({
  selector: 'app-message-operations-modal-item',
  standalone: true,
  imports: [],
  templateUrl: './message-operations-modal-item.component.html',
  styleUrl: './message-operations-modal-item.component.scss'
})
export class MessageOperationsModalItemComponent {
  svgConfig = input.required<SvgConfiguration>();
  itemLabel = input.required<string>();
}
