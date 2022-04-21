using System.Globalization;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Factories;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Extensions;

public static class UserChartRevisionExtensions
{
    public static string GenerateUserChartName(this UserChartRevision? userChart)
    {
        Guard.AgainstNull(userChart, nameof(userChart));
        return $"userchart/userchart-{userChart!.Version}_{userChart.ValidFrom.ToString("dd-MM-yyyy", new CultureInfo("fr-BE"))}.pdf";
    }

    public static string GenerateUserChartNameDefault() =>
        UserChartRevisionFactory.CreateDefault().GenerateUserChartName();
}
