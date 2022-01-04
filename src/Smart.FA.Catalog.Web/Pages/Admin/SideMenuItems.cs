using Core.SeedWork;

namespace Api.Pages.Admin;

public class SideMenuItem : Enumeration
{
    public string Href { get; }

    //TODO use translation key for value.
    public static readonly SideMenuItem MyProfile   = new("My trainer Profile", 1, "admin/training/list");
    public static readonly SideMenuItem MyTrainings = new("My trainings", 2, "/admin/training/List");

    private SideMenuItem(string name, int value, string href) : base(value, name)
    {
        Href = href;
    }
}
