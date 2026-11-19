
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { Auth } from '../../../core/services/auth';

interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  phone: string;
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  name = '';
  email = '';
  password = '';
  phone = '';
  isLoading = false;
  errorMessage = '';

  constructor(
    private auth: Auth,
    private router: Router
  ) {}

  register(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const data: RegisterRequest = {
      name: this.name,
      email: this.email,
      password: this.password,
      phone: this.phone
    };

    this.auth.register(data).subscribe({
      next: () => {
        this.isLoading = false;
        localStorage.setItem('authMessage', 'Registration successful! Welcome to E-Commerce.');
        this.router.navigate(['/']);
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;
        this.errorMessage =
          error.error?.message || 'Registration failed.';
      }
    });
  }
}
