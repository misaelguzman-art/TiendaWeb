import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const authGuard = () => {
  const router = inject(Router);
  const usuario = localStorage.getItem('usuario');
  
  if (usuario) {
    return true;
  }
  
  return router.parseUrl('/login');
};