using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisRacket : MonoBehaviour
{
    public int PlayerId;

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
