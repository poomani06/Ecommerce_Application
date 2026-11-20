export interface Product {
  productId: number;
  name: string;
  description?: string;
  price: number;
  stock: number;
  imageUrl?: string;
  isActive: boolean;
  categoryId: number;
  categoryName: string;
}

export interface ProductListResponse {
  products: Product[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}
export interface CreateProduct {
  name: string;
  description?: string;
  price: number;
  stock: number;
  imageUrl?: string;
  categoryId: number;
}

export interface UpdateProduct extends CreateProduct {
  isActive: boolean;
}