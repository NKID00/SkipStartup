using HarmonyLib;
using System;
using UnityEngine.Rendering.PostProcessing;

namespace SkipStartup
{
    public class Program
    {
        public static void Main()
        {
            Harmony.CreateAndPatchAll(typeof(Program));
        }

        [HarmonyPatch(typeof(RenpyLauncher.LauncherMain), "Start")]
        [HarmonyPrefix]
        public static bool StartPrefix(RenpyLauncher.LauncherMain __instance)
        {
            RenpyLauncher.LauncherParameters.Initialize("renpymain", skipPersistentData: false);
            
            var SaveManager = Traverse.Create(__instance).Field<LauncherSaveManager>("m_SaveManager").Value;
            
            RenpyLauncher.LauncherAppId newStartupApp = RenpyLauncher.LauncherAppId.Bios;
            if (!SaveManager.HasIteractedFile().GetAwaiter().GetResult())
            {
                LauncherMain.LoadDDLCScene = true;
                newStartupApp = RenpyLauncher.LauncherAppId.DokiDoki;
            }
            
            if (newStartupApp == RenpyLauncher.LauncherAppId.Bios)
            {
                Type[] types = { typeof(RenpyLauncher.LauncherAppId) };
                var GetAppIndex = Traverse.Create(__instance).Method("GetAppIndex", paramTypes: types);

                __instance.SwitchToApp(RenpyLauncher.LauncherAppId.Bios).Wait();
                Traverse.Create(__instance.apps[GetAppIndex.GetValue<int>(RenpyLauncher.LauncherAppId.Bios)]).Field("m_SequenceFinished").SetValue(true);

                __instance.SwitchToApp(RenpyLauncher.LauncherAppId.BootUp).Wait();
                Traverse.Create(__instance.apps[GetAppIndex.GetValue<int>(RenpyLauncher.LauncherAppId.BootUp)]).Field("m_SequenceFinished").SetValue(true);
                RenpyLauncher.LauncherMain.soundSource.StopLoopingAudio();

                __instance.SwitchToApp(RenpyLauncher.LauncherAppId.Login).Wait();
                Renpy.LauncherMain.LoadLauncher().Wait();
                Renpy.LauncherMain.LoadPreferences().Wait();
                var LoginApp = __instance.apps[GetAppIndex.GetValue<int>(RenpyLauncher.LauncherAppId.Login)] as RenpyLauncher.LoginApp;
                LoginApp.globalPostProcessVolume.sharedProfile.GetSetting<Bloom>().enabled.Override(x: false);
                LoginApp.globalPostProcessVolume.sharedProfile.GetSetting<Blur>().enabled.Override(x: false);
                Traverse.Create(LoginApp).Field("m_SequenceFinished").SetValue(true);
                RenpyLauncher.LauncherMain.PlayLoginComplete();
                Renpy.LauncherMain.CheckForWallpaperChange();
                LoginApp.NoEffectsCanvas.enabled = false;
                LoginApp.globalPostProcessVolume.enabled = false;
                LoginApp.cameraPostProcessLayer.enabled = false;
                Renpy.LauncherMain.OnLauncherLoaded();

                __instance.SwitchToApp(RenpyLauncher.LauncherAppId.Desktop).Wait();
            }
            else
            {
                __instance.SwitchToApp(newStartupApp).Wait();
            }

            return false;
        }
    }
}
