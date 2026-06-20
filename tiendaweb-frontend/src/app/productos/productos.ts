import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { ApiClient } from '../core/http/api-client';

export interface Producto {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  plataforma: string;
  duracionDias: number;
  categoriaId?: number;
}

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './productos.html'
})
export class Productos implements OnInit {
  private api = inject(ApiClient);
  private router = inject(Router);

  productos: Producto[] = [];
  username: string = '';
  rol: string = '';
  mensaje: string = '';
  mensajeError: boolean = false;
  productosEnCarrito: Set<number> = new Set();
  categoriasMap: Map<number, string> = new Map();

  ngOnInit() {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (usuarioGuardado) {
      const usuario = JSON.parse(usuarioGuardado);
      this.username = usuario.user;
      this.rol = usuario.rol || 'Cliente';
    this.cargarCategorias();
    this.cargarProductos();
    this.cargarCarrito();
    } else {
      this.router.navigate(['/login']);
    }
  }

  cargarCategorias() {
    this.api.get<{id: number, nombre: string}[]>('/GestionCategorias/lista-categorias').subscribe({
      next: (data) => {
        this.categoriasMap = new Map(data.map(c => [c.id, c.nombre]));
      },
      error: () => {}
    });
  }

  cargarProductos() {
    this.api.getProductos().subscribe({
      next: (data: Producto[]) => {
        this.productos = data;
        console.log('Productos cargados:', data);
      },
      error: (error: any) => {
        console.error('Error al obtener productos:', error);
        this.mensaje = 'Error al cargar productos';
        this.mensajeError = true;
      }
    });
  }

  cargarCarrito() {
    if (this.rol !== 'Cliente' && this.rol !== 'ClienteVip') return;
    
    this.api.verCarrito(this.username).subscribe({
      next: (data: any[]) => {
        this.productosEnCarrito = new Set(data.map(item => item.id));
      },
      error: (error: any) => {
        console.error('Error al cargar carrito:', error);
      }
    });
  }

  agregarAlCarrito(productoId: number, productoNombre: string) {
    if (this.rol === 'Admin') {
      this.mensaje = 'Los administradores no pueden comprar productos';
      this.mensajeError = true;
      return;
    }

    if (!this.username) {
      this.router.navigate(['/login']);
      return;
    }

    this.api.agregarAlCarrito(this.username, productoId).subscribe({
      next: (response: any) => {
        this.productosEnCarrito.add(productoId);
        this.mensaje = response.mensaje || 'Producto agregado al carrito';
        this.mensajeError = false;
        setTimeout(() => this.mensaje = '', 3000);
      },
      error: (error: any) => {
        console.error('Error al agregar al carrito:', error);
        this.mensaje = 'Error al agregar producto al carrito';
        this.mensajeError = true;
        setTimeout(() => this.mensaje = '', 3000);
      }
    });
  }

  estaEnCarrito(productoId: number): boolean {
    return this.productosEnCarrito.has(productoId);
  }

  irAlCarrito() {
    this.router.navigate(['/carrito']);
  }

  nombreCategoria(categoriaId?: number): string {
    if (!categoriaId || categoriaId === -1) return 'N/A';
    return this.categoriasMap.get(categoriaId) ?? 'N/A';
  }

  esAdmin(): boolean {
    return this.rol === 'Admin';
  }
}