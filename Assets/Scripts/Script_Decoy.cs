using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Threading.Tasks;
public class Script_Decoy : MonoBehaviour
{

    public float delay = 3f;
    private float countdown;
    public bool attention = false;
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
        if (countdown <= 0f && !attention)
        {
            Explode();
            attention = true;
        }
    }

    async void Explode()
    {
        //Instantiate(Explosion_Effect, transform.position, transform.rotation);
        await Task.Delay((int)(5 * 1000));
        Destroy(gameObject);
    }
}