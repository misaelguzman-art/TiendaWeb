import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApiClient } from '../core/http/api-client';

export interface ProductoItem {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
}

@Component({
  selector: 'app-producto',
  imports: [
    RouterLink
  ],
  templateUrl: './producto.html',
})
export class Producto implements OnInit {
  private api = inject(ApiClient);
  private url: string = 'http://localhost:4200/GestionProductos';

  productos: ProductoItem[] = [];

  ngOnInit(): void {
    console.log('iniciando');
    this.api.get<ProductoItem[]>(this.url + '/lista-productos').subscribe({
      next: data => this.productos = data,
      error: error => console.error('Error al obtener los productos', error)
    });
  }
}
