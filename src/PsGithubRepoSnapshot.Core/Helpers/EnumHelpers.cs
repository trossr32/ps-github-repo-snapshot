using System;
using PsGithubRepoSnapshot.Core.Enums;

namespace PsGithubRepoSnapshot.Core.Helpers;

public static class EnumHelpers
{
    public static string AsUrlPart(this AccountType accountType) => accountType switch {
        AccountType.User => "users",
        AccountType.Organisation => "orgs",
        _ => throw new ArgumentOutOfRangeException(nameof(accountType), accountType, null)
    };
}