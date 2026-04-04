using Korp.InvoiceService.Features.Invoice.Domain.Enums;

namespace Korp.InvoiceService.Features.Invoice.Domain.Exceptions;

public class InvalidInvoiceStateException : Exception
{
    public InvoiceStatusEnum CurrentStatus { get; }

    public InvalidInvoiceStateException(string message, InvoiceStatusEnum currentStatus) 
        : base(message + $"Current statte: [{currentStatus}] ")
    {
        CurrentStatus = currentStatus;
    }
}
