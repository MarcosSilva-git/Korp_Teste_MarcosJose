using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InvoiceService.Shared.DTOs.GetInvoices;

public record GetInvoicesQuery : IRequest<Result<List<GetInvoiceResponse>, ValidationResult>>
{
    public string? Ids { get; set; }
}
