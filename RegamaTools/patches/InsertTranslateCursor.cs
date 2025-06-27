using HarmonyLib;
using UnityEngine;

namespace RegamaTools.patches;

[HarmonyPatch]
internal class InsertTranslateCursor
{
    private static bool _inInsertOrTranslate = false;

    [HarmonyPatch(typeof(ESInsert), "Enter")]
    [HarmonyPatch(typeof(ESTranslate), "Enter")]
    [HarmonyPrefix]
    private static void Hide()
    {
        Cursor.visible = false;
        _inInsertOrTranslate = true;
    }

    [HarmonyPatch(typeof(ESInsert), "Exit")]
    [HarmonyPatch(typeof(ESTranslate), "Exit")]
    [HarmonyPrefix]
    private static void UnHide(ESStateBase __instance)
    {
        Cursor.visible = false;
        _inInsertOrTranslate = false;
    }

    [HarmonyPatch(typeof(LockCursorManager), "LateUpdate")]
    [HarmonyPrefix]
    private static bool ApplyCursorVisibility(LockCursorManager __instance)
    {
        if (UXUtils.UXScreen.Fullscreen)
        {
            LockCursorManager.hasFocus = true;
        }

        __instance.HandleCursorLock();
        if (!_inInsertOrTranslate)
        {
            Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
        }

        return false;
    }
}
