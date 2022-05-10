using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.Web.Pages.SuperUser;

public class SuperUserSideMenuItem : Enumeration<SuperUserSideMenuItem>
{
    public string Href { get; }

    public static readonly SuperUserSideMenuItem SuperUserTrainingList = new(1, CatalogResources.Trainings, Routes.SuperUserTrainingList);

    protected SuperUserSideMenuItem(int id, string name, string href) : base(id, name)
    {
        Href = href;
    }

    public string DisplayName()
    {
        return Id switch
        {
            1 => CatalogResources.Trainings,
            _ => throw new ArgumentOutOfRangeException($"Super admin sidemenu enumeration `{Id}` is unknown")
        };
    }
}
