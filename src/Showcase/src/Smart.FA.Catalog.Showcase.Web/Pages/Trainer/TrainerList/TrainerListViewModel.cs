namespace Smart.FA.Catalog.Showcase.Web.Pages.Trainer.TrainerList;

public class TrainerListViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? ProfileImagePath { get; set; }
}
