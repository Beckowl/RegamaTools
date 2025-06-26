using HarmonyLib;

namespace RegamaTools.patches;

[HarmonyPatch]
internal class DestructiblesUnlock
{
    [HarmonyPatch(typeof(MVMaterial), "IsAvailable", MethodType.Getter)]
    [HarmonyPostfix]
    private static void ForceAvailable(ref bool __result)
    {
        __result = true;
    }
}
