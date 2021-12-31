namespace Core.Domain;

public class TrainingDetail
{

    #region Private fields

    private string _title;
    private string _goal;
    private string _methodology;
    private string _language;

    #endregion

    #region Properties

    public int TrainingId { get;  }
    public virtual Training Training { get; }
    public string Title
    {
        get => _title;
        set
        {
            if (value.Length > 150) throw new Exception($"{nameof(Title)} has a maximum value of 150 characters");
            _title = value;
        }
    }

    public string Goal
    {
        get => _goal;
        set
        {
            if (value.Length > 1500) throw new Exception($"{nameof(Goal)} has a maximum value of 1500 characters");
            _goal = value;
        }
    }

    public string Methodology
    {
        get => _methodology;
        set
        {
            if (value.Length > 1500) throw new Exception($"{nameof(Methodology)} has a maximum value of 1500 characters");
            _methodology = value;
        }
    }
    public string Language
    {
        get => _language;
        set
        {
            if (value.Length > 2) throw new Exception($"{nameof(Language)} has a maximum value of 2 characters");
            _language = value;
        }
    }

    #endregion

    #region Constructors

    internal TrainingDetail(Training training, string title, string goal, string methodology, string language)
    {
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
}
