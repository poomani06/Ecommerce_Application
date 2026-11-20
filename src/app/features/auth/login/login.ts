
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { Auth } from '../../../core/services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  email = '';
  password = '';
  isLoading = false;
  errorMessage = '';

  constructor(
    private auth: Auth,
    private router: Router
  ) {}

  login(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.auth.login({
      email: this.email,
      password: this.password
    }).subscribe({
      next: (response: { role: string }) => {
  this.isLoading = false;

  if (response.role === 'Admin') {
    this.router.navigate(['/admin/products']);
  } else {
    this.router.navigate(['/products']);
  }
},
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;
        this.errorMessage =
          error.error?.message || 'Login failed. Please check your details.';
      }
    });
  }
}
