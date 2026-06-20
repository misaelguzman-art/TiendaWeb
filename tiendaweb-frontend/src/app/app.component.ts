import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, CommonModule],
  template: `
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
      <div class="container">
        <a class="navbar-brand" routerLink="/">TiendaWeb</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav me-auto">
            <li class="nav-item">
              <a class="nav-link" routerLink="/productos" routerLinkActive="active">Productos</a>
            </li>
            <li class="nav-item" *ngIf="usuario?.rol === 'Admin'">
              <a class="nav-link" routerLink="/admin/productos" routerLinkActive="active">Gestionar Productos</a>
            </li>
          </ul>

          <ul class="navbar-nav">
            <ng-container *ngIf="!usuario">
              <li class="nav-item">
                <a class="nav-link" routerLink="/login">Iniciar Sesion</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" routerLink="/registro">Registrarse</a>
              </li>
            </ng-container>

            <ng-container *ngIf="usuario">
              <li class="nav-item">
                <a class="nav-link" routerLink="/carrito" routerLinkActive="active">
                  Carrito
                </a>
              </li>
              <li class="nav-item dropdown">
                <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown">
                  {{ usuario.user }}
                  <span class="badge bg-info ms-1">{{ usuario.rol }}</span>
                </a>
                <ul class="dropdown-menu dropdown-menu-end">
                  <li><span class="dropdown-item-text">
                    <strong>Rol:</strong> {{ usuario.rol }}
                  </span></li>
                  <li><hr class="dropdown-divider"></li>
                  <li><button class="dropdown-item text-danger" (click)="cerrarSesion()">
                    Cerrar Sesion
                  </button></li>
                </ul>
              </li>
            </ng-container>
          </ul>
        </div>
      </div>
    </nav>

    <div class="container mt-3">
      <router-outlet></router-outlet>
    </div>
  `
})
export class AppComponent implements OnInit {
  private router = inject(Router);
  usuario: any = null;

  ngOnInit() {
    this.actualizarUsuario();

    window.addEventListener('storage', () => {
      this.actualizarUsuario();
    });
  }

  actualizarUsuario() {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (usuarioGuardado) {
      this.usuario = JSON.parse(usuarioGuardado);
    } else {
      this.usuario = null;
    }
  }

  cerrarSesion() {
    localStorage.removeItem('usuario');
    this.usuario = null;
    this.router.navigate(['/login']);
  }
}