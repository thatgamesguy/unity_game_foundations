using UnityEngine;

/// <summary>
/// A base class for any Singleton. Provides global singular access to a MonoBehaviour.
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// Gets a value indicating whether this instance is destroyed.
    /// </summary>
    /// <value><c>true</c> if is destroyed; otherwise, <c>false</c>.</value>
    public static bool IsDestroyed { get { return _instance == null; } }

    private static bool _applicationIsQuitting = false;

    /// <summary>
    /// Sets whether this instance will be lazily initialised i.e. only initialised when needed.
    /// </summary>
    private static readonly bool LAZY_INIT = false;

    private static T _instance = null;

    /// <summary>
    /// Gets the instance. The instance is created if not currently part of the scene.
    /// </summary>
    /// <value>The instance.</value>
    public static T instance
    {
        get
        {
            // Don't initialise new _instance if application quiting.
            if (_applicationIsQuitting)
            {
                return _instance;
            }

            if (!_instance)
            {
                Initialise();
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (!LAZY_INIT)
        {
            Initialise();
        }

        _applicationIsQuitting = false;
    }

    /// <summary>
    /// Attempt to find object if present in scene. If not found it is created.
    /// </summary>
    private static void Initialise()
    {
        _instance = GameObject.FindObjectOfType<T>();

        if (!_instance)
        {
            _instance = new GameObject().AddComponent<T>();
            _instance.gameObject.name = _instance.GetType().Name;
        }
    }

    void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }
}

