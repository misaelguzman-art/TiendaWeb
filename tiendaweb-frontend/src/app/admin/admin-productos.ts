import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiClient } from '../core/http/api-client';
import { ComentariosComponent } from '../productos/comentarios';

@Component({
  selector: 'app-admin-productos',
  standalone: true,
  imports: [CommonModule, FormsModule, ComentariosComponent],
  templateUrl: './admin-productos.html'
})
export class AdminProductos implements OnInit {
  private api = inject(ApiClient);

  productos: any[] = [];
  mostrarFormulario = false;
  editando = false;
  mensaje = '';
  mensajeError = false;

  productoForm = {
    id: 0,
    nombre: '',
    descripcion: '',
    precio: 0,
    plataforma: '',
    duracionDias: 30,
    codigoDescarga: '',
    categoriaId: 0
  };

  ngOnInit() {
    this.cargarProductos();
  }

  cargarProductos() {
    this.api.getProductos().subscribe({
      next: (data: any) => {
        this.productos = data;
      },
      error: (error: any) => {
        console.error('Error al cargar productos:', error);
        this.mensaje = 'Error al cargar productos';
        this.mensajeError = true;
      }
    });
  }

  guardarProducto() {
    if (this.editando) {
      this.api.actualizarProducto(this.productoForm.id, this.productoForm).subscribe({
        next: () => {
          this.mensaje = 'Producto actualizado con exito';
          this.mensajeError = false;
          this.cancelarEdicion();
          this.cargarProductos();
          setTimeout(() => this.mensaje = '', 3000);
        },
        error: (error: any) => {
          console.error('Error al actualizar producto:', error);
          this.mensaje = 'Error al actualizar producto';
          this.mensajeError = true;
        }
      });
    } else {
      this.api.crearProducto(this.productoForm).subscribe({
        next: () => {
          this.mensaje = 'Producto creado con exito';
          this.mensajeError = false;
          this.cancelarEdicion();
          this.cargarProductos();
          setTimeout(() => this.mensaje = '', 3000);
        },
        error: (error: any) => {
          console.error('Error al crear producto:', error);
          this.mensaje = 'Error al crear producto';
          this.mensajeError = true;
        }
      });
    }
  }

  editarProducto(producto: any) {
    this.editando = true;
    this.mostrarFormulario = true;
    this.productoForm = { ...producto };
  }

  cancelarEdicion() {
    this.editando = false;
    this.mostrarFormulario = false;
    this.productoForm = { id: 0, nombre: '', descripcion: '', precio: 0, plataforma: '', duracionDias: 30, codigoDescarga: '', categoriaId: 0 };
  }

  eliminarProducto(id: number) {
    if (confirm('Esta seguro de eliminar este producto?')) {
      this.api.eliminarProducto(id).subscribe({
        next: () => {
          this.mensaje = 'Producto eliminado con exito';
          this.mensajeError = false;
          this.cargarProductos();
          setTimeout(() => this.mensaje = '', 3000);
        },
        error: (error: any) => {
          console.error('Error al eliminar producto:', error);
          this.mensaje = 'Error al eliminar producto';
          this.mensajeError = true;
        }
      });
    }
  }
}