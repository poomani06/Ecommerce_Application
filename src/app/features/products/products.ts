
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { ProductService } from '../../core/services/product';
import { CategoryService } from '../../core/services/category';

import { Product, ProductListResponse } from '../../models/product.model';
import { Category } from '../../models/category.model';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-products',
  imports: [FormsModule, RouterLink],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products implements OnInit {

  products: Product[] = [];
  categories: Category[] = [];

  searchTerm = '';
  selectedCategoryId = '';

  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  totalCount = 0;

  isLoading = false;
  errorMessage = '';

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService
  ) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (data: Category[]) => {
        this.categories = data;
      },
      error: (error: HttpErrorResponse) => {
        console.error('Unable to load categories', error);
      }
    });
  }

  loadProducts(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const categoryId = this.selectedCategoryId
      ? Number(this.selectedCategoryId)
      : undefined;

    this.productService.getProducts(
      this.searchTerm,
      categoryId,
      this.currentPage,
      this.pageSize
    ).subscribe({
      next: (data: ProductListResponse) => {
        this.products = data.products;
        this.totalCount = data.totalCount;
        this.totalPages = data.totalPages;
        this.currentPage = data.pageNumber;
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.message || 'Unable to load products.';
        this.isLoading = false;
      }
    });
  }

  onSearchChange(): void {
    this.currentPage = 1;
    this.loadProducts();
  }

  onCategoryChange(): void {
    this.currentPage = 1;
    this.loadProducts();
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadProducts();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadProducts();
    }
  }
}
