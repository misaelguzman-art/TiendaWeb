import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ApiClient } from '../core/http/api-client';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="container py-5">
      <div class="row justify-content-center">
        <div class="col-md-6 col-lg-5">
          <div class="card shadow-lg border-0 rounded-4">
            <div class="card-body p-5">
              <h2 class="text-center mb-4 fw-bold">Crear Cuenta</h2>
              
              <div *ngIf="mensajeError" class="alert alert-danger">
                {{ mensajeError }}
              </div>

              <div *ngIf="mensajeExito" class="alert alert-success">
                {{ mensajeExito }}
              </div>

              <form (ngSubmit)="onRegistro()">
                <div class="mb-3">
                  <label class="form-label fw-bold">Usuario</label>
                  <input 
                    type="text" 
                    class="form-control form-control-lg" 
                    [(ngModel)]="user" 
                    name="user"
                    placeholder="Elige un nombre de usuario"
                    required
                    minlength="3">
                </div>

                <div class="mb-3">
                  <label class="form-label fw-bold">Contrasena</label>
                  <input 
                    type="password" 
                    class="form-control form-control-lg" 
                    [(ngModel)]="password" 
                    name="password"
                    placeholder="Crea una contrasena"
                    required
                    minlength="4">
                </div>

                <div class="mb-4">
                  <label class="form-label fw-bold">Confirmar Contrasena</label>
                  <input 
                    type="password" 
                    class="form-control form-control-lg" 
                    [(ngModel)]="confirmPassword" 
                    name="confirmPassword"
                    placeholder="Confirma tu contrasena"
                    required>
                  <small *ngIf="password && confirmPassword && password !== confirmPassword" class="text-danger">
                    Las contrasenas no coinciden
                  </small>
                </div>

                <button type="submit" class="btn btn-success btn-lg w-100" 
                        [disabled]="cargando || password !== confirmPassword">
                  {{ cargando ? 'Registrando...' : 'Registrarse' }}
                </button>
              </form>

              <div class="text-center mt-3">
                <p class="text-muted">
                  Ya tienes cuenta? 
                  <a routerLink="/login" class="text-primary fw-bold">Inicia sesion aqui</a>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class Registro {
  private api = inject(ApiClient);
  private router = inject(Router);

  user = '';
  password = '';
  confirmPassword = '';
  cargando = false;
  mensajeError = '';
  mensajeExito = '';

  onRegistro() {
    // Validaciones
    if (!this.user || !this.password || !this.confirmPassword) {
      this.mensajeError = 'Por favor, completa todos los campos';
      return;
    }

    if (this.user.length < 3) {
      this.mensajeError = 'El usuario debe tener al menos 3 caracteres';
      return;
    }

    if (this.password.length < 4) {
      this.mensajeError = 'La contrasena debe tener al menos 4 caracteres';
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.mensajeError = 'Las contrasenas no coinciden';
      return;
    }

    this.cargando = true;
    this.mensajeError = '';
    this.mensajeExito = '';

    this.api.registro(this.user, this.password).subscribe({
      next: (response) => {
        console.log('Respuesta del servidor:', response);
        this.cargando = false;
        this.mensajeExito = 'Cuenta creada con exito. Redirigiendo al login...';
        
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (error) => {
        console.error('Error en registro:', error);
        console.error('Status:', error.status);
        console.error('Mensaje:', error.error);
        this.cargando = false;
        
        // Si el error es 400, mostrar el mensaje del servidor
        if (error.status === 400) {
          this.mensajeError = error.error || 'El usuario ya existe.';
        } else {
          this.mensajeError = 'Error al registrar usuario. Intenta de nuevo.';
        }
      }
    });
  }
}