using System.Linq.Expressions;
using Core.SeedWork;

namespace Core.Domain;

public class TrainingDetail
{
    #region Private fields

    private string _title;
    private string? _goal;
    private string? _methodology;

    #endregion

    #region Properties

    public int TrainingId { get; }
    public virtual Training Training { get; }

    public string? Title
    {
        get => _title;
        set
        {
            Guard.AgainstNull(value, nameof(Title));
            ValidateMaxLength(value, nameof(Methodology), 500);
            _title = value;
        }
    }

    public string? Goal
    {
        get => _goal;
        set
        {
            ValidateMaxLength(value, nameof(Methodology), 1500);
            _goal = value;
        }
    }

    public string? Methodology
    {
        get => _methodology;
        set
        {
            ValidateMaxLength(value, nameof(Methodology), 1500);
            _methodology = value;
        }
    }

    public virtual Language Language { get; }

    #endregion

    #region Constructors

    internal TrainingDetail(Training training, string title, string? goal, string? methodology, Language language)
    {
        Training = training;
        TrainingId = training.Id;
        Title = title;
        Goal = goal;
        Methodology = methodology;
        Language = language;
    }

    protected TrainingDetail()
    {
    }

    #endregion

    #region Methods

    public void UpdateDescription(string title, string goal, string methodology)
    {
        Title = title;
        Goal = goal;
        Methodology = Methodology;
    }

    #endregion

    #region Validation

    public List<string> Validate()
    {
        List<string> errors = new();
        if (string.IsNullOrEmpty(Title)) errors.Add($"Missing {nameof(Title)} Details for language {Language}");
        if (string.IsNullOrEmpty(Goal)) errors.Add($"Missing {nameof(Goal)} Details for language {Language}");
        if (string.IsNullOrEmpty(Methodology))
            errors.Add($"Missing {nameof(Methodology)} Details for language {Language}");
        return errors;
    }

    private void ValidateMaxLength(string? field, string? name, int maxValue) =>
        LengthMaxValidation.Compile()(field, name, maxValue);

    private static readonly Expression<Action<string?, string?, int>> LengthMaxValidation =
        (fieldValue, fieldName, maxValue) =>
            Guard.Requires(() =>
                    string.IsNullOrEmpty(fieldValue) || fieldValue.Length < 1500,
                $"{nameof(fieldName)} has a maximum value of 1500 characters");

    private static readonly Expression<Func<bool>> Test = () => true;


    #endregion
}
