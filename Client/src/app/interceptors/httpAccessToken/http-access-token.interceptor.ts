import { HttpInterceptorFn } from '@angular/common/http';
import { StorageService } from '../../services/storage/storage.service';
import { inject } from '@angular/core';

export const httpAccessTokenInterceptor: HttpInterceptorFn = (req, next) => {
  let storageService = inject(StorageService);

  const accessToken = storageService.getAccessTokenFromSessionStorage();
  if (accessToken) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  return next(req);
};
