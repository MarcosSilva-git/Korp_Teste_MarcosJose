import { Component, inject, OnInit } from "@angular/core";
import { Product } from "./product.model";
import { ProductService } from "./product.service";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { CommonModule } from "@angular/common";
import { MatTableModule } from '@angular/material/table'
import { MatPaginatorModule } from '@angular/material/paginator';
import { ProductCreateUpdateDialogComponent, ProductCreateUpdateDialogData } from "./components/product-create-update/product-create-update.dialog";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
    selector: 'app-product-page',
    imports: [CommonModule, MatTableModule, MatIconModule, MatPaginatorModule, MatButtonModule],
    standalone: true,
    templateUrl: './product.page.html',
    styleUrl: './product.page.css'
})
export class ProductPage implements OnInit {
    products?: Product[]
    displayedColumns: string[] = ['id', 'name', 'stock', 'reserved', 'available', 'createdAt', 'actions']

    readonly dialog = inject(MatDialog);
    private _snackBar = inject(MatSnackBar);

    constructor(private _productService: ProductService) { }

    ngOnInit(): void {
        this.getProducts()
    }

    getProducts(): void {
        this._productService
            .getAll()
            .subscribe({
                next: res => this.products = res.data,
                error: () => alert('Erro ao carregar produtos:')
            })
    }

    deleteProduct(product: Product): void {
        this._productService.delete(product.id)
            .subscribe({ 
                next: () => {
                    this._snackBar.open("Produto deletado com sucesso")
                    this.getProducts()
                }
            })
    }

    openDialog(product?: Product): void {
        const dialogRef = this.dialog.open<ProductCreateUpdateDialogComponent, ProductCreateUpdateDialogData>(
            ProductCreateUpdateDialogComponent, {
            data: {
                product: product,
                title: product ? 'Editar Produto' : 'Criar Produto' 
            },
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.getProducts()
            }
        });
    }
}