import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiClient {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5056';

  get<T>(url: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}${url}`);
  }

  post<T>(url: string, body: unknown): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}${url}`, body);
  }

  put<T>(url: string, body: unknown): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}${url}`, body);
  }

  delete<T>(url: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}${url}`);
  }

  // ============ AUTENTICACION ============
  login(user: string, contrasena: string): Observable<any> {
    return this.post('/Usuarios/login', { user, contrasena });
  }

  registro(user: string, contrasena: string): Observable<any> {
    return this.post('/Usuarios/registro', { user, contrasena });
  }

  getUsuarios(): Observable<any> {
    return this.get('/Usuarios/lista');
  }

  // ============ PRODUCTOS ============
  getProductos(): Observable<any> {
    return this.get('/GestionProductos/lista-productos');
  }

  getProductosPorCategoria(categoriaId: number): Observable<any> {
    return this.get(`/GestionProductos/categoria/${categoriaId}`);
  }

  crearProducto(producto: any): Observable<any> {
    return this.post('/GestionProductos/crear-producto', producto);
  }

  actualizarProducto(id: number, producto: any): Observable<any> {
    return this.put(`/GestionProductos/actualizar-producto/${id}`, producto);
  }

  eliminarProducto(id: number): Observable<any> {
    return this.delete(`/GestionProductos/eliminar-producto/${id}`);
  }

  getComentariosProducto(productoId: number): Observable<any> {
    return this.get(`/GestionProductos/comentarios/${productoId}`);
  }

  // ============ CATEGORIAS ============
  getCategorias(): Observable<any> {
    return this.get('/GestionCategorias/lista-categorias');
  }

  // ============ CARRITO ============
  agregarAlCarrito(username: string, productoId: number): Observable<any> {
    return this.post(`/Carrito/${username}/agregar/${productoId}`, {});
  }

  eliminarDelCarrito(username: string, productoId: number): Observable<any> {
    return this.delete(`/Carrito/${username}/eliminar/${productoId}`);
  }

  verCarrito(username: string): Observable<any> {
    return this.get(`/Carrito/${username}/items`);
  }

  calcularTotal(username: string): Observable<any> {
    return this.get(`/Carrito/${username}/total`);
  }

  comprar(username: string): Observable<any> {
    return this.post(`/Carrito/${username}/comprar`, {});
  }

  // ============ COMENTARIOS ============
  getTodosComentarios(): Observable<any> {
    return this.get('/GestionComentarios/MostrarTodos');
  }

  getComentariosPorProducto(productoId: number): Observable<any> {
    return this.get(`/GestionComentarios/comentarios-por-producto/${productoId}`);
  }

  getComentariosPorUsuario(usuarioId: number): Observable<any> {
    return this.get(`/GestionComentarios/comentarios-por-usuario/${usuarioId}`);
  }

  agregarComentario(comentario: any): Observable<any> {
    return this.post('/GestionComentarios/AgregarComentario', comentario);
  }

  editarComentario(id: number, texto: string): Observable<any> {
    return this.put(`/GestionComentarios/editar-comentario/${id}`, texto);
  }

  eliminarComentario(id: number): Observable<any> {
    return this.delete(`/GestionComentarios/eliminar-comentario/${id}`);
  }
}