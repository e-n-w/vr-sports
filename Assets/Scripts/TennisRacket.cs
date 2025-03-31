using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisRacket : MonoBehaviour
{
    public int PlayerId;

    private Rigidbody rb;
    [SerializeField]
    private GameObject rightHand;
    [SerializeField]
    private GameObject visuals;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rightHand.transform.position);
        rb.MoveRotation(rightHand.transform.rotation);
    }

    private void LateUpdate()
    {
        visuals.transform.position = rightHand.transform.position;
    }
}
