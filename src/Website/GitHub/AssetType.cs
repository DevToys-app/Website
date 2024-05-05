namespace Website.GitHub
{
    internal enum AssetType
    {
        Windows_GUI_Installer_X86,
        Windows_GUI_Installer_X64,
        Windows_GUI_Installer_ARM64,
        Windows_GUI_Zip_X86,
        Windows_GUI_Zip_X64,
        Windows_GUI_Zip_ARM64,

        Windows_CLI_Zip_X86,
        Windows_CLI_Zip_X64,
        Windows_CLI_Zip_ARM64,
        Windows_CLI_Zip_Lightweight_X86,
        Windows_CLI_Zip_Lightweight_X64,
        Windows_CLI_Zip_Lightweight_ARM64,

        MacOS_GUI_AppBundle_X64,
        MacOS_GUI_AppBundle_ARM64,

        MacOS_CLI_Zip_X64,
        MacOS_CLI_Zip_ARM64,
        MacOS_CLI_Zip_Lightweight_X64,
        MacOS_CLI_Zip_Lightweight_ARM64,

        Debian_GUI_Deb_X64,
        Debian_GUI_Deb_ARM64,
        Debian_GUI_Zip_X64,
        Debian_GUI_Zip_ARM64,

        Debian_CLI_Deb_X64,
        Debian_CLI_Deb_ARM64,
        Debian_CLI_Zip_X64,
        Debian_CLI_Zip_ARM64,
        Debian_CLI_Zip_Lightweight_X64,
        Debian_CLI_Zip_Lightweight_ARM64,
    }
}
