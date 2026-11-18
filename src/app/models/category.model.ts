export interface Category {
  categoryId: number;
  name: string;
  description?: string;
}

export interface CreateCategory {
  name: string;
  description: string;
}

export interface UpdateCategory {
  name: string;
  description: string;
}