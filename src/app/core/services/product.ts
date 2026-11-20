import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  Product,
  ProductListResponse,
  CreateProduct,
  UpdateProduct
} from '../../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = `${environment.apiUrl}/products`;

  constructor(private http: HttpClient) {}

  getProducts(
    search = '',
    categoryId?: number,
    pageNumber = 1,
    pageSize = 10
  ): Observable<ProductListResponse> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (search.trim()) {
      params = params.set('search', search.trim());
    }

    if (categoryId) {
      params = params.set('categoryId', categoryId);
    }

    return this.http.get<ProductListResponse>(this.apiUrl, { params });
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }
  createProduct(product: CreateProduct): Observable<Product> {
  return this.http.post<Product>(this.apiUrl, product);
}

updateProduct(id: number, product: UpdateProduct): Observable<Product> {
  return this.http.put<Product>(`${this.apiUrl}/${id}`, product);
}

deleteProduct(id: number): Observable<{ message: string }> {
  return this.http.delete<{ message: string }>(`${this.apiUrl}/${id}`);
}
}