import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';


import {
  LoginRequest,
  RegisterRequest,
  AuthResponse
} from '../../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class Auth {
  private apiUrl = `${environment.apiUrl}/auth`;

  isLoggedIn = signal(!!localStorage.getItem('token'));

  constructor(private http: HttpClient) {}

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, data)
      .pipe(
        tap((response: AuthResponse) => {
  localStorage.setItem('token', response.token);
  localStorage.setItem('user', JSON.stringify(response));
  this.isLoggedIn.set(true);
})
      );
  }

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/register`, data)
      .pipe(
        tap((response: AuthResponse) => {
  localStorage.setItem('token', response.token);
  localStorage.setItem('user', JSON.stringify(response));
  this.isLoggedIn.set(true);
})
      );
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
  isAdmin(): boolean {
  const user = localStorage.getItem('user');

  if (!user) {
    return false;
  }

  const authResponse: AuthResponse = JSON.parse(user);
  return authResponse.role === 'Admin';
}

  logout(): void {
  localStorage.removeItem('token');
  localStorage.removeItem('user');
  this.isLoggedIn.set(false);
}
}