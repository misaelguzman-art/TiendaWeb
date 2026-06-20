import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './nav-menu.html',
})
export class NavMenu implements OnInit {
  private router = inject(Router);

  esAdmin = false;
  estaAutenticado = false;
  menuAdminAbierto = false;

  ngOnInit(): void {
    this.actualizarEstado();
    this.router.events.subscribe(() => {
      this.actualizarEstado();
    });
  }

  actualizarEstado() {
    const usuarioStr = localStorage.getItem('usuario');
    if (usuarioStr) {
      this.estaAutenticado = true;
      try {
        const usuario = JSON.parse(usuarioStr);
        this.esAdmin = usuario.rol === 'Admin';
      } catch (e) {
        this.esAdmin = false;
      }
    } else {
      this.estaAutenticado = false;
      this.esAdmin = false;
    }
  }

  toggleAdminMenu(event: Event) {
    event.preventDefault();
    this.menuAdminAbierto = !this.menuAdminAbierto;
  }

  logout() {
    localStorage.removeItem('usuario');
    this.estaAutenticado = false;
    this.esAdmin = false;
    this.router.navigate(['/login']);
  }
}

