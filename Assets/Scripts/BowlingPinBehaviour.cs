
using UnityEngine;

public class BowlingPinBehaviour : MonoBehaviour
{
   [SerializeField] public bool isStanding { get; private set; } 
    public bool isOnLane{ get; private set;}

    private void Start()
    {
        isStanding = true;
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


