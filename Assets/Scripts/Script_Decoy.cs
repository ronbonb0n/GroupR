using UnityEngine;
using System.Threading.Tasks;

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

    Material effectMaterial;
    public GameObject sonarEffect;
    public Color pulseColour = Color.green;
    public float ringMultiplier = 1f;
    public float pulseSpeed = 1f;

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
        transform.Find("Decoy_Grenade").gameObject.SetActive(false);
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

        effectMaterial = sonarEffect.gameObject.GetComponent<Renderer>().sharedMaterial;
        effectMaterial.SetColor("_PulseColour", pulseColour);
        effectMaterial.SetFloat("_PulseSpeed", pulseSpeed);
        effectMaterial.SetFloat("_Rings_Multiplier", ringMultiplier);

        Vector3 spawn_location = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

        sonarEffect = Instantiate(sonarEffect, spawn_location, Quaternion.Euler(-90,0,0)) as GameObject;
        // 9.49 is the dimensions of mesh
        sonarEffect.transform.localScale = new Vector3(radius/9.49f, radius/9.49f, 1); 

        await Task.Delay((int)(7 * 1000));

        if (gameObject != null)
        {
            Destroy(gameObject);
            Destroy(sonarEffect);
        }
    }
}
