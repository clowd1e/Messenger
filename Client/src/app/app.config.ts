import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { httpAccessTokenInterceptor } from './interceptors/httpAccessToken/http-access-token.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([
      httpAccessTokenInterceptor
    ])),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes)]
};
