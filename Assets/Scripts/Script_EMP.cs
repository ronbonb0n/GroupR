using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Script_EMP : MonoBehaviour
{

    public float delay = 2f;
    private float countdown;
    public bool exploded = false;
    public SphereCollider Collider;
    //public GameObject Explosion_Effect;  For explosion effect


    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0f && !exploded)
        {
            Explode();
            exploded = true;
        }
    }

    void Explode()
    {
        //Instantiate(Explosion_Effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
