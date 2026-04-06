import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators, FormArray } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { CreateOrUpdateInvoice, CreateOrUpdateInvoiceItem, Invoice, InvoiceItem } from '../../invoice.model';
import { InvoiceService } from '../../invoice.service';
import { Product } from '../../../products/product.model';
import { ProductService } from '../../../products/product.service';
import { MatSelectModule } from '@angular/material/select';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

export interface InvoiceCreateUpdateDialogData {
  invoice?: Invoice;
  title: string;
}

@Component({
  selector: 'app-invoice-create-update-dialog',
  standalone: true,
  imports: [
    CommonModule, 
    MatDialogModule, 
    MatButtonModule, 
    MatFormFieldModule, 
    MatInputModule, 
    ReactiveFormsModule, 
    MatIconModule,
    MatSelectModule,
    MatTableModule
  ],
  templateUrl: './invoice-create-update.dialog.html',
  styleUrl: './invoice-create-update.dialog.css'
})
export class InvoiceCreateUpdateDialogComponent implements OnInit {
    private readonly _invoiceService = inject(InvoiceService);
    private readonly _productService = inject(ProductService);
    readonly dialogRef = inject(MatDialogRef<InvoiceCreateUpdateDialogComponent>);
    readonly data: InvoiceCreateUpdateDialogData = inject(MAT_DIALOG_DATA);
    products: Product[] = []
    items: CreateOrUpdateInvoiceItem[] = []

    isAddingInvoice: boolean = false

    ngOnInit(): void {
        this._productService.getAll()
        .subscribe({ 
            next: res => this.products = res.data
        })

        if (this.data.invoice) {
            if (this.data.invoice.items) {

            }
        }
    }

    addItem(product: Product, quantity: string) {
        if (this.items.filter(i => i.productId == product.id).length == 0)
        {
            this.items.push({
                productId: product.id,
                productName: product.name,
                quantity: Number(quantity)
            })
        }
    }

    onNoClick() {
        this.dialogRef.close();
    }

    removeItem(item: CreateOrUpdateInvoiceItem) {
        this.items = this.items.filter(i => i.productId != i.productId)
    }

    submit() {
        var invoice: CreateOrUpdateInvoice = {
            items: this.items
        }

        this.isAddingInvoice = true

        this._invoiceService.create(invoice)
            .subscribe({
                next: res => {
                    alert(`Nota Adicionada`)
                    this.isAddingInvoice = false
                    this.dialogRef.close(true);
                },
                error: () => this.isAddingInvoice = false
            })
    }
}