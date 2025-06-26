using System;
using System.Reflection;
using System.Runtime.InteropServices;
using HarmonyLib;
using UnityEngine;

namespace RegamaTools.patches;

[HarmonyPatch]
internal class HideCursor
{
    [HarmonyPatch(typeof(ESInsert), "Enter")]
    [HarmonyPatch(typeof(ESTranslate), "Enter")]
    [HarmonyPrefix]
    private static void Hide()
    {
        LockCursorManager.LockCursor = true;
    }

    [HarmonyPatch(typeof(ESInsert), "Exit")]
    [HarmonyPatch(typeof(ESTranslate), "Exit")]
    [HarmonyPrefix]
    private static void UnHide(ESStateBase __instance)
    {
        LockCursorManager.LockCursor = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var laserField = __instance.GetType().GetField("laser", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (laserField == null) return;

        var laser = laserField.GetValue(__instance) as LaserPointer;

        Vector3 laserTargetPos = laser.cube.position + laser.relativeCurrentTargetPosition;

        IntPtr hwnd = GetActiveWindow();
        if (!GetClientRect(hwnd, out RECT clientRect)) return;

        POINT topLeft = new POINT { X = clientRect.Left, Y = clientRect.Top };
        if (!ClientToScreen(hwnd, ref topLeft)) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(laserTargetPos);

        int clampedX = Mathf.Clamp((int)screenPos.x, 0, clientRect.Right - clientRect.Left - 1);
        int clampedY = Mathf.Clamp((int)screenPos.y, 0, clientRect.Bottom - clientRect.Top - 1);

        int cursorX = topLeft.X + clampedX;
        int cursorY = topLeft.Y + (clientRect.Bottom - clientRect.Top) - clampedY;

        SetCursorPos(cursorX, cursorY);
    }

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

}
