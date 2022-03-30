using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Shared.Domain.Enumerations;

namespace Smart.FA.Catalog.Web.Pages.Admin;

public class SideMenuItem : Enumeration
{
    public string Href { get; }

    public static readonly SideMenuItem MyProfile   = new(CatalogResources.MyTrainerProfile, 1, "/admin/myprofile");
    public static readonly SideMenuItem MyTrainings = new(CatalogResources.MyTrainings, 2, "/admin/trainings/List");

    private SideMenuItem(string name, int value, string href) : base(value, name)
    {
        Href = href;
    }

    public string DisplayName()
    {
        return Id switch
        {
            1 => CatalogResources.MyTrainerProfile,
            2 => CatalogResources.MyTrainings,
            _ => throw new ArgumentOutOfRangeException($"Sidemenu enumeration `{Id}` is unknown")
        };
    }
}
