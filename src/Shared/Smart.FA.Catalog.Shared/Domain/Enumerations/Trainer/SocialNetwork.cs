using Smart.FA.Catalog.Localization;

namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Trainer;

/// <summary>
/// Enumerates the list of Social Networks supported by the system.
/// </summary>
public class SocialNetwork : Enumeration
{
    public static readonly SocialNetwork Twitter         = new(1, nameof(Twitter));

    public static readonly SocialNetwork Instagram       = new(2, nameof(Instagram));

    public static readonly SocialNetwork Facebook        = new(3, nameof(Facebook));

    public static readonly SocialNetwork Github          = new(4, nameof(Github));

    public static readonly SocialNetwork LinkedIn        = new(5, nameof(LinkedIn));

    public static readonly SocialNetwork PersonalWebsite = new(6, "Personal website");

    protected SocialNetwork(int id, string name) : base(id, name)
    {

    }
}
