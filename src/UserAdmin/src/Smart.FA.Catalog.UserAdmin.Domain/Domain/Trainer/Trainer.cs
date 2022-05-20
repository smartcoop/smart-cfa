using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;
using Smart.FA.Catalog.UserAdmin.Domain.Domain.ValueObjects;
using Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

namespace Smart.FA.Catalog.UserAdmin.Domain.Domain;

public class Trainer : Entity, IAggregateRoot
{
    #region Private fields

    private readonly List<TrainerApproval> _approvals = new();
    private readonly List<TrainerAssignment> _assignments = new();
    private readonly List<TrainerSocialNetwork> _socialNetworks = new();

    #endregion

    #region Properties

    public virtual Name Name { get; private set; } = null!;

    public string Biography { get; private set; } = null!;

    public string Title { get; set; } = null!;

    public string? Email { get; private set; }

    public string? ProfileImagePath { get; set; } = null!;

    public Language DefaultLanguage { get; private set; } = null!;

    public TrainerIdentity Identity { get; } = null!;

    public virtual IReadOnlyCollection<TrainerApproval> Approvals => _approvals.AsReadOnly();

    public virtual IReadOnlyCollection<TrainerAssignment> Assignments => _assignments.AsReadOnly();

    public virtual IReadOnlyCollection<TrainerSocialNetwork> SocialNetworks => _socialNetworks.AsReadOnly();

    #endregion

    #region Constructors

    public Trainer(Name name, TrainerIdentity identity, string title, string biography, Language defaultLanguage)
    {
        Identity = identity;
        ChangeDefaultLanguage(defaultLanguage);
        Rename(name);
        UpdateBiography(biography);
        UpdateTitle(title);
    }

    protected Trainer()
    {
    }

    #endregion

    #region Methods

    public void UpdateBiography(string newBiography)
    {
        Guard.AgainstNull(newBiography, nameof(newBiography));
        Guard.Requires(() => newBiography.Length <= 500, "Biography is too long");

        Biography = newBiography;
    }

    public void UpdateTitle(string newTitle)
    {
        Guard.AgainstNull(newTitle, nameof(newTitle));
        Guard.Requires(() => newTitle.Length <= 150
            , "Title is too long");
        Title = newTitle;
    }

    public void UpdateProfileImagePath(string profileImagePath)
    {
        Guard.AgainstNull(profileImagePath, nameof(profileImagePath));
        Guard.Requires(() => profileImagePath.Length <= 50, "URL for profile path is too long");
        ProfileImagePath = profileImagePath;
    }

    public void AssignTo(Training training)
    {
        Guard.Requires(() => !_assignments.Select(assignment => assignment.Training).Contains(training),
            "The trainer is already assigned to that training");
        _assignments.Add(new TrainerAssignment(training, this));
    }

    public void UnAssignFrom(Training training)
    {
        var trainingNumberRemoved = _assignments.RemoveAll(assignment => assignment.Training == training);
        Guard.Ensures(() => trainingNumberRemoved != 0, "The trainer was never assigned to that training");
    }

    public List<Training> GetTrainings()
        => Assignments.Any() ? Assignments.Select(assignment => assignment.Training).ToList() : new List<Training>();

    public void Rename(Name name) => Name = name;

    public void ChangeDefaultLanguage(Language language) => DefaultLanguage = language;

    /// <summary>
    /// Adds or updates an <see cref="TrainerSocialNetwork"/> to/of the underlying <see cref="Trainer"/>.
    /// </summary>
    /// <param name="socialNetwork">The type of social network to be updated.</param>
    /// <param name="urlToProfile">The URL to profile of the trainer <paramref name="socialNetwork"/>.</param>
    public void SetSocialNetwork(SocialNetwork socialNetwork, string? urlToProfile)
    {
        var existing = _socialNetworks.FirstOrDefault(p => p.SocialNetwork == socialNetwork);

        if (existing is not null)
        {
            // There is no point to keep in the database a SocialNetwork if the url is empty.
            if (string.IsNullOrWhiteSpace(urlToProfile))
            {
                _socialNetworks.Remove(existing);
                return;
            }

            existing.SetSocialNetworkInfo(Id, socialNetwork, urlToProfile);
        }
        else
        {
            _socialNetworks.Add(new TrainerSocialNetwork(Id, socialNetwork, urlToProfile));
        }
    }

    /// <summary>
    /// Changes the <see cref="Trainer" />'s email address.
    /// </summary>
    /// <param name="email">The new email.</param>
    /// <exception cref="ArgumentNullException"><paramref name="email" /> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="email" />'s format is invalid.</exception>
    public void ChangeEmail(string? email)
    {
        Email = Guard.AgainstInvalidEmail(email, nameof(email));
    }

    /// <summary>
    /// Approve a specific user chart
    /// </summary>
    /// <param cref="ArgumentNullException" name="userChart"></param>
    public void ApproveUserChart(UserChartRevision? userChart)
    {
        Guard.AgainstNull(userChart, nameof(userChart));
        if (Approvals.Any(approval => approval.UserChartId == userChart!.Id))
        {
            return;
        }
        _approvals.Add(new TrainerApproval(this, userChart!));
    }

    #endregion

    public override string ToString()
    {
        return Name.ToString();
    }
}
