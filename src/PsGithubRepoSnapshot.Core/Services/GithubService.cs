using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PsGithubRepoSnapshot.Core.Components;
using PsGithubRepoSnapshot.Core.Enums;
using PsGithubRepoSnapshot.Core.Helpers;
using PsGithubRepoSnapshot.Core.Models;
using PsGithubRepoSnapshot.Core.Models.Github;

namespace PsGithubRepoSnapshot.Core.Services
{
    public class GithubService
    {
        /// <summary>
        /// Get all repos for the Github user or organisation using the Github API
        /// </summary>
        /// <returns></returns>
        public async Task<List<GithubRepo>> GetRepositories(FilterConfig filter)
        {
            using var client = GetClient();

            var response = await client.GetAsync($"/{GithubContext.Credentials.AccountType.AsUrlPart()}/{GithubContext.Credentials.AccountName}/repos");

            if (!response.IsSuccessStatusCode)
                return default;

            var content = await response.Content.ReadAsStringAsync();

            var repos = JsonSerializer.Deserialize<List<GithubRepo>>(content);

            if (repos == null || !repos.Any())
                return default;

            if (string.IsNullOrEmpty(filter.FilterText))
                return repos;

            return filter.FilterType switch
            {
                FilterType.None => repos,

                FilterType.Positive => filter.FilterSearchType switch
                    {
                        FilterSearchType.Contains => repos.Where(r => r.Name.Contains(filter.FilterText, StringComparison.OrdinalIgnoreCase)).ToList(),
                        FilterSearchType.StartsWith => repos.Where(r => r.Name.StartsWith(filter.FilterText, StringComparison.OrdinalIgnoreCase)).ToList(),
                        FilterSearchType.EndsWith => repos.Where(r => r.Name.EndsWith(filter.FilterText, StringComparison.OrdinalIgnoreCase)).ToList(),
                        _ => throw new ArgumentOutOfRangeException()
                    },

                FilterType.Negative => filter.FilterSearchType switch
                    {
                        FilterSearchType.Contains => repos.Where(r => !r.Name.Contains(filter.FilterText, StringComparison.OrdinalIgnoreCase)).ToList(),
                        FilterSearchType.StartsWith => repos.Where(r => !r.Name.StartsWith(filter.FilterText, StringComparison.OrdinalIgnoreCase)).ToList(),
                        FilterSearchType.EndsWith => repos.Where(r => !r.Name.EndsWith(filter.FilterText, StringComparison.OrdinalIgnoreCase)).ToList(),
                        _ => throw new ArgumentOutOfRangeException()
                    },

                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        /// Download a repository using the Github API. Repositories are downloaded as a zip archive which is the extracted.
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="path"></param>
        /// <param name="branch"></param>
        /// <param name="extract"></param>
        /// <returns></returns>
        public async Task DownloadAndExtractRepository(GithubRepo repo, string path, string branch, bool extract)
        {
            using var client = GetClient();
            
            var response = await client.GetAsync($"/repos/{GithubContext.Credentials.AccountName}/{repo.Name}/zipball/{branch}");

            response.EnsureSuccessStatusCode();

            var repoPath = Path.Join(path, repo.Name);
            var zip = $"{repoPath}.zip";

            await using (var fs = new FileStream(zip, FileMode.Create, FileAccess.Write, FileShare.None))
            await using (Stream contentStream = await response.Content.ReadAsStreamAsync())
            {
                await contentStream.CopyToAsync(fs);
            }

            if (extract)
                ZipFile.ExtractToDirectory(zip, repoPath);
        }

        /// <summary>
        /// Return an HttpClient set up for calling the Github API
        /// </summary>
        /// <returns></returns>
        private HttpClient GetClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com")
            };

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", GithubContext.Credentials.ApiKey);
            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
