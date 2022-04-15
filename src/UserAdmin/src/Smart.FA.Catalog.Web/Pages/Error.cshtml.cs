using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    private readonly ILogger<ErrorModel> _logger;
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    private string? ExceptionMessage { get; set; }

    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var exceptionHandlerPathFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is DomainException domainException)
        {
            ExceptionMessage = GetStructuredErrorMessage(domainException.ToString(), RequestId, exceptionHandlerPathFeature.Path );
        }
        ExceptionMessage ??=
            GetStructuredErrorMessage("unknown", RequestId,  exceptionHandlerPathFeature!.Path, exceptionHandlerPathFeature.Error!.Message);

        _logger.LogError("{Message}", ExceptionMessage);
    }

    private string GetStructuredErrorMessage(string code, string requestId, string pagePath, string? message = null)
        => $"request {requestId}-code {code}. On page {pagePath} {(message is null? string.Empty: ": " + message)}";
}
