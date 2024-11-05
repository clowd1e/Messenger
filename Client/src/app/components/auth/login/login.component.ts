import { Component } from '@angular/core';
import { InputComponent } from "../input/input.component";
import { ButtonComponent } from "../button/button.component";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [InputComponent, ButtonComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

}
