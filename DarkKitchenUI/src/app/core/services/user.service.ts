import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegisterRequest } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7134/api';

  register(data: RegisterRequest) {
    return this.http.post(`${this.apiUrl}/users`, data);
  }
}
