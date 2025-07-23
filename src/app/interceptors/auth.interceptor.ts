import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token=sessionStorage.getItem('token');
  if(req.url.includes('/login') || req.url.includes('/home') || req.url.includes('/sellerauth')){
    return next(req);
  }
  if(token){
    const authReq=req.clone(
      {
        setHeaders:{
          Authorization:`Bearer ${token}`,
        },
      }
    );
    return next(authReq);
  }
  return next(req);
};
