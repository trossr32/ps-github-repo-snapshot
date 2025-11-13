using PsGithubRepoSnapshot.Core.Models.Github;

namespace PsGithubRepoSnapshot.Core.Components;

/// <summary>
/// The static credentials used by the PsTransmissionManager module, set with the Set-GithubCredentials cmdlet.
/// </summary>
public static partial class GithubContext
{
    public static GithubCredentials Credentials { get; set; }

    /// <summary>
    /// Check if the credentials are valid (i.e. that there is a Host property)
    /// </summary>
    public static bool HasCredentials => !string.IsNullOrWhiteSpace(Credentials?.AccountName) && !string.IsNullOrWhiteSpace(Credentials?.ApiKey);

    /// <summary>
    /// Set credentials to null
    /// </summary>
    public static void Dispose()
    {
        Credentials = null;
    }
}