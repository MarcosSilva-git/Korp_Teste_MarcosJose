import { Component, inject, OnInit } from "@angular/core";
import { Invoice, InvoiceStatus } from "./invoice.model";
import { InvoiceService } from "./invoice.service";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { CommonModule } from "@angular/common";
import { MatTableModule } from '@angular/material/table'
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialog } from "@angular/material/dialog";

@Component({
  selector: 'app-product-page',
  imports: [CommonModule, MatTableModule, MatIconModule, MatPaginatorModule, MatButtonModule],
  standalone: true,
  templateUrl: './invoice.page.html',
  styleUrl: './invoice.page.css'
})
export class InvoicePage implements OnInit {
    invoices?: Invoice[]
    displayedColumns: string[] = ['id', 'status', 'quantityItems', 'createdAt', 'actions']

    constructor(private _invoiceService: InvoiceService) { }

    ngOnInit(): void {
        this.getInvoices()
    }

    getInvoices() {
        this._invoiceService.getAll()
            .subscribe({
                next: res => this.invoices = res.data
            })
    }

    // print(invoice: Invoice) {
    //     this._invoiceService.print(invoice)
    //         .subscribe({
    //             next: res => this.invoices = this.invoices!.filter(i => i.id != res.id )
    //         })
    // }

    openDialog(invoice?: Invoice) {
        
    }

    showStatus(status: InvoiceStatus): string {
        switch (status) {
            case 'Open': return "Aberto"
            case 'Closed': return 'Fechado'
            case 'Cancelled': return 'Cancelado'
            case 'Processing': return 'Processando'
            default: return ""
        }
    }
}