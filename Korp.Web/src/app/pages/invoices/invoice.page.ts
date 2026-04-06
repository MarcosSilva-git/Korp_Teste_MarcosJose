import { Component, inject, OnInit } from "@angular/core";
import { Invoice, InvoiceStatus } from "./invoice.model";
import { InvoiceService } from "./invoice.service";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { CommonModule } from "@angular/common";
import { MatTableModule } from '@angular/material/table'
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialog } from "@angular/material/dialog";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { InvoiceCreateUpdateDialogComponent, InvoiceCreateUpdateDialogData } from "./components/invoice-create-update/invoice-create-update.dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
  selector: 'app-product-page',
  imports: [CommonModule, MatTableModule, MatIconModule, MatPaginatorModule, MatButtonModule, MatProgressSpinnerModule],
  standalone: true,
  templateUrl: './invoice.page.html',
  styleUrl: './invoice.page.css'
})
export class InvoicePage implements OnInit {
    invoices?: Invoice[]
    displayedColumns: string[] = ['id', 'status', 'quantityItems', 'createdAt', 'actions']

    printingInvoices: Invoice[] = []
    private _snackBar = inject(MatSnackBar);
    readonly dialog = inject(MatDialog);

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

    print(invoice: Invoice) {
        this.printingInvoices.push(invoice)

        this._invoiceService.print(invoice)
            .subscribe({
                next: res => {
                    this.invoices = this.invoices!.filter(i => i.id != res.id )
                    this.printingInvoices = this.printingInvoices!.filter(i => i.id != res.id )

                    this._snackBar.open("Nota imprimida")
                    this.getInvoices()
                },
                error: error => alert(error)
            })
    }

    openDialog(invoice?: Invoice) {
        const dialogRef = this.dialog.open<InvoiceCreateUpdateDialogComponent, InvoiceCreateUpdateDialogData>(
            InvoiceCreateUpdateDialogComponent, {
            data: {
                invoice: invoice,
                title: invoice ? 'Editar Nota' : 'Criar Nota' 
            },
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.getInvoices()
            }
        });
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