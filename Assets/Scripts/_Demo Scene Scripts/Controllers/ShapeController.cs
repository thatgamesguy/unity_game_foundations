using UnityEngine;
using System.Collections;


public class ShapeController : MonoBehaviour
{
    public AudioClip SelectedSound;

    private static readonly int NUMBER_TO_SPAWN = 2;

    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < -5)
        {
            try
            {
                ObjectPool.instance.PoolObject(gameObject);
            }
            catch (PrefabNotFoundException ex)
            {
                Debug.Log(ex.Message);
                Destroy(gameObject);
            }
        }
    }

    protected void OnSelected(GameEvent e)
    {
        if (transform.localScale.x < 0.85f)
            return;

        Events.instance.Raise(new AudioEvent2D(SelectedSound));

        for (int i = 0; i < NUMBER_TO_SPAWN; i++)
            Spawn();

        try
        {
            ObjectPool.instance.PoolObject(gameObject);
        }
        catch (PrefabNotFoundException ex)
        {
            Debug.Log(ex.Message);
            Destroy(gameObject);
        }
    }

    protected void Spawn()
    {
        GameObject cube = null;

        try
        {
            cube = ObjectPool.instance.GetObject(this.name);
            cube.transform.position = new Vector3(transform.position.x + Random.Range(-1f, 1f),
                         transform.position.y,
                         transform.position.z + Random.Range(-1f, 1f));

            var scale = transform.localScale * 0.9f;
            cube.transform.localScale = scale;
            cube.SetActive(true);

        }
        catch (PrefabNotFoundException e)
        {
            Debug.Log(e.Message);
        }
    }

    protected void OnExplosion(ExplosionEvent e)
    {
        if (e.InExplosionRadius(transform.position))
        {
            _rigidbody.AddExplosionForce(e.Force, e.Position, e.Radius);
        }
    }
}

