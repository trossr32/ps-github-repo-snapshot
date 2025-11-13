using PsGithubRepoSnapshot.Core.Enums;

namespace PsGithubRepoSnapshot.Core.Models;

public class FilterConfig
{
    public FilterType FilterType { get; set; }
    public FilterSearchType FilterSearchType { get; set; }
    public string FilterText { get; set; }
}