import { Routes } from '@angular/router';
import { LayoutComponent } from './features/layout/layout.component';
import { SignupComponent } from './features/signup/signup.component';
import { LoginComponent } from './features/login/login.component';
import { NotFoundComponent } from './features/not-found/not-found.component';
import { ChatsPageComponent } from './features/chats-page/chats-page.component';

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
        path: 'signup',
        component: SignupComponent,
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
    },
    {
        path: "**",
        component: NotFoundComponent
    }
];
