import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const clonerRequest = req.clone({
    withCredentials : true
  });
  
  return next(clonerRequest);
};
