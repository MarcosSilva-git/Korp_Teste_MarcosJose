export type Invoice = {
    id: number
    status: InvoiceStatus
    createdAt: Date
    items: InvoiceItem[]
}

export type InvoiceItem = {
    id: number
    invoiceId: number
    itemSequence: number
    productId: number
    productName: string
    quantity: number
    createdAt: Date
}

export type InvoiceStatus = 'Open' | 'Processing' | 'Closed' | 'Cancelled';

export type CreateOrUpdateInvoice = {
    id?: number
    items: CreateOrUpdateInvoiceItem[]
}

export type CreateOrUpdateInvoiceItem = Pick<InvoiceItem, 'productId' | 'productName' | 'quantity'>