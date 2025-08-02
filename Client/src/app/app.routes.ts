import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { NotFoundComponent } from './features/not-found/not-found.component';
import { ChatsPageComponent } from './features/chats-page/chats-page.component';
import { RegisterComponent } from './features/register/register.component';
import { EmailConfirmComponent } from './features/email-confirm/email-confirm.component';
import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { LayoutWithThemeSwitchComponent } from './layouts/layout-with-theme-switch/layout-with-theme-switch.component';
import { RegistrationSuccessComponent } from './features/registration-success/registration-success.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: '',
        component: LayoutWithThemeSwitchComponent,
        children: [
            {
                path: 'login',
                component: LoginComponent,
                pathMatch: 'full'
            },
            {
                path: 'signup',
                component: RegisterComponent,
                pathMatch: 'full'
            },
            {
                path: 'confirm-email',
                component: EmailConfirmComponent,
                pathMatch: 'full'
            },
            {
                path: 'signup/success',
                component: RegistrationSuccessComponent,
                pathMatch: 'full'
            }
        ]
    },
    {
        path: '',
        component: DefaultLayoutComponent,
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
