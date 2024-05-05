using Octokit;
using Website.Core;

namespace Website.GitHub
{
    internal static class GitHubHelper
    {
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
        private static DateTime lastUpdate = DateTime.MinValue;
        private static IReadOnlyDictionary<AssetType, string> assetsURLsCache = InitializeAssetsUrlsCache();

        internal static async Task<IReadOnlyDictionary<AssetType, string>> GetLatestAssetsLinksAsync()
        {
            using (await semaphore.WaitAsync(CancellationToken.None))
            {
                if (DateTime.Now - lastUpdate > TimeSpan.FromMinutes(30))
                {
                    lastUpdate = DateTime.Now;

                    // We update the assets list at most once every 30 minutes, in case if a new release got rolled out.
                    assetsURLsCache = await UpdateAssetsListAsync();
                }
            }

            return assetsURLsCache;
        }

        private static async Task<IReadOnlyDictionary<AssetType, string>> UpdateAssetsListAsync()
        {
            try
            {
                string organization = "DevToys-app";
                string repositoryName = "DevToys";
                var client = new GitHubClient(new ProductHeaderValue("DevToysWebsite"));

                var latestRelease = await client.Repository.Release.GetLatest(organization, repositoryName);

                var assets = await client.Repository.Release.GetAllAssets(organization, repositoryName, latestRelease.Id);

                var assetsURLs = new Dictionary<AssetType, string>();

                assetsURLs[AssetType.Windows_GUI_Installer_X86] = assets.Single(a => a.Name == FileName_Windows_GUI_Installer_X86).BrowserDownloadUrl;
                assetsURLs[AssetType.Windows_GUI_Installer_X64] = assets.Single(a => a.Name == FileName_Windows_GUI_Installer_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.Windows_GUI_Installer_ARM64] = assets.Single(a => a.Name == FileName_Windows_GUI_Installer_ARM64).BrowserDownloadUrl;
                assetsURLs[AssetType.Windows_GUI_Zip_X86] = assets.Single(a => a.Name == FileName_Windows_GUI_Zip_X86).BrowserDownloadUrl;
                assetsURLs[AssetType.Windows_GUI_Zip_X64] = assets.Single(a => a.Name == FileName_Windows_GUI_Zip_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.Windows_GUI_Zip_ARM64] = assets.Single(a => a.Name == FileName_Windows_GUI_Zip_ARM64).BrowserDownloadUrl;

                assetsURLs[AssetType.Windows_CLI_Zip_X86] = assets.Single(a => a.Name == Filename_Windows_CLI_Zip_X86).BrowserDownloadUrl;
                assetsURLs[AssetType.Windows_CLI_Zip_X64] = assets.Single(a => a.Name == FileName_Windows_CLI_Zip_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.Windows_CLI_Zip_ARM64] = assets.Single(a => a.Name == FileName_Windows_CLI_Zip_ARM64).BrowserDownloadUrl;
                assetsURLs[AssetType.Windows_CLI_Zip_Lightweight_X86] = assets.Single(a => a.Name == FileName_Windows_CLI_Zip_Lightweight_X86).BrowserDownloadUrl;
                assetsURLs[AssetType.Windows_CLI_Zip_Lightweight_X64] = assets.Single(a => a.Name == FileName_Windows_CLI_Zip_Lightweight_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.Windows_CLI_Zip_Lightweight_ARM64] = assets.Single(a => a.Name == FileName_Windows_CLI_Zip_Lightweight_ARM64).BrowserDownloadUrl;

                assetsURLs[AssetType.MacOS_GUI_AppBundle_X64] = assets.Single(a => a.Name == FileName_MacOS_GUI_AppBundle_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.MacOS_GUI_AppBundle_ARM64] = assets.Single(a => a.Name == FileName_MacOS_GUI_AppBundle_ARM64).BrowserDownloadUrl;

                assetsURLs[AssetType.MacOS_CLI_Zip_X64] = assets.Single(a => a.Name == FileName_MacOS_CLI_Zip_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.MacOS_CLI_Zip_ARM64] = assets.Single(a => a.Name == FileName_MacOS_CLI_Zip_ARM64).BrowserDownloadUrl;
                assetsURLs[AssetType.MacOS_CLI_Zip_Lightweight_X64] = assets.Single(a => a.Name == FileName_MacOS_CLI_Zip_Lightweight_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.MacOS_CLI_Zip_Lightweight_ARM64] = assets.Single(a => a.Name == FileName_MacOS_CLI_Zip_Lightweight_ARM64).BrowserDownloadUrl;

                assetsURLs[AssetType.Debian_GUI_Deb_X64] = assets.Single(a => a.Name == FileName_Debian_GUI_Deb_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.Debian_GUI_Deb_ARM64] = assets.Single(a => a.Name == FileName_Debian_GUI_Deb_ARM64).BrowserDownloadUrl;
                assetsURLs[AssetType.Debian_GUI_Zip_X64] = assets.Single(a => a.Name == FileName_Debian_GUI_Zip_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.Debian_GUI_Zip_ARM64] = assets.Single(a => a.Name == FileName_Debian_GUI_Zip_ARM64).BrowserDownloadUrl;

                assetsURLs[AssetType.Debian_CLI_Deb_X64] = assets.Single(a => a.Name == FileName_Debian_CLI_Deb_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.Debian_CLI_Deb_ARM64] = assets.Single(a => a.Name == FileName_Debian_CLI_Deb_ARM64).BrowserDownloadUrl;
                assetsURLs[AssetType.Debian_CLI_Zip_X64] = assets.Single(a => a.Name == FileName_Debian_CLI_Zip_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.Debian_CLI_Zip_ARM64] = assets.Single(a => a.Name == FileName_Debian_CLI_Zip_ARM64).BrowserDownloadUrl;
                assetsURLs[AssetType.Debian_CLI_Zip_Lightweight_X64] = assets.Single(a => a.Name == FileName_Debian_CLI_Zip_Lightweight_X64).BrowserDownloadUrl;
                assetsURLs[AssetType.Debian_CLI_Zip_Lightweight_ARM64] = assets.Single(a => a.Name == FileName_Debian_CLI_Zip_Lightweight_ARM64).BrowserDownloadUrl;

                return assetsURLs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return assetsURLsCache;
            }
        }

        private static IReadOnlyDictionary<AssetType, string> InitializeAssetsUrlsCache()
        {
            return new Dictionary<AssetType, string>
            {
                { AssetType.Windows_GUI_Installer_X86, "#" },
                { AssetType.Windows_GUI_Installer_X64, "#" },
                { AssetType.Windows_GUI_Installer_ARM64, "#" },
                { AssetType.Windows_GUI_Zip_X86, "#" },
                { AssetType.Windows_GUI_Zip_X64, "#" },
                { AssetType.Windows_GUI_Zip_ARM64, "#" },
                { AssetType.Windows_CLI_Zip_X86, "#" },
                { AssetType.Windows_CLI_Zip_X64, "#" },
                { AssetType.Windows_CLI_Zip_ARM64, "#" },
                { AssetType.Windows_CLI_Zip_Lightweight_X86, "#" },
                { AssetType.Windows_CLI_Zip_Lightweight_X64, "#" },
                { AssetType.Windows_CLI_Zip_Lightweight_ARM64, "#" },
                { AssetType.MacOS_GUI_AppBundle_X64, "#" },
                { AssetType.MacOS_GUI_AppBundle_ARM64, "#" },
                { AssetType.MacOS_CLI_Zip_X64, "#" },
                { AssetType.MacOS_CLI_Zip_ARM64, "#" },
                { AssetType.MacOS_CLI_Zip_Lightweight_X64, "#" },
                { AssetType.MacOS_CLI_Zip_Lightweight_ARM64, "#" },
                { AssetType.Debian_GUI_Deb_X64, "#" },
                { AssetType.Debian_GUI_Deb_ARM64, "#" },
                { AssetType.Debian_GUI_Zip_X64, "#" },
                { AssetType.Debian_GUI_Zip_ARM64, "#" },
                { AssetType.Debian_CLI_Deb_X64, "#" },
                { AssetType.Debian_CLI_Deb_ARM64, "#" },
                { AssetType.Debian_CLI_Zip_X64, "#" },
                { AssetType.Debian_CLI_Zip_ARM64, "#" },
                { AssetType.Debian_CLI_Zip_Lightweight_X64, "#" },
                { AssetType.Debian_CLI_Zip_Lightweight_ARM64 , "#" },
            };
        }
    }
}
