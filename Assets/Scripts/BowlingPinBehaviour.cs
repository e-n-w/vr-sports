
using UnityEngine;

public class BowlingPinBehaviour : MonoBehaviour
{
      
    [SerializeField] private Rigidbody[] shards;
    [SerializeField] private float explosionForce = .0000005f;
    [SerializeField] private float explosionRadius = .01f;
    [SerializeField] private float upwardsModifier = -1f;
    private MeshRenderer _objectRenderer;
    public bool isOnLane{ get; private set;}
    public bool isStanding { get; private set; }

    
    private void Start()
    {
        isStanding = true;
        _objectRenderer = GetComponent<MeshRenderer>();
        // Initialize shards as kinematic and hidden
        shards = GetComponentsInChildren<Rigidbody>(true);
        Debug.Log(shards.Length);
        
        foreach (Rigidbody shard in shards)
        {
            shard.isKinematic = true;
            shard.GetComponent<Renderer>().enabled = false;
        }
        gameObject.AddComponent<Rigidbody>();
        //Vector3 rotation = transform.eulerAngles;
        //Vector3.Dot(rotation, Vector3.up);
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BowlingLane") && isStanding)
        {
            Debug.Log("Fallen Over");
            isStanding = false;
            Explode();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Explode();
        }
    }

    public void Explode()
    {
        
        _objectRenderer.enabled = false;
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<MeshCollider>());

        foreach (Rigidbody shard in shards)
        {
            //if (shard.transform == transform) continue;
            // Show the shard and enable physics
            shard.gameObject.SetActive(true);
            shard.GetComponent<Renderer>().enabled = true;
            shard.isKinematic = false;
            // Apply explosion force from the object's center
            shard.AddExplosionForce(
                explosionForce,
                transform.position,
                explosionRadius,
                upwardsModifier
            );
        }
    }
}


