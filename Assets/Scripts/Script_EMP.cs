using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Threading.Tasks;

public class Script_EMP : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    private AudioSource audioSource;
    public float delay = 2f;
    private float countdown;
    public bool exploded = false;
    public SphereCollider Collider;
    public float radius = 20;

    public GameObject explosionEffect;


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
        if(countdown <= 0f && !exploded)
        {
            Explode();
            exploded = true;
        }
    }
    private AudioClip GetClip()
    {
        return clips[(0)];
    }
    async void Explode()
    {
        transform.Find("EMP_Grenade").gameObject.SetActive(false);
        AudioClip clip = GetClip();
        audioSource.PlayOneShot(clip);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in colliders)
        {
            GameObject hitObject = c.gameObject;
            if (hitObject.CompareTag("Drone"))
            {
                hitObject.GetComponentInParent<DroneController>().isStunned = true;
            }
        }
        Vector3 spawn_location = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

        explosionEffect = Instantiate(explosionEffect, spawn_location, Quaternion.Euler(0, 0, 0)) as GameObject;

        await Task.Delay((int)(2 * 1000));
        if (gameObject != null)
        {
            Destroy(gameObject);
            Destroy(explosionEffect);
        }
    }
}
