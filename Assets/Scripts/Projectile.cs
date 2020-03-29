using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public GameObject SplashVisualizer;

    public GameObject BoomVisualizer;

    public void Update()
    {
        Bounds bounds = new Bounds(); ;
        var rends = GetComponentsInChildren<Renderer>();
        foreach (var r in rends)
        {
            bounds.Encapsulate(r.bounds);
        }
        if(transform.position.y + bounds.extents.y < 0)
        {
            MakeSplash(transform.position);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform);
        if(collision.rigidbody.transform.GetComponent<Damagable>() != null)
        {
            MakeBoom(collision.contacts[0].point);
        }
        else
        {
            MakeSplash(collision.contacts[0].point);
        }
        Destroy(gameObject);
    }

    void MakeBoom(Vector3 pos)
    {
        if(BoomVisualizer != null)
        {
            var newBoom = GameObject.Instantiate(BoomVisualizer);
            newBoom.transform.position = pos;
        }
    }
    
    void MakeSplash(Vector3 pos)
    {
        if(SplashVisualizer != null)
        {
            var newSplash = GameObject.Instantiate(SplashVisualizer);
            newSplash.transform.position = pos;
        }
    }
}
