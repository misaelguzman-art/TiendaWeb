import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiClient } from '../core/http/api-client';

export interface PedidoAdmin {
  id: number;
  fechaPedido: string;
  total: number;
  estado: string;
  usuario: {
    user: string;
  };
  cantidadItems: number;
}

@Component({
  selector: 'app-admin-pedidos',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-pedidos.html'
})
export class AdminPedidos implements OnInit {
  private api = inject(ApiClient);
  pedidos: PedidoAdmin[] = [];
  cargando = true;
  mensaje = '';

  ngOnInit(): void {
    this.cargarPedidos();
  }

  cargarPedidos() {
    this.api.get<PedidoAdmin[]>('/GestionPedidos/todos').subscribe({
      next: (data) => {
        this.pedidos = data;
        this.cargando = false;
      },
      error: (err) => {
        console.error('Error cargando pedidos', err);
        this.mensaje = 'No se pudieron cargar los pedidos.';
        this.cargando = false;
      }
    });
  }

  formatearFecha(fecha: string): string {
    const date = new Date(fecha);
    return date.toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}
