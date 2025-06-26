using HarmonyLib;
using UnityEngine;

namespace RegamaTools.patches;

[HarmonyPatch]
internal class NoBuildLimit
{
    [HarmonyPatch(typeof(ConstraintVisualizer), "Init")]
    [HarmonyPrefix]
    private static void HideConstraintBox(ConstraintVisualizer __instance)
    {
        GameObject.Destroy(__instance.gameObject);
    }

    [HarmonyPatch(typeof(ModelingDynamicBoxConstraint), "CanAddCubeAt")]
    [HarmonyPatch(typeof(ModelingBoxCountConstraint), "CanAddCubeAt")]
    [HarmonyPatch(typeof(ModelingBoxCountConstraint), "CanRemoveCubeAt")]
    [HarmonyPostfix]
    private static void NoLimitPatches(ref bool __result)
    {
        __result = true;
    }
}
