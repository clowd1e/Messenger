import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { accessTokenInterceptor } from './shared/interceptors/access-token.interceptor';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';
import { refreshTokenInterceptorInterceptor } from './shared/interceptors/refresh-token-interceptor.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideToastr({
      closeButton: true,
      timeOut: 4000,
      extendedTimeOut: 2000,
      easing: 'ease-in',
      progressBar: true,
      progressAnimation: 'increasing',
    }),
    provideHttpClient(withInterceptors([
      accessTokenInterceptor,
      refreshTokenInterceptorInterceptor
    ])),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes)]
};
