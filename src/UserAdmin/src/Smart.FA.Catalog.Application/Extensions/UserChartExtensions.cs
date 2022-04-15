using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Factories;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Application.Extensions;

public static class UserChartExtensions
{
    public static string GenerateUserChartName(this UserChart? userChart)
    {
        Guard.AgainstNull(userChart, nameof(userChart));
        return $"userchart/userchart-{userChart.Version}_{userChart.ValidityDate:d}.pdf";
    }

    public static string GenerateUserChartNameDefault() =>
        UserChartFactory.CreateDefault().GenerateUserChartName();
}
