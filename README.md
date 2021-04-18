# RevitLauncher

A Revit launcher integrated in the windows context menu.

Automatically detect the Revit file version, support opening the file in the new process, or open in the current existing Revit process.

Supports ".rvt", ".rte", ".rft", ".rfa" 4 file extensions.

## Third Party 

- [SharpShell](https://github.com/dwmkerr/sharpshell)
- [OpenMcdf](https://github.com/ironfede/openmcdf)

## Installation

Get the latest installer from the [Releases](https://github.com/Zhuangkh/RevitLauncher/releases) page.

This project uses [Microsoft Visual Studio Installer Projects](https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2017InstallerProjects) to generate the MSI package.

At the same time, with the help of [srm.exe](https://www.nuget.org/packages/ServerRegistrationManager) to complete the registration of shell extensions.

You may need to restart the *Explorer* process after completing the MSI installation to see the effect.

## License

[Apache 2.0 License](./LICENSE).
