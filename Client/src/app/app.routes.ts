import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { NotFoundComponent } from './features/not-found/not-found.component';
import { MainComponent } from './features/main/main.component';
import { RegisterComponent } from './features/register/register.component';
import { EmailConfirmComponent } from './features/email-confirm/email-confirm.component';
import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { LayoutWithThemeSwitchComponent } from './layouts/layout-with-theme-switch/layout-with-theme-switch.component';
import { RegistrationSuccessComponent } from './features/registration-success/registration-success.component';
import { ForgotPasswordComponent } from './features/forgot-password/forgot-password.component';
import { ForgotPasswordSuccessComponent } from './features/forgot-password-success/forgot-password-success.component';
import { ResetPasswordComponent } from './features/reset-password/reset-password.component';

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
            },
            {
                path: 'forgot-password',
                component: ForgotPasswordComponent,
                pathMatch: 'full'
            },
            {
                path: 'forgot-password/success',
                component: ForgotPasswordSuccessComponent,
                pathMatch: 'full'
            },
            {
                path: 'reset-password',
                component: ResetPasswordComponent,
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
                component: MainComponent
            },
            {
                path: 'chats/add/private/:userId',
                component: MainComponent
            },
            {
                path: 'chats/add/group',
                component: MainComponent
            }
        ]
    },
    {
        path: "**",
        component: LayoutWithThemeSwitchComponent,
        children: [
            {
                path: "**",
                component: NotFoundComponent
            }
        ]
    }
];
