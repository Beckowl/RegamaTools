using System;
using System.Runtime.InteropServices;
using HarmonyLib;

namespace RegamaTools.patches;

[HarmonyPatch]
internal class ResizableWindow
{
    const int GWL_STYLE = -16;
    const int WS_SIZEBOX = 0x00040000;
    const int WS_MAXIMIZEBOX = 0x00010000;

    [HarmonyPatch(typeof(MVGameController), "InitStandAlone")]
    [HarmonyPostfix]
    private static void MakeWindowResizable()
    {
        SetWindowResizable();
        ScreenSizeChecker.Instance.onScreenSizeChanged += SetWindowResizable;
    }

    private static void SetWindowResizable()
    {
        IntPtr hWnd = GetActiveWindow();
        int style = GetWindowLong(hWnd, GWL_STYLE);
        style |= WS_SIZEBOX | WS_MAXIMIZEBOX;
        SetWindowLong(hWnd, GWL_STYLE, style);
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
}
