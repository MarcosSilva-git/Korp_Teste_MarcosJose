using Korp.InvoiceService.Domain.Enums;

namespace Korp.InvoiceService.Domain.Exceptions;

public class InvalidInvoiceStateException : BusinessException
{
    public InvoiceStatusEnum CurrentStatus { get; }

    public InvalidInvoiceStateException(string message, InvoiceStatusEnum currentStatus) 
        : base(message + $"Current statte: [{currentStatus}] ")
    {
        CurrentStatus = currentStatus;
    }
}
