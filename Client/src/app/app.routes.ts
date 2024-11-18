import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { ChatsPageComponent } from './components/main/chats-page/chats-page.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        component: LoginComponent,
        pathMatch: 'full'
    },
    {
        path: 'chats',
        component: ChatsPageComponent,
        pathMatch: 'full'
    }
];
