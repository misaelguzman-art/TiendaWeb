import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ApiClient } from '../core/http/api-client';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="container py-5">
      <div class="row justify-content-center">
        <div class="col-md-6 col-lg-5">
          <div class="card shadow-lg border-0 rounded-4">
            <div class="card-body p-5">
              <h2 class="text-center mb-4 fw-bold">Iniciar Sesion</h2>
              
              <div *ngIf="mensajeError" class="alert alert-danger">
                {{ mensajeError }}
              </div>

              <div *ngIf="mensajeExito" class="alert alert-success">
                {{ mensajeExito }}
              </div>

              <form (ngSubmit)="onLogin()">
                <div class="mb-3">
                  <label class="form-label fw-bold">Usuario</label>
                  <input 
                    type="text" 
                    class="form-control form-control-lg" 
                    [(ngModel)]="user" 
                    name="user"
                    placeholder="Ingresa tu usuario"
                    required>
                </div>

                <div class="mb-4">
                  <label class="form-label fw-bold">Contrasena</label>
                  <input 
                    type="password" 
                    class="form-control form-control-lg" 
                    [(ngModel)]="password" 
                    name="password"
                    placeholder="Ingresa tu contrasena"
                    required>
                </div>

                <button type="submit" class="btn btn-primary btn-lg w-100" [disabled]="cargando">
                  {{ cargando ? 'Ingresando...' : 'Iniciar Sesion' }}
                </button>
              </form>

              <div class="text-center mt-3">
                <p class="text-muted">
                  No tienes cuenta? 
                  <a routerLink="/registro" class="text-primary fw-bold">Registrate aqui</a>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class Login {
  private api = inject(ApiClient);
  private router = inject(Router);

  user = '';
  password = '';
  cargando = false;
  mensajeError = '';
  mensajeExito = '';

  onLogin() {
    if (!this.user || !this.password) {
      this.mensajeError = 'Por favor, completa todos los campos';
      return;
    }

    this.cargando = true;
    this.mensajeError = '';
    this.mensajeExito = '';

    this.api.login(this.user, this.password).subscribe({
      next: (response) => {
        console.log('Login exitoso:', response);
        this.cargando = false;
        
        localStorage.setItem('usuario', JSON.stringify({
          id: response.id,
          user: response.user,
          rol: response.rol
        }));
        
        this.mensajeExito = 'Inicio de sesion exitoso. Redirigiendo...';
        
        setTimeout(() => {
          this.router.navigate(['/productos']);
        }, 1500);
      },
      error: (error) => {
        console.error('Error en login:', error);
        this.cargando = false;
        this.mensajeError = 'Usuario o contrasena incorrectos';
      }
    });
  }
}