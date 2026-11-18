import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CategoryService } from '../../core/services/category';
import { Category, CreateCategory, UpdateCategory } from '../../models/category.model';
@Component({
  selector: 'app-categories',
  imports: [FormsModule],
  templateUrl: './categories.html',
  styleUrl: './categories.css'
})
export class Categories implements OnInit {

  categories: Category[] = [];

  name = '';
  description = '';

  isEditing = false;
  selectedCategoryId = 0;

  isLoading = false;
  isSaving = false;

  errorMessage = '';
  successMessage = '';

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.categoryService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'Unable to load categories.';
        this.isLoading = false;
      }
    });
  }

  saveCategory(): void {
    const trimmedName = this.name.trim();

    if (!trimmedName) {
      this.errorMessage = 'Category name is required.';
      return;
    }

    this.isSaving = true;
    this.errorMessage = '';
    this.successMessage = '';

    if (this.isEditing) {
      const category: UpdateCategory = {
        name: trimmedName,
        description: this.description.trim()
      };

      this.categoryService
        .updateCategory(this.selectedCategoryId, category)
        .subscribe({
          next: () => {
            this.successMessage = 'Category updated successfully.';
            this.resetForm();
            this.loadCategories();
            this.isSaving = false;
          },
          error: (error) => {
            this.errorMessage =
              error.error?.message || 'Unable to update category.';
            this.isSaving = false;
          }
        });
    } else {
      const category: CreateCategory = {
        name: trimmedName,
        description: this.description.trim()
      };

      this.categoryService.createCategory(category).subscribe({
        next: () => {
          this.successMessage = 'Category added successfully.';
          this.resetForm();
          this.loadCategories();
          this.isSaving = false;
        },
        error: (error) => {
          this.errorMessage =
            error.error?.message || 'Unable to add category.';
          this.isSaving = false;
        }
      });
    }
  }

  editCategory(category: Category): void {
    this.isEditing = true;
    this.selectedCategoryId = category.categoryId;
    this.name = category.name;
    this.description = category.description || '';

    this.errorMessage = '';
    this.successMessage = '';
  }

  deleteCategory(id: number): void {
    const confirmed = window.confirm(
      'Are you sure you want to delete this category?'
    );

    if (!confirmed) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.categoryService.deleteCategory(id).subscribe({
      next: () => {
        this.successMessage = 'Category deleted successfully.';
        this.loadCategories();
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'Unable to delete category.';
      }
    });
  }

  resetForm(): void {
    this.name = '';
    this.description = '';
    this.isEditing = false;
    this.selectedCategoryId = 0;
  }
}