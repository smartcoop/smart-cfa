using System.Linq.Expressions;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Core.Domain;

public class TrainingDetail
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

    public string? PracticalModalities
    {
        get => _practicalModalities;
        set
        {
            ValidateMaxLength(value, nameof(PracticalModalities), 1500);
            _practicalModalities = value;
        }
    }

    public Language Language { get; } = null!;

    #endregion

    #region Constructors

    internal TrainingDetail(Training training, string title, string? goal, string? methodology, string? practicalModalities, Language language)
    {
        Training = training;
        TrainingId = training.Id;
        Title = title;
        Goal = goal;
        Methodology = methodology;
        Language = language;
        PracticalModalities = practicalModalities;
    }

    protected TrainingDetail()
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

    #region Validation

    private void ValidateMaxLength(string? field, string? name, int maxValue) =>
        LengthMaxValidation.Compile()(field, name, maxValue);

    private static readonly Expression<Action<string?, string?, int>> LengthMaxValidation =
        (fieldValue, fieldName, maxValue) =>
            Guard.Requires(() =>
                    string.IsNullOrEmpty(fieldValue) || fieldValue.Length < maxValue,
                $"{nameof(fieldName)} has a maximum value of {maxValue} characters");

    #endregion
}
