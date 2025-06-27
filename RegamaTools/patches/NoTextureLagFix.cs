using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace RegamaTools.patches;

[HarmonyPatch]
internal class NoTextureLagFix
{
    // Prevents lag caused by Debug.LogError spam when touching "No Texture" blocks
    // This removes the Debug.LogError call from both methods

    [HarmonyPatch(typeof(MVMaterialRepository), "GetMaterial")]
    [HarmonyPatch(typeof(MVMaterialRepository), "GetMaterialPhysicalProperties")]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> GetMaterial(IEnumerable<CodeInstruction> instructions)
    {
        var code = new List<CodeInstruction>(instructions);
        var logMethod = AccessTools.Method(typeof(Debug), nameof(Debug.LogError), [typeof(object)]);

        for (int i = 0; i < code.Count; i++)
        {
            if (code[i].Calls(logMethod))
            {
                // remove ldstr
                if (i > 0 && code[i - 1].opcode == OpCodes.Ldstr)
                {
                    code.RemoveAt(i - 1);
                    i--;
                }

                code.RemoveAt(i); // remove debug.log call
                break;
            }
        }

        return code;
    }
}
