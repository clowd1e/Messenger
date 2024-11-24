import { HttpInterceptorFn } from '@angular/common/http';

export const httpAccessTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const accessToken = localStorage.getItem('accessToken');
  if (accessToken) {
    req.headers.set('Authorization', `Bearer ${accessToken}`);
  }

  return next(req);
};
