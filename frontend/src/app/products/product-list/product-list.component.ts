import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../core/services/product.service';
import { AlertService } from '../../core/services/alert.service';
import { Product } from '../../core/models/product.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  loading = false;
  searchId: number | null = null;

  constructor(
    private productService: ProductService,
    private alertService: AlertService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.productService.getAll().subscribe({
      next: (data) => {
        this.products = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  deleteProduct(product: Product): void {
    const confirmado = confirm(`¿Está seguro de eliminar el producto "${product.name}"?`);
    if (!confirmado) return;

    this.productService.delete(product.id).subscribe({
      next: () => {
        this.products = this.products.filter((p) => p.id !== product.id);
        this.alertService.show('Producto eliminado correctamente.', 'success');
      }
    });
  }

  buscarPorId(): void {
    if (this.searchId === null || this.searchId <= 0) {
      this.alertService.show('Ingrese un Id válido para buscar.', 'warning');
      return;
    }
    this.router.navigate(['/products', this.searchId]);
  }
}
