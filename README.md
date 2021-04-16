# PsGithubRepoSnapshot

[![PowerShell Gallery Version](https://img.shields.io/powershellgallery/v/GithubRepoSnapshot?label=GithubRepoSnapshot&logo=powershell&style=plastic)](https://www.powershellgallery.com/packages/GithubRepoSnapshot)
[![PowerShell Gallery](https://img.shields.io/powershellgallery/dt/GithubRepoSnapshot?style=plastic)](https://www.powershellgallery.com/packages/GithubRepoSnapshot)

A Powershell module that integrates with the Github API and downloads a snapshot of all repositories for a user or organisation.

Available in the [Powershell Gallery](https://www.powershellgallery.com/packages/GithubRepoSnapshot)

## Description

### Get-GithubRepos

Get a list of repositories from the Github API for a user or organisation. Optionally filter the results.

Uses endpoints: 

[Github API docs (get repositories for user)](https://docs.github.com/en/rest/reference/repos#list-repositories-for-a-user).
[Github API docs (get repositories for organisation)](https://docs.github.com/en/rest/reference/repos#list-organization-repositories).

## Requirements

A Github personal access token needs to be created with permissions to access repositories: [Github personal access tokens](https://github.com/settings/tokens).

## Installation (from the Powershell Gallery)

```powershell
Install-Module GithubRepoSnapshot
Import-Module GithubRepoSnapshot
```

## Supplying API credentials

Before any of the GithubRepoSnapshot cmdlets can be run, both your host and credentials need to be registered to the current session using the Set-GithubRepoSnapshotCredentials cmdlet, for example:

```powershell
Set-GithubCredentials -AccountName 'user' -ApiKey 'api_key' -AccountType User
```

This registers your credentials for the duration of the session. Adding a -StorePermanent switch to the Set-GithubRepoSnapshotCredentials command will create an encrypted file saved on your machine that will obviate the need to set credentials with each new session:

```powershell
Set-GithubCredentials -AccountName 'user' -ApiKey 'api_key' -AccountType User -StorePermanent
```

You can remove credentials at any time by using the Remove-GithubRepoSnapshotCredentials cmdlet. To remove a file created using the -StorePermanent switch run the Remove-GithubRepoSnapshotCredentials with a -DeletePermanent switch:

```powershell
Remove-GithubCredentials -DeletePermanent
```

## Included cmdlets

### Credentials

```powershell
Set-GithubCredentials
Remove-GithubCredentials
```

### Get repositories

```powershell
Get-GithubRepos
```

### Create snapshot

```powershell
New-GithubRepoSnapshot
```

## Building the module and importing locally

### Build the .NET core solution

```powershell
dotnet build [Github clone/download directory]\ps-github-repo-snapshot\src\PsGithubRepoSnapshot.sln
```

### Copy the built files to your Powershell modules directory

Remove any existing installation in this directory, create a new module directory and copy all the built files.

```powershell
Remove-Item "C:\Users\[User]\Documents\PowerShell\Modules\GithubRepoSnapshot" -Recurse -Force -ErrorAction SilentlyContinue
New-Item -Path 'C:\Users\[User]\Documents\PowerShell\Modules\GithubRepoSnapshot' -ItemType Directory
Get-ChildItem -Path "[Github clone/download directory]\ps-github-repo-snapshot\src\PsGithubRepoSnapshotCmdlet\bin\Debug\netcoreapp3.1\" | Copy-Item -Destination "C:\Users\[User]\Documents\PowerShell\Modules\GithubRepoSnapshot" -Recurse
```

## Contribute

Please raise an issue if you find a bug or want to request a new feature, or create a pull request to contribute.
