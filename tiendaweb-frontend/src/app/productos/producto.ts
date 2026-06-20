import { Component, inject, OnInit } from '@angular/core';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiClient } from '../core/http/api-client';
import { ComentariosComponent } from './comentarios';

export interface ProductoItem {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  plataforma: string;
  duracionDias: number;
  categoriaId?: number;
}

@Component({
  selector: 'app-producto',
  standalone: true,
  imports: [RouterLink, FormsModule, CommonModule, ComentariosComponent],
  templateUrl: './producto.html',
})
export class Producto implements OnInit {
  private api = inject(ApiClient);
  private route = inject(ActivatedRoute);

  producto: ProductoItem = {
    id: 0,
    nombre: '',
    descripcion: '',
    precio: 0,
    plataforma: '',
    duracionDias: 30
  };

  esEdicion = false;
  mensaje = '';
  mensajeError = false;
  username = '';
  mostrarComentarios = false;

  ngOnInit(): void {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (usuarioGuardado) {
      const usuario = JSON.parse(usuarioGuardado);
      this.username = usuario.user;
    }

    // Verificar si es edición (tiene ID en la URL)
    this.route.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.esEdicion = true;
        this.cargarProducto(id);
        this.mostrarComentarios = true;
      } else {
        this.esEdicion = false;
        this.mostrarComentarios = false;
      }
    });
  }

  cargarProducto(id: number) {
     this.api.get<ProductoItem[]>('/GestionProductos/lista-productos').subscribe({
      next: data => {
        const encontrado = data.find(p => p.id === Number(id));
        if (encontrado) {
          this.producto = encontrado;
          console.log(' Producto cargado:', this.producto);
        } else {
          this.mensaje = ' Producto no encontrado';
          this.mensajeError = true;
        }
      },
      error: error => {
        console.error(' Error al cargar producto:', error);
        this.mensaje = ' Error al cargar el producto';
        this.mensajeError = true;
      }
    });
  }

  guardarProducto() {
    if (this.esEdicion) { 
      this.mensaje = ' Funcionalidad de edición pendiente';
      this.mensajeError = false;
    } else { 
      this.api.post('/GestionProductos/crear-producto', this.producto).subscribe({
        next: () => {
          this.mensaje = 'Producto creado con éxito';
          this.mensajeError = false; 
          this.producto = {
            id: 0,
            nombre: '',
            descripcion: '',
            precio: 0,
            plataforma: '',
            duracionDias: 30
          };
        },
        error: error => {
          console.error(' Error al crear producto:', error);
          this.mensaje = ' Error al crear el producto';
          this.mensajeError = true;
        }
      });
    }
  }
}