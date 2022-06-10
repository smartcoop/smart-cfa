using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Exceptions;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Shared.Collections;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;

namespace Smart.FA.Catalog.Core.Domain;

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

    public Trainer(Name name, TrainerIdentity identity, string title, string biography, Language defaultLanguage, string email)
    {
        Identity = identity;
        ChangeDefaultLanguage(defaultLanguage);
        Rename(name);
        UpdateBiography(biography);
        UpdateTitle(title);
        ChangeEmail(email);
    }

    protected Trainer()
    {
    }

    #endregion

    #region Methods

    /// <summary>
    /// Update the biography of the <see cref="Trainer"/>
    /// </summary>
    // /// <param name="newBiography">The new biography</param>
    public void UpdateBiography(string newBiography)
    {
        Guard.AgainstNull(newBiography, nameof(newBiography));
        Guard.Requires(() => newBiography.Length <= 500, Errors.Trainer.BiographyIsTooLong(Id, newBiography.Length, 500).Message);

        Biography = newBiography;
    }

    /// <summary>
    /// Update the job title of the <see cref="Trainer"/>
    /// </summary>
    /// <param name="newTitle">The new job title</param>
    public void UpdateTitle(string newTitle)
    {
        Guard.AgainstNull(newTitle, nameof(newTitle));
        Guard.Requires(() => newTitle.Length <= 150, Errors.Trainer.TitleIsTooLong(Id, newTitle.Length, 150).Message);
        Title = newTitle;
    }

    /// <summary>
    /// Update the relative path of the profile image of the <see cref="Trainer"/>
    /// </summary>
    /// <param name="profileImagePath">The new relative path of the profile image</param>
    public void UpdateProfileImagePath(string profileImagePath)
    {
        Guard.AgainstNull(profileImagePath, nameof(profileImagePath));
        Guard.Requires(() => profileImagePath.Length <= 50, Errors.Trainer.ProfileImage.UrlTooLong(Id, profileImagePath.Length, 50).Message);
        ProfileImagePath = profileImagePath;
    }

    /// <summary>
    /// Assign a <see cref="Trainer"/> to a <see cref="Training"/>
    /// </summary>
    /// <param name="training">The <see cref="Training"/> to assign a <see cref="Trainer"/> to</param>
    public void AssignTo(Training training)
    {
        Guard.Requires(() => !_assignments.Select(assignment => assignment.Training).Contains(training), Errors.Trainer.TrainerAlreadyAssignedToTraining(Id, training.Id).Message);
        _assignments.Add(new TrainerAssignment(training, this));
    }

    /// <summary>
    /// UnAssign a <see cref="Trainer"/>  from a specific  <see cref="Training"/>
    /// </summary>
    /// <param name="training">The <see cref="Training"/> to unAssign a <see cref="Trainer"/> from</param>
    public void UnAssignFrom(Training training)
    {
        var trainingNumberRemoved = _assignments.RemoveAll(assignment => assignment.Training == training);
        Guard.Ensures(() => trainingNumberRemoved != 0, Errors.Trainer.TrainerNeverAssignedToTraining(Id, training.Id).Message);
    }

    /// <summary>
    /// Rename the <see cref="Trainer"/> to the new <see cref="Name"/>
    /// </summary>
    /// <param name="name">The new name</param>
    public void Rename(Name name) => Name = name;

    /// <summary>
    /// Update the default language of the <see cref="Trainer"/>
    /// </summary>
    /// <param name="language">The new default language of the trainer</param>
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
