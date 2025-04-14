using UnityEngine;

public class TennisRacket : MonoBehaviour
{
    public TennisPlayer Player { get; set; }

    new private Rigidbody rigidbody;
    [SerializeField]
    private GameObject rightHand;
    [SerializeField]
    private GameObject visuals;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rightHand.transform.position);
        rigidbody.MoveRotation(rightHand.transform.rotation);
    }

    private void LateUpdate()
    {
        visuals.transform.position = rightHand.transform.position;
    }
}
