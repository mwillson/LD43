using UnityEngine;
using UnityEngine.Events;
 
[RequireComponent(typeof(RectTransform))]
public class ScreenWatcher : MonoBehaviour
{
    static ScreenWatcher instance = null;
    public static ScreenWatcher Instance { get { return instance; } }
    static UnityEvent OnResolutionChange = null;
    static UnityEvent OnOrientationChange = null;
    static Vector2 resolution; // Current Resolution
    static ScreenOrientation orientation; // Current Screen Orientation
 
    static void init()
    {
        if (instance != null) return;
 
        resolution = new Vector2(Screen.width, Screen.height);
        orientation = Screen.orientation;
 
        OnResolutionChange = new UnityEvent();
        OnOrientationChange = new UnityEvent();
        GameObject canvas = new GameObject("ScreenWatcher");
        canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        instance = canvas.AddComponent<ScreenWatcher>();
        DontDestroyOnLoad(canvas);
    }
 
    private void Start()
    {
        if (instance != this)
        {
            Destroy(this);
        }
    }
 
    private void OnRectTransformDimensionsChange()
    {
        // Check for an orientation change.
        ScreenOrientation curOri = Screen.orientation;
        switch (curOri)
        {
            case ScreenOrientation.Unknown: // Ignore
            {
                break;
            }
            default:
            {
                if (orientation != curOri)
                {
                    orientation = curOri;
                    OnOrientationChange.Invoke();
                }
                break;
            }
        }
 
        // Check for a resolution change.
        if ((resolution.x != Screen.width && resolution.x != Screen.height) || (resolution.y != Screen.height && resolution.y != Screen.width))
        {
            resolution = new Vector2(Screen.width, Screen.height);
            OnResolutionChange.Invoke();
        }
    }
 
    public static void AddResolutionChangeListener(UnityAction callback)
    {
        init();
        OnResolutionChange.AddListener(callback);
    }
 
    public static void RemoveResolutionChangeListener(UnityAction callback)
    {
        OnResolutionChange.RemoveListener(callback);
    }
 
    public static void AddOrientationChangeListener(UnityAction callback)
    {
        init();
        OnOrientationChange.AddListener(callback);
    }
 
    public static void RemoveOrientationChangeListener(UnityAction callback)
    {
        OnOrientationChange.RemoveListener(callback);
    }
 
    private void OnDestroy()
    {
        if (instance == this)
        {
            // Clean up memory.
            OnResolutionChange.RemoveAllListeners();
            OnResolutionChange = null;
            OnOrientationChange.RemoveAllListeners();
            OnOrientationChange = null;
            instance = null;
        }
    }
}