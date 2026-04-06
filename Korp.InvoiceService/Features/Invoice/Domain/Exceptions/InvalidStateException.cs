using Korp.InvoiceService.Features.Invoice.Domain.Enums;

namespace Korp.InvoiceService.Features.Invoice.Domain.Exceptions;

public class InvalidInvoiceStatusException : Exception
{
    public InvoiceStatusEnum CurrentStatus { get; }

    public InvalidInvoiceStatusException(string message, InvoiceStatusEnum currentStatus) 
        : base(message + $"Current statte: [{currentStatus}] ")
    {
        CurrentStatus = currentStatus;
    }
}
