import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const adminGuard = () => {
  const router = inject(Router);
  const usuarioGuardado = localStorage.getItem('usuario');
  
  if (!usuarioGuardado) {
    return router.parseUrl('/login');
  }

  const usuario = JSON.parse(usuarioGuardado);
  
  if (usuario.rol === 'Admin') {
    return true;
  }

  return router.parseUrl('/productos');
};