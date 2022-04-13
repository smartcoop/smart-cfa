using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Factories;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Application.Extensions;

public static class UserChartExtensions
{
    public static string GenerateUserChartName(this UserChartRevision? userChart)
    {
        Guard.AgainstNull(userChart, nameof(userChart));
        return $"userchart/userchart-{userChart.Version}_{userChart.ValidFrom:d}.pdf";
    }

    public static string GenerateUserChartNameDefault() =>
        UserChartFactory.CreateDefault().GenerateUserChartName();
}
