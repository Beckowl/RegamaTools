using HarmonyLib;
using UnityEngine;

namespace RegamaTools.patches;

[HarmonyPatch]
internal class MaterialPicking
{
    [HarmonyPatch(typeof(CubeModelingStateMachine), "Update")]
    [HarmonyPostfix]
    private static void DoMaterialPicking(CubeModelingStateMachine __instance)
    {

        if (MVInputWrapper.DebugGetKeyDown(KeyCode.Mouse2))
        {
            var cube = __instance.SelectedCube;

            if (cube != null)
            {

                __instance.CurrentMaterialId = cube.cube.faceMaterials[(int)cube.pickedFace];
            }
        }
    }
}
