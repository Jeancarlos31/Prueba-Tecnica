import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../core/models/product.model';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-detail.component.html'
})
export class ProductDetailComponent implements OnInit {
  product: Product | null = null;
  loading = true;
  notFound = false;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.notFound = true;
      this.loading = false;
      return;
    }

    this.productService.getById(id).subscribe({
      next: (data) => {
        this.product = data;
        this.loading = false;
      },
      error: () => {
        this.notFound = true;
        this.loading = false;
      }
    });
  }
}
