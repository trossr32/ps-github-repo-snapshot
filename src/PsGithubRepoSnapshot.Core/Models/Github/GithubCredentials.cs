using PsGithubRepoSnapshot.Core.Enums;

namespace PsGithubRepoSnapshot.Core.Models.Github
{
    public class GithubCredentials
    {
        public AccountType AccountType { get; set; } = AccountType.User;
        public string AccountName { get; set; }
        public string ApiKey { get; set; }
    }
}
