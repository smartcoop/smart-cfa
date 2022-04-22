using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Domain;

public class TrainingLocalizedDetails
{
    #region Private fields

    private string _title = null!;
    private string? _goal;
    private string? _methodology;
    private string? _practicalModalities;

    #endregion

    #region Properties

    public int TrainingId { get; }
    public virtual Training Training { get; } = null!;

    public string Title
    {
        get => _title;
        set
        {
            Guard.AgainstNull(value, nameof(Title));
            _title = Guard.AgainstMaxLength(value, nameof(Title), 500)!;
        }
    }

    public string? Goal
    {
        get => _goal;
        set => _goal = Guard.AgainstMaxLength(value, nameof(Goal), 1000);
    }

    public string? Methodology
    {
        get => _methodology;
        set => _methodology = Guard.AgainstMaxLength(value, nameof(Methodology), 1000);
    }

    public string? PracticalModalities
    {
        get => _practicalModalities;
        set => _practicalModalities = Guard.AgainstMaxLength(value, nameof(PracticalModalities), 1000);
    }

    public Language Language { get; } = null!;

    #endregion

    #region Constructors

    internal TrainingLocalizedDetails(Training training, string title, string? goal, string? methodology, string? practicalModalities, Language language)
    {
        Training = training;
        TrainingId = training.Id;
        Title = title;
        Goal = goal;
        Methodology = methodology;
        Language = language;
        PracticalModalities = practicalModalities;
    }

    protected TrainingLocalizedDetails()
    {
    }

    #endregion

    #region Methods

    public void UpdateDescription(string title, string? goal, string? methodology, string? practicalModalities)
    {
        Title = title;
        Goal = goal;
        Methodology = methodology;
        PracticalModalities = practicalModalities;
    }

    #endregion
}
