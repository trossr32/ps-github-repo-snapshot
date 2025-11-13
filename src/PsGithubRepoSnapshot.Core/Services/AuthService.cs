using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using PsGithubRepoSnapshot.Core.Models.Github;

namespace PsGithubRepoSnapshot.Core.Services;

public static class AuthService
{
    private static readonly string ConfigFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ps_github_repo_snapshot_config.json");
        
    /// <summary>
    /// Attempt to retrieve credentials from a locally stored file.
    /// </summary>
    /// <returns></returns>
    public static async Task<GithubCredentials> GetConfig()
    {
        // if no config file exists then we have no credentials
        if (!File.Exists(ConfigFile))
            return null;

        var encrypted = await File.ReadAllTextAsync(ConfigFile);

        var decrypted = await EncryptionService.Decrypt(encrypted);

        return JsonSerializer.Deserialize<GithubCredentials>(decrypted);
    }

    /// <summary>
    /// Create a local config file in the module directory with encrypted auth credentials.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static async Task SetConfig(GithubCredentials config)
    {
        var encrypted = await EncryptionService.Encrypt(JsonSerializer.Serialize(config));

        await File.WriteAllTextAsync(ConfigFile, encrypted);
    }

    /// <summary>
    /// Remove config file if it exists and suppress any errors if it doesn't exist.
    /// </summary>
    public static void RemoveConfig()
    {
        try
        {
            File.Delete(ConfigFile);
        }
        catch (Exception) { }
    }
}