import { HttpErrorResponse, HttpInterceptorFn, HttpStatusCode } from '@angular/common/http';
import { ApiService } from '../services/api.service';
import { inject } from '@angular/core';
import { StorageService } from '../services/storage.service';
import { RefreshTokenRequest } from '../models/requests/refresh-token-request';
import { catchError, switchMap, throwError } from 'rxjs';
import { Router } from '@angular/router';

export const refreshTokenInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const apiService = inject(ApiService);
  const storageService = inject(StorageService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      console.log("Caught error in interceptor:", error);
      if (error.status == HttpStatusCode.Unauthorized && !req.url.includes('api/refresh')) {
        console.log("Fired refresh token interceptor");
        let accessToken = storageService.getAccessTokenFromLocalStorage();
        let refreshToken = storageService.getRefreshTokenFromLocalStorage();
        let request: RefreshTokenRequest = {
          accessToken: accessToken,
          refreshToken: refreshToken
        }
        console.log("Refresh token request:", request);

        return apiService.refresh(request).pipe(
          switchMap((response) => {
            storageService.setAccessTokenToLocalStorage(response.accessToken);
            storageService.setRefreshTokenToLocalStorage(response.refreshToken);
            const retryReq = req.clone({
              setHeaders: { Authorization: `Bearer ${response.accessToken}` }
            });
            return next(retryReq);
          }),
          catchError((refreshError) => {
            storageService.removeAccessTokenFromLocalStorage();
            storageService.removeRefreshTokenFromLocalStorage();
            router.navigate(['/login']);

            return throwError(() => refreshError);
          })
        );
      };
      return throwError(() => error);
    })
  );
};
