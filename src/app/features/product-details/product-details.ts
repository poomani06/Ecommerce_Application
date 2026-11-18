import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { ProductService } from '../../core/services/product';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-details',
  imports: [],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css'
})
export class ProductDetails implements OnInit {
  product: Product | null = null;
  isLoading = true;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    const productId = Number(this.route.snapshot.paramMap.get('id'));

    if (!productId || productId <= 0) {
      this.errorMessage = 'Invalid product ID.';
      this.isLoading = false;
      return;
    }

    this.productService.getProductById(productId).subscribe({
      next: (data: Product) => {
        this.product = data;
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.message || 'Unable to load product details.';
        this.isLoading = false;
      }
    });
  }
}