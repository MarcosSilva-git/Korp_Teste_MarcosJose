using Korp.Shared.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Korp.Shared;

public abstract class SharedControllerBase : ControllerBase
{
    protected ActionResult CreateValidationProblemFromResult<T>(Result<T, ValidationResult> result)
    {
        var memberName = result.Error!.MemberNames.FirstOrDefault() ?? string.Empty;
        ModelState.AddModelError(memberName, result.Error.ErrorMessage ?? string.Empty);

        return ValidationProblem(ModelState);
    }

    protected ActionResult CreateValidationsProblemFromResult<T>(Result<T, List<ValidationResult>> result)
    {
        foreach (var error in result.Error!)
        {
            var memberName = error.MemberNames.FirstOrDefault() ?? string.Empty;
            ModelState.AddModelError(memberName, error.ErrorMessage ?? string.Empty);
        }

        return ValidationProblem(ModelState);
    }
}
