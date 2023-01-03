using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Threading.Tasks;
using System;
public class Script_Decoy : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    private AudioSource audioSource;
    public float delay = 2f;
    private float countdown;
    public bool attention = false;
    public SphereCollider Collider;
    public float radius = 20;
    //public GameObject Explosion_Effect;  For explosion effect


    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
    private AudioClip GetClip()
    {
        return clips[(0)];
    }
    async void Explode()
    {
        //Instantiate(Explosion_Effect, transform.position, transform.rotation);
        AudioClip clip = GetClip();
        audioSource.PlayOneShot(clip);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in colliders)
        {
            GameObject hitObject = c.gameObject;
            if (hitObject.CompareTag("Drone"))
            {
                Senses s = hitObject.GetComponent<Senses>();
                s.isAttracted = true;
                s.lastSpottedPlayerAt = transform.position;
            }
        }
        await Task.Delay((int)(7 * 1000));
        Destroy(gameObject);
    }
}
