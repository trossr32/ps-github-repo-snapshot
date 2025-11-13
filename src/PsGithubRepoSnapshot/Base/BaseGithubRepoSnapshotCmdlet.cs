using System;
using System.Management.Automation;
using System.Threading.Tasks;
using PsGithubRepoSnapshot.Core.Components;
using PsGithubRepoSnapshot.Core.Services;

namespace GithubRepoSnapshot.Base;

public abstract class BaseGithubRepoSnapshotCmdlet : Cmdlet
{
    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="BaseGithubRepoSnapshotCmdlet"/>.
    /// Attempt to validate credentials are available in the session or stored permanently.
    /// </summary>
    protected override void BeginProcessing()
    {
        if (GithubContext.HasCredentials)
            return;

        GithubContext.Credentials = Task.Run(async () => await AuthService.GetConfig()).Result;

        if (GithubContext.HasCredentials)
            return;

        const string error = "No credentials set, Set-GithubCredentials must be run to save credentials before running any cmdlets.";

        ThrowTerminatingError(new ErrorRecord(new Exception(error), null, ErrorCategory.AuthenticationError, null)
        {
            CategoryInfo = {Reason = error},
            ErrorDetails = new ErrorDetails(error)
        });
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="BaseGithubRepoSnapshotCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {
            
    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="BaseGithubRepoSnapshotCmdlet"/>.
    /// </summary>
    protected override void EndProcessing()
    {
            
    }
}