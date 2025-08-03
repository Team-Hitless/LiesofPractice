using LiesOfPractice.Interfaces;
using LiesOfPractice.Viewmodels;
using Octokit;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LiesOfPractice.Services;

public class GitHubService(IWindowService windowService, GitHubViewModel gitHubView) : IGitHubService
{
    const long repoId = 1028636440;
    public async Task CheckGitHubNewerVersion()
    {
        //if (!dataService.Config.Settings.CheckUpdates) return;

        GitHubClient client = new(new ProductHeaderValue("LiesOfPractice"));
        IReadOnlyList<Release> releases = await client.Repository.Release.GetAll(repoId);

        var release = releases.ToList()
            .OrderByDescending(item => item.CreatedAt)
            .FirstOrDefault();

        if (release == null)
            return;

        string tag = release.TagName;
        tag = Regex.Replace(tag, "[^0-9.]", "");
        var latestGitHubVersion = new Version(tag);
        var localVersion = Assembly.GetExecutingAssembly().GetName().Version;

        if (localVersion is null)
            return;

        int versionComparison = localVersion.CompareTo(latestGitHubVersion);
        if (versionComparison < 0)
        {
            //The version on GitHub is more up to date than this local release.
            gitHubView.CurrentVersion = localVersion.ToString();
            gitHubView.NewVersion = latestGitHubVersion.ToString();
            gitHubView.Url = release.HtmlUrl;
            windowService.OpenWindow(gitHubView);
        }
    }
}
