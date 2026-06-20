import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiClient } from '../core/http/api-client';

export interface Comentario {
  id: number;
  texto: string;
  usuarioId: number;
  productoId: number;
  fecha: string;
  calificacion?: number;
  usuario?: {
    user: string;
  };
}

@Component({
  selector: 'app-comentarios',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="comentarios-section mt-5">
      <h4 class="mb-4">Comentarios</h4>

      <!-- Mensaje de estado -->
      <div *ngIf="mensaje" [class]="mensajeError ? 'alert alert-danger' : 'alert alert-success'">
        {{ mensaje }}
      </div>

      <!-- Formulario para agregar comentario -->
      <div class="card mb-4">
        <div class="card-header bg-light">
          <h6 class="mb-0">Dejar un comentario</h6>
        </div>
        <div class="card-body">
          <textarea 
            class="form-control mb-3" 
            [(ngModel)]="nuevoComentario"
            placeholder="Escribe tu opinión sobre este producto..."
            rows="3"
            name="comentario">
          </textarea>
          <button 
            class="btn btn-primary"
            (click)="agregarComentario()"
            [disabled]="!nuevoComentario.trim() || agregando">
            {{ agregando ? 'Enviando...' : 'Enviar Comentario' }}
          </button>
        </div>
      </div>

      <!-- Lista de comentarios -->
      <div class="comentarios-list">
        <div *ngIf="comentarios.length === 0" class="text-center text-muted py-4">
          <p>Sin comentarios aún. ¡Sé el primero en comentar!</p>
        </div>

        <div *ngFor="let comentario of comentarios" class="card mb-3 border-start border-3 border-primary">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-start">
              <div>
                <h6 class="card-title mb-1">
                  <strong>{{ comentario.usuario?.user || 'Usuario' }}</strong>
                </h6>
                <small class="text-muted">{{ formatearFecha(comentario.fecha) }}</small>
              </div>
            </div>
            <p class="card-text mt-2">{{ comentario.texto }}</p>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .comentarios-section {
      background-color: #f8f9fa;
      padding: 20px;
      border-radius: 8px;
    }
    .comentarios-list {
      max-height: 500px;
      overflow-y: auto;
    }
  `]
})
export class ComentariosComponent implements OnInit {
  @Input() productoId!: number;
  @Input() username: string = '';

  private api = inject(ApiClient);

  comentarios: Comentario[] = [];
  nuevoComentario: string = '';
  mensaje: string = '';
  mensajeError: boolean = false;
  agregando: boolean = false;

  ngOnInit() {
    this.cargarComentarios();
  }

  cargarComentarios() {
    this.api.getComentariosPorProducto(this.productoId).subscribe({
      next: (data: Comentario[]) => {
        this.comentarios = data;
      },
      error: (error: any) => {
        console.error('Error al cargar comentarios:', error);
      }
    });
  }

  agregarComentario() {
    if (!this.nuevoComentario.trim()) {
      return;
    }

    this.agregando = true;
      let usuarioId = 0;
      const userStr = localStorage.getItem('usuario');
      if (userStr) {
        try {
          const userObj = JSON.parse(userStr);
          usuarioId = userObj.id || 0;
        } catch (e) {}
      }

      const nuevoComentario = {
        texto: this.nuevoComentario,
        usuarioId: usuarioId,
        productoId: this.productoId,
        fecha: new Date().toISOString().slice(0, 19).replace('T', ' '),
        calificacion: 0
      };

    this.api.post('/GestionComentarios/AgregarComentario', nuevoComentario).subscribe({
      next: () => {
        this.mensaje = 'Comentario agregado exitosamente';
        this.mensajeError = false;
        this.nuevoComentario = '';
        this.agregando = false;
        this.cargarComentarios();
        setTimeout(() => this.mensaje = '', 3000);
      },
      error: (error: any) => {
        console.error('Error al agregar comentario:', error);
        this.mensaje = 'Error al agregar comentario';
        this.mensajeError = true;
        this.agregando = false;
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
