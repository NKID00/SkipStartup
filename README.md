> **Note: This mod is more likely to be an experiment on mod developing for DDLC+ and may not be stable enough since it comes from reverse engineering. I would appreciate it if you [open an issue](https://github.com/NKID00/SkipStartup/issues/new) to describe the situation when the mod malfunctions.**

## SkipStartup

Skip most of the startup processes (Bios, Startup and Login, **excluding the initial progress bar**) in [DDLC+](ddlc.plus) and present the desktop directly.

## Install & Uninstall (for non-developer users)

You need to have a copy of the PC version of the game to install this mod. **Console versions are not supported. This mod may conflict with other mods, please install this mod only on the vanilla game!**

To install this mod, download the latest [release](https://github.com/NKID00/SkipStartup/releases) and extract the zip file into the root directory of the game. Not such overwriting or modifying the original game files would happen.

To uninstall it, simply delete the files extracted in the installation process.

## Build (for developers)

1. Clone this repo to local.

2. Open the solution file `SkipStartup.sln` with Visual Studio 2019 (with C# and .NET components installed).

3. Open the `SkipStartup` project and fix the broken assembly reference paths of `DDLC.dll`, `Unity.Postprocessing.Runtime.dll`, `UnityEngine.CoreModule.dll` and `UnityEngine.UIModule.dll` (you can find them in the `<path to game>/Doki Doki Literature Club Plus_Data/Managed/` directory).

4. Build the `SkipStartup` project. The assemblies are generated in the `bin/<Debug or Release>/netstandard2.0` directory.

After building the mod, you still need a injector (like the [UnityDoorstop](https://github.com/NeighTools/UnityDoorstop) used in the release) or a mod loader to run the static method `SkipStartup.Program.Main` before the game starts.

## Developing notes (may be useful for mod developers)

- The preferences and saves is stored in the `%USERPROFILE%/AppData/LocalLow/Team Salvato/Doki Doki Literature Club Plus/` Directory (for Windows). Always backup these files, especially when you are dealing with multiple game instances!

- Most of the game logics is written in the `DDLC.dll`.

- The desktop environment in DDLC+ is divided into many `RenpyLauncher.LauncherApp`s and is managed by the class `RenpyLauncher.LauncherMain`. These apps will be loaded at start but only be switched to when necessary (and only one app can be switched to at once). This mod let the game switches to the `RenpyLauncher.BiosApp`, `RenpyLauncher.StartupApp` and `RenpyLauncher.LoginApp` but does not run its normal sequence, then switches directly into the `RenpyLauncher.DesktopApp`. I haven't figured out which part of the game controls the initial progress bar yet.

## License

MIT
