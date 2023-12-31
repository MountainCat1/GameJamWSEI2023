using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, IObstacle
{
    private Vector2 _destination;
    private float _speed;
    private float _acceleration;

    public void Initialize(Vector3 destination, float speed = 1f, float acceleration = 1f)
    {
        _acceleration = acceleration;
        _speed = speed;
        _destination = destination;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Trigger");
        //var frog = col.gameObject.GetComponent<Frog>();
        //if (frog is null)
        //    return;

        //var locPos = transform.InverseTransformPoint(frog.transform.position);

        //frog.transform.SetParent(transform, false);
        //frog.transform.localPosition = locPos;
    }

    private void Update()
    {
        _speed += _acceleration * Time.deltaTime;


        float step = Time.deltaTime * _speed;
        transform.position = Vector2.MoveTowards(transform.position, _destination, step);


        if (Vector2.Distance(transform.position, _destination) <= 0.01f)
        {
            var frog = transform.GetComponentInChildren<Frog>();
                    
            if(frog is not null)
            {
                frog.transform.SetParent(null);                
                frog.TakeDamage();
            }
            
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {

    }
}
