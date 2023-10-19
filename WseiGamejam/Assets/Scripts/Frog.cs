using System;
using ModestTree;
using UnityEngine;
using UnityEngine.EventSystems;

public class Frog : MonoBehaviour
{
    public static Frog Singleton { private set; get; }

    public static event Action FrogMoved;
    public static event Action FromRemoved;
    
    private PlayerManager _playerManager;
    private Player _runner;

    [SerializeField] float jumpLength = 1f;

    [SerializeField] LayerMask layerMask;

    public void Awake()
    {
        if (Singleton)
        {
            Destroy(Singleton);
            Singleton = this;
        }

        Singleton = this;
    }

    void Start()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
        _runner = _playerManager.Runner;
        _runner.RunnerPlayerInput.PlayerMoved += OnPlayerMoved;
    }


    private void OnDestroy()
    {
        _runner.RunnerPlayerInput.PlayerMoved -= OnPlayerMoved;
    }

    private void Update()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.4f, layerMask);
        if(colliders.Length < 1)
        {
            TakeDamage(); 
        }
        else
        {
            foreach (var collider in colliders)
            {
                var platform = collider.GetComponent<Platform>();

                if (platform is null) continue;
                else
                {
                    transform.SetParent(platform.transform);
                    return;
                }
            }

            transform.SetParent(null);
        }
    }

    private void LateUpdate()
    {
        var viewpointCoord = Camera.main.WorldToViewportPoint(transform.position);

        if (viewpointCoord.x < 0.0f || viewpointCoord.x > 1.0f)
        {
            TakeDamage();
        }
    }

    private void OnPlayerMoved(Vector2 vector)
    {
        //if (transform.parent != null)
        //{
        //    if (transform.parent.GetComponent<IObstacle>() is not null)
        //    {
        //        transform.SetParent(null);
        //    }
        //}

        var delta = vector.normalized * jumpLength;
        transform.position += (Vector3)delta;
        FrogMoved?.Invoke();
    }
    
    public void TakeDamage()
    {
        Debug.Log("Frog taking damage!");

        // Vector2 newStartPoint = Vector2.zero;
        //
        // if (frogSpawner != null)
        // {
        //     newStartPoint = frogSpawner.GetSpawnPoint();
        // }
        //
        // transform.position = newStartPoint;

        Kill();
    }

    private void Kill()
    {
        Remove();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.4f);
    }

    public void Remove()
    {
        Destroy(gameObject);
        FromRemoved?.Invoke();
    }
}