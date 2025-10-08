using UnityEngine;

public abstract class SO_Singleton<T> : ScriptableObject where T : Object
{
    private static bool init;
    private static T _instance;

    public static T I
    {
        get
        {
            if (!init)
                Init();

            return _instance;
        }
    }

//#if UNITY_EDITOR
//    [ContextMenu("ForceInit"), RuntimeInitializeOnLoadMethod]
//#endif
    private static void Init()
    {
        _instance = Resources.Load<T>($"SO_Singletons/{typeof(T).Name}");
        init = true;
    }

    protected virtual void OnInit() { }
}