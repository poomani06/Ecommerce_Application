import { Routes } from '@angular/router';
import { Home } from './features/home/home';
import { Categories } from './features/categories/categories';
import { Products } from './features/products/products';
import { ProductDetails } from './features/product-details/product-details';

export const routes: Routes = [
  {
    path: '',
    component: Home
  },
  {
    path: 'categories',
    component: Categories
  },
  {
  path: 'products',
  component: Products
  },
  { path: 'products/:id', component: ProductDetails },
  {
    path: '**',
    redirectTo: ''
  }
];