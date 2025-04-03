
using UnityEngine;

public class BowlingPinBehaviour : MonoBehaviour
{
   [SerializeField] public bool isStanding { get; private set; } 
    public bool isOnLane{ get; private set;}

    
    private void Start()
    {
        isStanding = true;
        //Vector3 rotation = transform.eulerAngles;
        //Vector3.Dot(rotation, Vector3.up);
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BowlingLane") && isStanding)
        {
            Debug.Log("Fallen Over");
            isStanding = false;
        }
    }
}


