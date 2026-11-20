
import { Component, OnInit, ChangeDetectorRef} from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ProductService } from '../../../core/services/product';
import { CategoryService } from '../../../core/services/category';
import { Product } from '../../../models/product.model';
import { Category } from '../../../models/category.model';
import { CreateProduct } from '../../../models/product.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-products',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products implements OnInit {

  products: Product[] = [];
  categories: Category[] = [];
  newProduct: CreateProduct = {
  name: '',
  description: '',
  price: 0,
  stock: 0,
  imageUrl: '',
  categoryId: 0
};

showAddForm = false;

  errorMessage = '';
  successMessage = '';

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadProducts();
    this.loadCategories();
  }

  loadProducts(): void {
  this.productService.getProducts('', undefined, 1, 100).subscribe({
    next: (response: { products: Product[] }) => {
      console.log('API Response:', response);
      this.products = response.products;
      console.log('Products loaded:', this.products);
    },
    error: (error: HttpErrorResponse) => {
      console.error('Products API Error:', error);
      this.errorMessage = 'Failed to load products.';
    }
  });
}

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (data: Category[]) => {
        this.categories = data;
      },
      error: () => {
        this.errorMessage = 'Failed to load categories.';
      }
    });
  }
  addProduct(): void {
  this.productService.createProduct(this.newProduct).subscribe({
    next: () => {
      this.successMessage = 'Product added successfully.';
      this.errorMessage = '';
      this.showAddForm = false;

      this.newProduct = {
        name: '',
        description: '',
        price: 0,
        stock: 0,
        imageUrl: '',
        categoryId: 0
      };

      this.loadProducts();
    },
    error: () => {
      this.errorMessage = 'Failed to add product.';
      this.successMessage = '';
    }
  });
}

  deleteProduct(id: number): void {
    if (!confirm('Are you sure you want to deactivate this product?')) {
      return;
    }

    this.productService.deleteProduct(id).subscribe({
      next: (response: { message: string }) => {
        this.successMessage = response.message;
        this.loadProducts();
      },
      error: () => {
        this.errorMessage = 'Failed to deactivate product.';
      }
    });
  }
}
