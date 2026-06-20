import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { ApiClient } from '../core/http/api-client';

@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './carrito.html'
})
export class Carrito implements OnInit {
  private api = inject(ApiClient);
  private router = inject(Router);

  items: any[] = [];
  total = 0;
  subtotal = 0;
  descuentoPorcentaje = 0;
  comprando = false;
  mensaje = '';
  mensajeError = false;
  username = '';
  rol = '';
  codigosActivacion: any[] = [];
  mostrarCodigosActivacion = false;

  ngOnInit() {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (usuarioGuardado) {
      const usuario = JSON.parse(usuarioGuardado);
      this.username = usuario.user;
      this.rol = usuario.rol || 'Cliente';
      this.cargarCarrito();
    } else {
      this.router.navigate(['/login']);
    }
  }

  cargarCarrito() {
    this.api.verCarrito(this.username).subscribe({
      next: (data: any) => {
        this.items = data || [];
        this.calcularTotal();
      },
      error: (error: any) => {
        console.error('Error al cargar carrito:', error);
        this.mensaje = 'Error al cargar el carrito';
        this.mensajeError = true;
      }
    });
  }

  calcularTotal() {
    this.api.calcularTotal(this.username).subscribe({
      next: (data: any) => {
        this.subtotal = data.subtotal || data.total;
        this.total = data.total;
        this.descuentoPorcentaje = parseFloat(data.descuentoAplicado) || 0;
      },
      error: (error: any) => {
        console.error('Error al calcular total:', error);
      }
    });
  }

  eliminarItem(productoId: number) {
    this.api.eliminarDelCarrito(this.username, productoId).subscribe({
      next: () => {
        this.mensaje = 'Producto eliminado del carrito';
        this.mensajeError = false;
        this.cargarCarrito();
        setTimeout(() => this.mensaje = '', 3000);
      },
      error: (error: any) => {
        console.error('Error al eliminar item:', error);
        this.mensaje = 'Error al eliminar producto';
        this.mensajeError = true;
        setTimeout(() => this.mensaje = '', 3000);
      }
    });
  }

  vaciarCarrito() {
    if (!confirm('¿Estás seguro de que deseas vaciar el carrito?')) return;
    
    const itemsCopy = [...this.items];
    itemsCopy.forEach(item => {
      this.api.eliminarDelCarrito(this.username, item.id).subscribe();
    });
    this.items = [];
    this.total = 0;
    this.subtotal = 0;
    this.mensaje = 'Carrito vaciado';
    this.mensajeError = false;
    setTimeout(() => this.mensaje = '', 3000);
  }

  realizarCompra() {
    if (this.items.length === 0) {
      this.mensaje = 'El carrito está vacío';
      this.mensajeError = true;
      return;
    }

    if (!confirm('¿Deseas continuar con la compra?')) return;

    this.comprando = true;
    this.api.comprar(this.username).subscribe({
      next: (response: any) => {
        this.comprando = false;
        this.codigosActivacion = response.codigosActivacion || [];
        this.mostrarCodigosActivacion = true;
        this.mensaje = '✓ ¡Compra realizada exitosamente!';
        this.mensajeError = false;
        this.items = [];
        this.total = 0;
        this.subtotal = 0;
      },
      error: (error: any) => {
        this.comprando = false;
        console.error('Error al realizar compra:', error);
        this.mensaje = error.error?.message || 'Error al realizar la compra';
        this.mensajeError = true;
      }
    });
  }

  copiarCodigo(codigo: string) {
    navigator.clipboard.writeText(codigo).then(() => {
      this.mensaje = '✓ Código copiado al portapapeles';
      this.mensajeError = false;
      setTimeout(() => this.mensaje = '', 2000);
    });
  }

  regresarProductos() {
    this.mostrarCodigosActivacion = false;
    this.router.navigate(['/productos']);
  }

  esPremium(): boolean {
    return this.descuentoPorcentaje > 0;
  }
}