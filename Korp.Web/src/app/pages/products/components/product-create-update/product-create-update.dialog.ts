import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CreateOrUpdateProduct, Product } from '../../product.model';
import { ProductService } from '../../product.service';
import { MatSnackBar } from '@angular/material/snack-bar';

export interface ProductCreateUpdateDialogData {
  product?: Product
  title: string
}

type ProductForm = {
  id: FormControl<number>
  name: FormControl<string | null>;
  stock: FormControl<number | null>;
};

@Component({
  selector: 'app-product-create-update-dialog',
  imports: [MatDialogModule, MatButtonModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule],
  standalone: true,
  templateUrl: './product-create-update.dialog.html',
  styleUrl: './product-create-update.dialog.css'
})
export class ProductCreateUpdateDialogComponent implements OnInit {
  _productService = inject(ProductService)
  dialogRef = inject(MatDialogRef<ProductCreateUpdateDialogComponent>);
  data: ProductCreateUpdateDialogData = inject(MAT_DIALOG_DATA);
  _snackBar = inject(MatSnackBar);
  
  productForm = new FormGroup<ProductForm>({
    id: new FormControl(),
    name: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]),
    stock: new FormControl(null, [Validators.required, Validators.min(1)])
  });

  onNoClick(): void {
    this.dialogRef.close();
  }

  ngOnInit(): void {
      this.productForm.patchValue(this.data.product ?? {})
  }

  getErrorMessage(field: string) {
    const control = this.productForm.get(field);
    
    if (control?.hasError('required')) {
      return 'Obrigatório';
    }
    if (control?.hasError('minlength')) {
      return ` Mínimo de 3 caracteres`;
    }
    if (control?.hasError('maxlength')) {
      return `Máximo de 100 caracteres`;
    }
    if (control?.hasError('min')) {
      return 'Deve ser adicionado no mínimo 1 produto no estoque';
    }
    return '';
  }

  createProduct(productForm: FormGroup<ProductForm>) {
      const product : CreateOrUpdateProduct = {
        id: this.productForm.value.id!,
        name: this.productForm.value.name!,
        stock: this.productForm.value.stock!, 
      }

      this._productService
        .create(product)
        .subscribe({
          next: () => {
            this._snackBar.open("Produto criado com sucesso")
            this.dialogRef.close(product);
          },
          error: () => {
            alert("Ocorreu um erro inesperado, tente novamente em alguns instantes")
          }
        })
  }

  updateProduct(productForm: FormGroup<ProductForm>) {
    const product : CreateOrUpdateProduct = {
      id: this.productForm.value.id!,
      name: this.productForm.value.name!,
      stock: this.productForm.value.stock!, 
    }
    
    this._productService
      .update(product)
      .subscribe({
        next: () => {
          this._snackBar.open("Produto atualizado com sucesso")
          this.dialogRef.close(product);
        },
        error: () => {
          alert("Ocorreu um erro inesperado, tente novamente em alguns instantes")
        }
      })
  }

  submit() {
    if (this.productForm.valid) {
      if (this.data.product) {
        this.updateProduct(this.productForm)
      } else {
          this.createProduct(this.productForm)
      } 
    }
  }
}