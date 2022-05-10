using Microsoft.AspNetCore.Mvc.Rendering;

namespace Smart.FA.Catalog.AccountSimulator;

public static class AccountUsers
{
    public static List<SelectListItem> SelectListItems => new() { new SelectListItem { Value = "1", Text = "Victor" }, new SelectListItem { Value = "2", Text = "Maxime" } };
}
