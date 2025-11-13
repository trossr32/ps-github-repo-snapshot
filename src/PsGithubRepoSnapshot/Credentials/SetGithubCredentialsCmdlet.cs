using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsGithubRepoSnapshot.Core.Components;
using PsGithubRepoSnapshot.Core.Enums;
using PsGithubRepoSnapshot.Core.Models.Github;
using PsGithubRepoSnapshot.Core.Services;

namespace GithubRepoSnapshot.Credentials;

/// <summary>
/// <para type="synopsis">
/// Register Github credentials for the session. Required before running any Github cmdlets.
/// </para>
/// <para type="description">
/// Register Github credentials for the session. Required before running any Github cmdlets.
/// </para>
/// <para type="description">
/// Optionally store credentials permanently in an encrypted locally stored file to remove the need for credentials
/// to be set in each session by supplying the StorePermanent switch parameter.
/// </para>
/// <example>
///     <para>Example 1: Register credentials in session</para>
///     <code>PS C:\> Set-GithubCredentials -AccountName "Hello" -ApiKey "W0rLd!" -AccountType User</code>
///     <remarks>Credentials will be registered for the lifetime of the session.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Register credentials in session and store permanently</para>
///     <code>PS C:\> Set-GithubCredentials -N "Hello" -K "W0rLd!" -T User -StorePermanent</code>
///     <remarks>Credentials will be registered for the lifetime of the session and stored locally so that this command will not need to be run again.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-github-repo-snapshot)">[Github]</para>
/// </summary>
[Cmdlet(VerbsCommon.Set, "GithubCredentials", HelpUri = "https://github.com/trossr32/ps-github-repo-snapshot")]
public class SetGithubCredentialsCmdlet : Cmdlet
{
    /// <summary>
    /// <para type="description">
    /// Your Github user or organisation name
    /// </para>
    /// </summary>
    [Parameter(Mandatory = true, Position = 0)]
    [Alias("N")]
    public string AccountName { get; set; }

    /// <summary>
    /// <para type="description">
    /// Your Github personal token API key
    /// </para>
    /// </summary>
    [Parameter(Mandatory = true, Position = 1)]
    [Alias("K")]
    public string ApiKey { get; set; }

    /// <summary>
    /// <para type="description">
    /// Whether the account is a user or an organisation
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false, Position = 2)]
    [Alias("T")]
    public AccountType AccountType { get; set; } = AccountType.User;

    /// <summary>
    /// <para type="description">
    /// If supplied credentials will be stored permanently in an encrypted local file that removes the need for credentials to be set in each session.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    [Alias("S")]
    public SwitchParameter StorePermanent { get; set; }

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="SetGithubCredentialsCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
            
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="SetGithubCredentialsCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
        try
        {
            var config = new GithubCredentials
            {
                AccountType = AccountType,
                AccountName = AccountName,
                ApiKey = ApiKey
            };

            GithubContext.Credentials = config;

            if (StorePermanent.IsPresent)
                Task.Run(async () => await AuthService.SetConfig(config));
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to set credentials with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="SetGithubCredentialsCmdlet"/>.
    /// </summary>
    protected override void EndProcessing()
    {
            
    }
}