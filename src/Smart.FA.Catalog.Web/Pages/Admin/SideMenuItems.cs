using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Web.Pages.Admin;

public class SideMenuItem : Enumeration
{
    public string Href { get; }

    //TODO use translation key for value.
    public static readonly SideMenuItem MyProfile   = new("My trainer Profile", 1, "/admin/myprofile");
    public static readonly SideMenuItem MyTrainings = new("My trainings", 2, "/admin/trainings/List");

    private SideMenuItem(string name, int value, string href) : base(value, name)
    {
        Href = href;
    }
}
