using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Factories;

namespace Smart.FA.Catalog.Application.Extensions;

public static class UserChartExtensions
{
    public static string GenerateUserChartName(this UserChart userChart) =>
        $"userchart/userchart-{userChart.Version}_{userChart.ValidityDate:d}.pdf";

    public static string GenerateUserChartNameDefault() =>
        UserChartFactory.CreateDefault().GenerateUserChartName();
}
