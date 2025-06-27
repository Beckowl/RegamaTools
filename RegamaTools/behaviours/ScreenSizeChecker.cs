using UnityEngine;

namespace RegamaTools.behaviours;

public class ScreenSizeChecker : MonoBehaviour
{
    public static ScreenSizeChecker Instance;

    private float _previousWidth;
    private float _previousHeight;

    public delegate void OnScreenSizeChanged();
    public OnScreenSizeChanged onScreenSizeChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
            return;
        }
        Instance.onScreenSizeChanged = null;
        Destroy(gameObject);
    }

    private void Init()
    {
        _previousWidth = Screen.width;
        _previousHeight = Screen.height;
    }

    private void Update()
    {
        if (onScreenSizeChanged == null) { return; }
        if (_previousWidth != Screen.width || _previousHeight != Screen.height)
        {
            _previousWidth = Screen.width;
            _previousHeight = Screen.height;
            onScreenSizeChanged.Invoke();
        }
    }

}