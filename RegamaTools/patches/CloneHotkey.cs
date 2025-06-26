using HarmonyLib;

namespace RegamaTools.patches;

[HarmonyPatch]
internal class CloneHotkey
{
    [HarmonyPatch(typeof(ESSelection), "Execute")]
    [HarmonyPostfix]
    private static void Execute(ESSelection __instance, EditorStateMachine e)
    {
        if (MVInputWrapper.DebugGetKeyDown(UnityEngine.KeyCode.Q))
        {
            MVGameController.EditorController.EditorWorldObjectCreation.CloneHierarchy(__instance.selectedWorldObject, false, false, false);
        }
    }
}
