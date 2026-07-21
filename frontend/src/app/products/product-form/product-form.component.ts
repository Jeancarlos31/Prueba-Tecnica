import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ProductService } from '../../core/services/product.service';
import { AlertService } from '../../core/services/alert.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './product-form.component.html'
})
export class ProductFormComponent implements OnInit {
  form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    description: [''],
    price: [0, [Validators.required, Validators.min(0.01)]],
    stock: [0, [Validators.required, Validators.min(0)]]
  });

  isEditMode = false;
  productId: number | null = null;
  saving = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEditMode = true;
      this.productId = Number(idParam);

      this.productService.getById(this.productId).subscribe({
        next: (product) => {
          this.form.patchValue({
            name: product.name,
            description: product.description ?? '',
            price: product.price,
            stock: product.stock
          });
        }
      });
    }
  }

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    const payload = {
      name: value.name,
      description: value.description || undefined,
      price: value.price,
      stock: value.stock
    };

    this.saving = true;

    if (this.isEditMode && this.productId != null) {
      this.productService.update(this.productId, payload).subscribe({
        next: () => {
          this.saving = false;
          this.alertService.show('Producto actualizado correctamente.', 'success');
          this.router.navigate(['/products', this.productId]);
        },
        error: () => {
          this.saving = false;
        }
      });
    } else {
      this.productService.create(payload).subscribe({
        next: (created) => {
          this.saving = false;
          this.alertService.show('Producto creado correctamente.', 'success');
          this.router.navigate(['/products', created.id]);
        },
        error: () => {
          this.saving = false;
        }
      });
    }
  }
}
