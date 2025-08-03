using System.Threading.Tasks;

namespace LiesOfPractice.Interfaces;

public interface IGitHubService
{
    public Task CheckGitHubNewerVersion();
}
