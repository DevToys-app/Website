using Octokit;
using Website.Core;
using Website.Models;

namespace Website.Helpers
{
    internal static class GitHubHelper
    {
        private const string GitHubOrganization = "DevToys-app";
        private const string GitHubRepositoryName = "DevToys";

        private const string FileName_Debian_CLI_Deb_ARM64 = "devtoys.cli_linux_arm.deb";
        private const string FileName_Debian_CLI_Zip_ARM64 = "devtoys.cli_linux_arm.zip";
        private const string FileName_Debian_CLI_Zip_Lightweight_ARM64 = "devtoys.cli_linux_arm_portable.zip";
        private const string FileName_Debian_CLI_Deb_X64 = "devtoys.cli_linux_x64.deb";
        private const string FileName_Debian_CLI_Zip_X64 = "devtoys.cli_linux_x64.zip";
        private const string FileName_Debian_CLI_Zip_Lightweight_X64 = "devtoys.cli_linux_x64_portable.zip";

        private const string FileName_MacOS_CLI_Zip_ARM64 = "devtoys.cli_osx_arm64.zip";
        private const string FileName_MacOS_CLI_Zip_Lightweight_ARM64 = "devtoys.cli_osx_arm64_portable.zip";
        private const string FileName_MacOS_CLI_Zip_X64 = "devtoys.cli_osx_x64.zip";
        private const string FileName_MacOS_CLI_Zip_Lightweight_X64 = "devtoys.cli_osx_x64_portable.zip";

        private const string FileName_Windows_CLI_Zip_ARM64 = "devtoys.cli_win_arm64.zip";
        private const string FileName_Windows_CLI_Zip_Lightweight_ARM64 = "devtoys.cli_win_arm64_portable.zip";
        private const string FileName_Windows_CLI_Zip_X64 = "devtoys.cli_win_x64.zip";
        private const string FileName_Windows_CLI_Zip_Lightweight_X64 = "devtoys.cli_win_x64_portable.zip";
        private const string Filename_Windows_CLI_Zip_X86 = "devtoys.cli_win_x86.zip";
        private const string FileName_Windows_CLI_Zip_Lightweight_X86 = "devtoys.cli_win_x86_portable.zip";

        private const string FileName_Debian_GUI_Deb_ARM64 = "devtoys_linux_arm.deb";
        private const string FileName_Debian_GUI_Zip_ARM64 = "devtoys.cli_linux_arm_portable.zip";
        private const string FileName_Debian_GUI_Deb_X64 = "devtoys_linux_x64.deb";
        private const string FileName_Debian_GUI_Zip_X64 = "devtoys.cli_linux_x64_portable.zip";

        private const string FileName_MacOS_GUI_AppBundle_ARM64 = "devtoys_osx_arm64.zip";
        private const string FileName_MacOS_GUI_AppBundle_X64 = "devtoys_osx_x64.zip";

        private const string FileName_Windows_GUI_Installer_ARM64 = "devtoys_win_arm64.exe";
        private const string FileName_Windows_GUI_Zip_ARM64 = "devtoys_win_arm64_portable.zip";
        private const string FileName_Windows_GUI_Installer_X64 = "devtoys_win_x64.exe";
        private const string FileName_Windows_GUI_Zip_X64 = "devtoys_win_x64_portable.zip";
        private const string FileName_Windows_GUI_Installer_X86 = "devtoys_win_x86.exe";
        private const string FileName_Windows_GUI_Zip_X86 = "devtoys_win_x86_portable.zip";

        private readonly static DisposableSemaphore semaphore = new DisposableSemaphore();
        private readonly static GitHubClient githubClient = new GitHubClient(new ProductHeaderValue("DevToys-Website"));
        private static DateTime lastUpdate = DateTime.MinValue;
        private static IReadOnlyDictionary<DownloadAssetType, string> assetsURLsCache = InitializeAssetsUrlsCache();

        internal static async Task<IReadOnlyDictionary<DownloadAssetType, string>> GetLatestAssetsLinksAsync()
        {
            if (DateTime.Now - lastUpdate > TimeSpan.FromMinutes(15))
            {
                using (await semaphore.WaitAsync(CancellationToken.None))
                {
                    if (DateTime.Now - lastUpdate > TimeSpan.FromMinutes(15))
                    {
                        lastUpdate = DateTime.Now;

                        // We update the assets list at most once every 15 minutes, in case if a new release got rolled out.
                        assetsURLsCache = await UpdateAssetsListAsync();
                    }
                }
            }

            return assetsURLsCache;
        }

        private static async Task<IReadOnlyDictionary<DownloadAssetType, string>> UpdateAssetsListAsync()
        {
            try
            {
                var latestRelease = await githubClient.Repository.Release.GetLatest(GitHubOrganization, GitHubRepositoryName);

                var assets = await githubClient.Repository.Release.GetAllAssets(GitHubOrganization, GitHubRepositoryName, latestRelease.Id);

                var assetsURLs = new Dictionary<DownloadAssetType, string>();

                assetsURLs[DownloadAssetType.Windows_GUI_Installer_X86] = assets.Single(a => a.Name == FileName_Windows_GUI_Installer_X86).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Windows_GUI_Installer_X64] = assets.Single(a => a.Name == FileName_Windows_GUI_Installer_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Windows_GUI_Installer_ARM64] = assets.Single(a => a.Name == FileName_Windows_GUI_Installer_ARM64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Windows_GUI_Zip_X86] = assets.Single(a => a.Name == FileName_Windows_GUI_Zip_X86).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Windows_GUI_Zip_X64] = assets.Single(a => a.Name == FileName_Windows_GUI_Zip_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Windows_GUI_Zip_ARM64] = assets.Single(a => a.Name == FileName_Windows_GUI_Zip_ARM64).BrowserDownloadUrl;

                assetsURLs[DownloadAssetType.Windows_CLI_Zip_X86] = assets.Single(a => a.Name == Filename_Windows_CLI_Zip_X86).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Windows_CLI_Zip_X64] = assets.Single(a => a.Name == FileName_Windows_CLI_Zip_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Windows_CLI_Zip_ARM64] = assets.Single(a => a.Name == FileName_Windows_CLI_Zip_ARM64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Windows_CLI_Zip_Lightweight_X86] = assets.Single(a => a.Name == FileName_Windows_CLI_Zip_Lightweight_X86).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Windows_CLI_Zip_Lightweight_X64] = assets.Single(a => a.Name == FileName_Windows_CLI_Zip_Lightweight_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Windows_CLI_Zip_Lightweight_ARM64] = assets.Single(a => a.Name == FileName_Windows_CLI_Zip_Lightweight_ARM64).BrowserDownloadUrl;

                assetsURLs[DownloadAssetType.MacOS_GUI_AppBundle_X64] = assets.Single(a => a.Name == FileName_MacOS_GUI_AppBundle_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.MacOS_GUI_AppBundle_ARM64] = assets.Single(a => a.Name == FileName_MacOS_GUI_AppBundle_ARM64).BrowserDownloadUrl;

                assetsURLs[DownloadAssetType.MacOS_CLI_Zip_X64] = assets.Single(a => a.Name == FileName_MacOS_CLI_Zip_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.MacOS_CLI_Zip_ARM64] = assets.Single(a => a.Name == FileName_MacOS_CLI_Zip_ARM64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.MacOS_CLI_Zip_Lightweight_X64] = assets.Single(a => a.Name == FileName_MacOS_CLI_Zip_Lightweight_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.MacOS_CLI_Zip_Lightweight_ARM64] = assets.Single(a => a.Name == FileName_MacOS_CLI_Zip_Lightweight_ARM64).BrowserDownloadUrl;

                assetsURLs[DownloadAssetType.Debian_GUI_Deb_X64] = assets.Single(a => a.Name == FileName_Debian_GUI_Deb_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Debian_GUI_Deb_ARM64] = assets.Single(a => a.Name == FileName_Debian_GUI_Deb_ARM64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Debian_GUI_Zip_X64] = assets.Single(a => a.Name == FileName_Debian_GUI_Zip_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Debian_GUI_Zip_ARM64] = assets.Single(a => a.Name == FileName_Debian_GUI_Zip_ARM64).BrowserDownloadUrl;

                assetsURLs[DownloadAssetType.Debian_CLI_Deb_X64] = assets.Single(a => a.Name == FileName_Debian_CLI_Deb_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Debian_CLI_Deb_ARM64] = assets.Single(a => a.Name == FileName_Debian_CLI_Deb_ARM64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Debian_CLI_Zip_X64] = assets.Single(a => a.Name == FileName_Debian_CLI_Zip_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Debian_CLI_Zip_ARM64] = assets.Single(a => a.Name == FileName_Debian_CLI_Zip_ARM64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Debian_CLI_Zip_Lightweight_X64] = assets.Single(a => a.Name == FileName_Debian_CLI_Zip_Lightweight_X64).BrowserDownloadUrl;
                assetsURLs[DownloadAssetType.Debian_CLI_Zip_Lightweight_ARM64] = assets.Single(a => a.Name == FileName_Debian_CLI_Zip_Lightweight_ARM64).BrowserDownloadUrl;

                return assetsURLs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return assetsURLsCache;
            }
        }

        internal static IReadOnlyDictionary<DownloadAssetType, string> InitializeAssetsUrlsCache()
        {
            return new Dictionary<DownloadAssetType, string>
            {
                { DownloadAssetType.Windows_GUI_Installer_X86, "#" },
                { DownloadAssetType.Windows_GUI_Installer_X64, "#" },
                { DownloadAssetType.Windows_GUI_Installer_ARM64, "#" },
                { DownloadAssetType.Windows_GUI_Zip_X86, "#" },
                { DownloadAssetType.Windows_GUI_Zip_X64, "#" },
                { DownloadAssetType.Windows_GUI_Zip_ARM64, "#" },
                { DownloadAssetType.Windows_CLI_Zip_X86, "#" },
                { DownloadAssetType.Windows_CLI_Zip_X64, "#" },
                { DownloadAssetType.Windows_CLI_Zip_ARM64, "#" },
                { DownloadAssetType.Windows_CLI_Zip_Lightweight_X86, "#" },
                { DownloadAssetType.Windows_CLI_Zip_Lightweight_X64, "#" },
                { DownloadAssetType.Windows_CLI_Zip_Lightweight_ARM64, "#" },
                { DownloadAssetType.MacOS_GUI_AppBundle_X64, "#" },
                { DownloadAssetType.MacOS_GUI_AppBundle_ARM64, "#" },
                { DownloadAssetType.MacOS_CLI_Zip_X64, "#" },
                { DownloadAssetType.MacOS_CLI_Zip_ARM64, "#" },
                { DownloadAssetType.MacOS_CLI_Zip_Lightweight_X64, "#" },
                { DownloadAssetType.MacOS_CLI_Zip_Lightweight_ARM64, "#" },
                { DownloadAssetType.Debian_GUI_Deb_X64, "#" },
                { DownloadAssetType.Debian_GUI_Deb_ARM64, "#" },
                { DownloadAssetType.Debian_GUI_Zip_X64, "#" },
                { DownloadAssetType.Debian_GUI_Zip_ARM64, "#" },
                { DownloadAssetType.Debian_CLI_Deb_X64, "#" },
                { DownloadAssetType.Debian_CLI_Deb_ARM64, "#" },
                { DownloadAssetType.Debian_CLI_Zip_X64, "#" },
                { DownloadAssetType.Debian_CLI_Zip_ARM64, "#" },
                { DownloadAssetType.Debian_CLI_Zip_Lightweight_X64, "#" },
                { DownloadAssetType.Debian_CLI_Zip_Lightweight_ARM64 , "#" },
            };
        }
    }
}
