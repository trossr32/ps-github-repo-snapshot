using System;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using GithubRepoSnapshot.Base;
using PsGithubRepoSnapshot.Core.Enums;
using PsGithubRepoSnapshot.Core.Models;
using PsGithubRepoSnapshot.Core.Models.Github;
using PsGithubRepoSnapshot.Core.Services;

namespace GithubRepoSnapshot.Repositories
{
    /// <summary>
    /// <para type="synopsis">
    /// Create a snapshot directory containing Github repositories.
    /// </para>
    /// <para type="description">
    /// Create a snapshot directory containing Github repositories.
    /// Optionally filter repositories by providing a filter type, filter search type and filter text parameters, and specify which branch to retrieve.
    /// </para>
    /// <example>
    ///     <para>Example 1: Download and extract all repositories at their default branch to C:/temp</para>
    ///     <code>PS C:\> New-GithubRepoSnapshot -ExtractArchive</code>
    ///     <remarks>Download and extract all repositories at their default branch to C:/temp.</remarks>
    /// </example>
    /// <example>
    ///     <para>Example 2: Download and extract repositories where the name does not contain 'foobar' at their 'staging' branch to D:/foobar</para>
    ///     <code>PS C:\> New-GithubRepoSnapshot -Path 'D:/foobar' -Branch 'staging' -ExtractArchive -FilterType Negative -FilterSearchType Contains -FilterText 'foobar'</code>
    ///     <remarks>Download and extract repositories where the name does not contain 'foobar' at their 'staging' branch to D:/foobar.</remarks>
    /// </example>
    /// <para type="link" uri="(https://github.com/trossr32/ps-github-repo-snapshot)">[Github]</para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "GithubRepoSnapshot", HelpUri = "https://github.com/trossr32/ps-github-repo-snapshot")]
    [OutputType(typeof(GithubRepo[]))]
    public class NewGithubRepoSnapshotCmdlet : BaseGithubRepoSnapshotCmdlet
    {
        /// <summary>
        /// <para type="description">
        /// The path the downloaded repositories will be saved to.
        /// Within this path a new containing directory with a datetime stamped name will be created for each run.
        /// Defaults to C:\temp
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Path { get; set; } = @"C:\temp";

        /// <summary>
        /// <para type="description">
        /// By default, whatever the default branch is configured in Github for a given repository will be used.
        /// If supplied, this overrides that default.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string Branch { get; set; }

        /// <summary>
        /// <para type="description">
        /// By default, branches are downloaded as zip archives. If this parameter is supplied the archives will also be extracted.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter ExtractArchive { get; set; }

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

                var outputDir = global::System.IO.Path.Join(Path, $"github.repo.snapshot.{DateTime.Now:yyyyMMdd_HHmmss}");

                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                var repos = Task.Run(async () => await githubSvc.GetRepositories(filter)).Result;

                foreach (var repo in repos)
                {
                    WriteVerbose($"Downloading {(ExtractArchive.IsPresent ? "and extracting" : "")} {repo.Name} to {outputDir}");

                    if (string.IsNullOrWhiteSpace(Branch))
                        Branch = repo.DefaultBranch;

                    Task.Run(async () => await githubSvc.DownloadAndExtractRepository(repo, outputDir, Branch, ExtractArchive.IsPresent));
                }
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to download repositories with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
            }
        }
    }
}
