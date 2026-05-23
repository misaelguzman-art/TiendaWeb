import {Component, inject} from '@angular/core';
import { ApiClient } from '../core/http/api-client';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-productos',
  imports: [
    RouterLink
  ],
  templateUrl: './productos.html',
})
export class Productos {
  private api = inject(ApiClient);
  private url = 'http://localhost:5056/GestionProductos'; //si el puerto de dotnet es diferente cambiar aquI
  //SPA UNA SOLA PAGINA
  productos: Producto[] = [];

  ngOnInit() {
    console.log('iniciando');
    this.api.get<Producto[]>(this.url+'/lista-productos').subscribe({
      next: data => this.productos = data,
      error: error => console.error('Error al obtener productos', error)
    });
  }
}

export interface Producto {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
}
