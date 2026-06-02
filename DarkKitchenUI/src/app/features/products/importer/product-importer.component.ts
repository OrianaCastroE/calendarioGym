import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ProductService } from '../../../core/services/product.service';
import { ImporterInfo } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-importer',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-importer.component.html',
  styleUrl: './product-importer.component.css'
})
export class ProductImporterComponent implements OnInit {
  private readonly auth = inject(AuthService);
  private readonly productService = inject(ProductService);
  private readonly router = inject(Router);

  importers: ImporterInfo[] = [];
  selectedImporter = '';
  selectedFile: File | null = null;
  isLoading = true;
  isUploading = false;
  errorMessage = '';
  successMessage = '';

  ngOnInit(): void {
    if (this.auth.isTokenExpired()) {
      this.auth.logout(true);
      return;
    }
    this.productService.getImporters().subscribe({
      next: list => {
        this.importers = list;
        if (list.length) this.selectedImporter = list[0].name;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Could not load available importers.';
        this.isLoading = false;
      }
    });
  }

  get expectedExtension(): string | null {
    return this.importers.find(i => i.name === this.selectedImporter)?.extension ?? null;
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files?.[0] ?? null;
    this.successMessage = '';
    this.errorMessage = '';
  }

  upload(): void {
    if (!this.selectedImporter || !this.selectedFile) {
      this.errorMessage = 'Please choose an importer and a file.';
      return;
    }

    this.isUploading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.productService.importProducts(this.selectedImporter, this.selectedFile).subscribe({
      next: res => {
        this.successMessage = res.message ?? `${res.imported} products imported.`;
        this.selectedFile = null;
        this.isUploading = false;
      },
      error: err => {
        this.errorMessage = err?.error?.message ?? err?.error ?? 'Import failed.';
        this.isUploading = false;
      }
    });
  }

  back(): void {
    this.router.navigate(['/products']);
  }
}
