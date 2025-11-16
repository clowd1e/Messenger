import { HttpInterceptorFn } from '@angular/common/http';
import { StorageService } from '../services/storage.service';
import { inject } from '@angular/core';

export const accessTokenInterceptor: HttpInterceptorFn = (req, next) => {
  let storageService = inject(StorageService);
  
  const accessToken = storageService.getAccessTokenFromLocalStorage();
  if (accessToken) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }
  
  return next(req);
};
