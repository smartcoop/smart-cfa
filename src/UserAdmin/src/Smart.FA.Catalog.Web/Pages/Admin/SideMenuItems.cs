using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Web.Pages.Admin;

public class SideMenuItem : Enumeration<SideMenuItem>
{
    public string Href { get; }

    public static readonly SideMenuItem MyProfile   = new(CatalogResources.MyTrainerProfile, 1, "/cfa/admin/trainers/myprofile");
    public static readonly SideMenuItem MyTrainings = new(CatalogResources.MyTrainings, 2, "/cfa/admin/trainings/List");

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
