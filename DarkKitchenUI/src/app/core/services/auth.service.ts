import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { LoginRequest, LoginResponse } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly apiUrl = 'https://localhost:7134/api';
  private readonly tokenKey = 'token';

  login(credentials: LoginRequest) {
    return this.http.post<LoginResponse>(`${this.apiUrl}/sessions`, credentials).pipe(
      tap(response => localStorage.setItem(this.tokenKey, response.token))
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getAuthHeaders() {
    return { Authorization: `Bearer ${this.getToken()}` };
  }

  getUserName(): string | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['name'] ?? payload['unique_name'] ?? payload['email'] ?? null;
    } catch {
      return null;
    }
  }
}
