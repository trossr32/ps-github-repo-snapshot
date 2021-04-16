using System;
using System.Management.Automation;
using PsGithubRepoSnapshot.Core.Components;
using PsGithubRepoSnapshot.Core.Services;

namespace GithubRepoSnapshot.Credentials
{
    /// <summary>
    /// <para type="synopsis">
    /// Remove previously registered Github credentials.
    /// </para>
    /// <para type="description">
    /// Remove previously registered Github credentials for the session.
    /// </para>
    /// <para type="description">
    /// Optionally remove stored credentials from an encrypted local file by supplying the DeletePermanent switch parameter.
    /// </para>
    /// <example>
    ///     <para>Example 1: Remove previously registered credentials from session</para>
    ///     <code>PS C:\> Remove-GithubCredentials</code>
    ///     <remarks>Previously registered credentials will be removed.</remarks>
    /// </example>
    /// <example>
    ///     <para>Example 2: Remove previously registered credentials from session and those stored locally</para>
    ///     <code>PS C:\> Remove-GithubCredentials -DeletePermanent</code>
    ///     <remarks>Previously registered credentials will be removed from the session and from the encrypted local file.</remarks>
    /// </example>
    /// <para type="link" uri="(https://github.com/trossr32/ps-github-repo-snapshot)">[Github]</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "GithubCredentials", HelpUri = "https://github.com/trossr32/ps-github-repo-snapshot")]
    public class RemoveGithubCredentialsCmdlet : Cmdlet
    {
        /// <summary>
        /// <para type="description">
        /// If supplied any credentials stored permanently in an encrypted local file will be deleted.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [Alias("D")]
        public SwitchParameter DeletePermanent { get; set; }

        /// <summary>
        /// Implements the <see cref="BeginProcessing"/> method for <see cref="RemoveGithubCredentialsCmdlet"/>.
        /// </summary>
        protected override void BeginProcessing()
        {
            
        }

        /// <summary>
        /// Implements the <see cref="ProcessRecord"/> method for <see cref="RemoveGithubCredentialsCmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                GithubContext.Dispose();

                if (DeletePermanent.IsPresent)
                    AuthService.RemoveConfig();
            }
            catch (Exception e)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to remove credentials with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
            }
        }

        /// <summary>
        /// Implements the <see cref="EndProcessing"/> method for <see cref="RemoveGithubCredentialsCmdlet"/>.
        /// </summary>
        protected override void EndProcessing()
        {
            
        }
    }
}
