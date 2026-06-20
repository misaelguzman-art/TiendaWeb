import { Routes } from '@angular/router';
import { Productos } from './productos/productos';
import { Producto } from './productos/producto';
import { Login } from './auth/login';
import { Registro } from './auth/registro';
import { Carrito } from './carrito/carrito';
import { AdminProductos } from './admin/admin-productos';
import { AdminPedidos } from './admin/admin-pedidos';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'registro', component: Registro },
  { path: 'productos', component: Productos, canActivate: [authGuard] },
  { path: 'carrito', component: Carrito, canActivate: [authGuard] },
  { path: 'admin/productos', component: AdminProductos, canActivate: [authGuard, adminGuard] },
  { path: 'admin/pedidos', component: AdminPedidos, canActivate: [authGuard, adminGuard] },
  { path: 'producto/nuevo', component: Producto, canActivate: [authGuard, adminGuard] },
  { path: 'producto/:id', component: Producto, canActivate: [authGuard] }
];



