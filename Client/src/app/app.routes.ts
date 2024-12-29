import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { ChatsPageComponent } from './pages/chats-page/chats-page.component';
import { LayoutComponent } from './pages/layout/layout.component';

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
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: 'chats',
                redirectTo: 'chats/#'
            },
            {
                path: 'chats/:chatId',
                component: ChatsPageComponent
            },
            {
                path: 'chats/add',
                component: ChatsPageComponent
            },
            {
                path: 'chats/add/:userId',
                component: ChatsPageComponent
            }
        ]
    }
];
