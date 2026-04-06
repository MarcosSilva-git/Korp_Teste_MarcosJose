using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InvoiceService.Shared.DTOs.Invoice.CloseInvoice;

public record CloseInvoiceRequest(int Id) : IRequest<Result<CloseInvoiceResponse, ValidationResult>>;
