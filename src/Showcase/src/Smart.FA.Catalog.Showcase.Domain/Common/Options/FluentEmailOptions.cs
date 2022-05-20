namespace Smart.FA.Catalog.Showcase.Domain.Common.Options;

public class FluentEmailOptions
{
    public const string SectionName = "Mailing:FluentEmail";

    public string Server { get; set; } = null!;

    public int Port { get; set; }

    public string User { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool UseSsl { get; set; }

    public bool RequiresAuthentication { get; set; }

    public string DefaultSender { get; set; } = null!;
}

public class InquiryOptions
{
    public const string SectionName = "Mailing:Inquiry";

    public string DefaultEmail { get; set; } = null!;

    public int RateLimitInSeconds { get; set; }
}
