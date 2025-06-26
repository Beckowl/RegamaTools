using HarmonyLib;

namespace RegamaTools.patches;

[HarmonyPatch]
internal class NoTextureMaterial
{

    [HarmonyPatch(typeof(MVMaterialRepository), MethodType.Constructor)]
    [HarmonyPostfix]
    private static void SetMaterialProperties(MVMaterialRepository __instance)
    {
        __instance.noMaterial.name = "No material";
        __instance.noMaterial.description = "The forbidden material.";
        __instance.noMaterial.isUnlocked = true;
    }


    [HarmonyPatch(typeof(MVGUIMaterialSelectionWindow), "AddMaterialsToPage")]
    [HarmonyPrefix]
    private static void AddNoLimitMaterial(MVGUIMaterialSelectionWindow __instance, UXGroup pageGroup, int materialIdOffset, ref int materials)
    {
        int maxPage = __instance._totalMaterialCount / __instance.MaxCubesPerPage;
        if (__instance._totalMaterialCount % __instance.MaxCubesPerPage != 0)
        {
            maxPage++;
        }

        int pageIndex = materialIdOffset / __instance.MaxCubesPerPage;

        if (pageIndex == maxPage - 1)
        {
            materials++;
        }
    }
}
