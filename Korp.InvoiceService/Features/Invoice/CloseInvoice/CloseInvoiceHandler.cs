using Korp.InvoiceService.Features.Invoice.Domain.Enums;
using Korp.InvoiceService.Features.Invoice.Domain.Exceptions;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Shared.DTOs.Invoice.CloseInvoice;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InvoiceService.Features.Invoice.CloseInvoice;

public class CloseInvoiceHandler(InvoiceDbContext _invoiceDbContext) 
    : IRequestHandlerAsync<CloseInvoiceRequest, Result<CloseInvoiceResponse, ValidationResult>>
{
    public async Task<Result<CloseInvoiceResponse, ValidationResult>> HandleAsync(CloseInvoiceRequest request, CancellationToken ct)
    {
        var invoice = await _invoiceDbContext.Invoices.FindAsync([request.Id], ct);

        if (invoice is null)
            return new ValidationResult($"Invoice with id {request.Id} not found", [$"InvoiceId_{request.Id}"]);

        if (invoice.InvoiceStatus == InvoiceStatusEnum.Cancelled)
            return new ValidationResult($"Invoice with id {request.Id} is cancelled and cannot be closed", [$"InvoiceId_{request.Id}"]);
    
        if (invoice.InvoiceStatus == InvoiceStatusEnum.Closed)
            return new ValidationResult($"Invoice with id {request.Id} is already closed", [$"InvoiceId_{request.Id}"]);

        if (invoice.InvoiceStatus == InvoiceStatusEnum.Processing)
            return new ValidationResult($"Invoice with id {request.Id} is still processing and cannot be closed", [$"InvoiceId_{request.Id}"]);

        if (invoice.InvoiceStatus != InvoiceStatusEnum.Open)
            throw new InvalidInvoiceStatusException("Unable to close invoice with invalid status:", invoice.InvoiceStatus);

        invoice.MarkAsClosed();
        await _invoiceDbContext.SaveChangesAsync(ct);
        
        return new CloseInvoiceResponse(invoice.Id);
    }
}
