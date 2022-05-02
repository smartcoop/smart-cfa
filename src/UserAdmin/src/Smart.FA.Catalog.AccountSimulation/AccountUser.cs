using Microsoft.AspNetCore.Mvc.Rendering;

namespace Smart.FA.Catalog.AccountSimulation;

public static class AccountUsers
{
    public static List<SelectListItem> List => new()
    {

        new SelectListItem {Value = "1", Text = "Victor"},
        new SelectListItem {Value = "2", Text = "Maxime"}

    };
}
