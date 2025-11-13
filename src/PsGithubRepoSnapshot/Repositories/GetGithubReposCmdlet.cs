using System;
using System.Management.Automation;
using System.Text.Json;
using System.Threading.Tasks;
using GithubRepoSnapshot.Base;
using PsGithubRepoSnapshot.Core.Enums;
using PsGithubRepoSnapshot.Core.Models;
using PsGithubRepoSnapshot.Core.Models.Github;
using PsGithubRepoSnapshot.Core.Services;

namespace GithubRepoSnapshot.Repositories;

/// <summary>
/// <para type="synopsis">
/// Get Github repos.
/// </para>
/// <para type="description">
/// Get Github repos. Optionally filter response by providing a filter type, filter search type and filter text parameters.
/// </para>
/// <para type="description">
/// Calls the get repos endpoint for a user or organisation:
/// https://docs.github.com/en/rest/reference/repos#list-repositories-for-a-user
/// https://docs.github.com/en/rest/reference/repos#list-organization-repositories
/// </para>
/// <example>
///     <para>Example 1: Get all repos</para>
///     <code>PS C:\> Get-GithubRepos</code>
///     <remarks>Retrieves all repos.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Get all repos and return as JSON</para>
///     <code>PS C:\> Get-GithubRepos -Json</code>
///     <remarks>Retrieves all repos and returns as JSON.</remarks>
/// </example>
/// <example>
///     <para>Example 3: Get repos where the name starts with foobar</para>
///     <code>PS C:\> Get-GithubRepos -FilterType Positive -FilterSearchType StartsWith -FilterText 'foobar'</code>
///     <remarks>Retrieves all repos where the name starts with foobar</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-github-repo-snapshot)">[Github]</para>
/// <para type="link" uri="(https://docs.github.com/en/rest/reference/repos#list-repositories-for-a-user)">[Github API docs]</para>
/// <para type="link" uri="(https://docs.github.com/en/rest/reference/repos#list-organization-repositories)">[Github API docs]</para>
/// </summary>
[Cmdlet(VerbsCommon.Get, "GithubRepos", HelpUri = "https://github.com/trossr32/ps-github-repo-snapshot")]
[OutputType(typeof(GithubRepo[]))]
public class GetGithubReposCmdlet : BaseGithubRepoSnapshotCmdlet
{
    /// <summary>
    /// <para type="description">
    /// Optionally filter returned repositories.
    /// None = no filter, Positive = all where repo name matches filter, Negative = all where repo name does not match filter
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public FilterType FilterType { get; set; } = FilterType.None;

    /// <summary>
    /// <para type="description">
    /// How to apply a filter type if supplied.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public FilterSearchType FilterSearchType { get; set; } = FilterSearchType.Contains;

    /// <summary>
    /// <para type="description">
    /// The text used to filter against repository names if a filter type is supplied.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public string FilterText { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied the data will be output as a JSON string.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter Json { get; set; }
        
    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="GetGithubReposCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="GetGithubReposCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
            
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="GetGithubReposCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
        try
        {
            var githubSvc = new GithubService();

            var filter = new FilterConfig
            {
                FilterType = FilterType,
                FilterSearchType = FilterSearchType,
                FilterText = FilterText
            };

            var repos = Task.Run(async () => await GithubService.GetRepositories(filter)).Result.ToArray();

            if (Json)
                WriteObject(JsonSerializer.Serialize(repos));
            else
                WriteObject(repos);
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to retrieve torrents with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}