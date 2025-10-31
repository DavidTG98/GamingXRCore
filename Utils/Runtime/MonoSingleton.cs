using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    public static T I
    {
        get
        {
            if (!_instance)
            {
                var objs = FindObjectsByType(typeof(T), FindObjectsInactive.Include, FindObjectsSortMode.None) as T[];

                if (objs.Length > 0)
                    _instance = objs[0];
                else
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }

                _instance.Init();
            }

            return _instance;
        }
    }

    protected virtual void Init() { }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            var objs = FindObjectsByType(typeof(T), FindObjectsInactive.Include, FindObjectsSortMode.None) as T[];

            foreach (var item in objs)
            {
                if (item != _instance)
                {
                    Destroy(item);
                }
            }

            return;
        }

        _instance = this as T;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene prevScene, Scene newScene)
    {
        //Destrói todas as duplicatas ao trocar de cena
        var objs = FindObjectsByType(typeof(T), FindObjectsInactive.Include, FindObjectsSortMode.None) as T[];
        foreach (var item in objs)
        {
            if (item != _instance)
            {
                Destroy(item.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }
}