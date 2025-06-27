using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using RegamaTools.behaviours;

namespace RegamaTools;

[BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private Harmony _harmony = new(PluginInfo.GUID);

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {PluginInfo.NAME} is loaded!");

        _harmony.PatchAll();

        gameObject.AddComponent<ScreenSizeChecker>();
    }
}
