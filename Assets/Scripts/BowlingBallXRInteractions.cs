using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class BowlingBallXRInteractions : MonoBehaviour
{
    [SerializeField] private float rotationScalar = 2f;
    [SerializeField] private Rigidbody body;
    public InputDevice rightController, leftController;
    [SerializeField] private Transform handHold;
    private bool applyForce;
    //private NearFarInteractor rightHandCheck, leftHandCheck;

    public enum HandType { LeftHand, RightHand, None}
    private HandType currentHand;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();
        //handHold = GetComponentInChildren<Transform>();
        if (!rightController.isValid) InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref rightController);
        if (!leftController.isValid) InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref leftController);

        //leftHandCheck = GameObject.FindGameObjectWithTag("LeftController").GetComponent<NearFarInteractor>();
        //rightHandCheck = GameObject.FindGameObjectWithTag("RightController").GetComponent<NearFarInteractor>();

        //Debug.Log(leftHandCheck.name);
    }

    private void FixedUpdate()
    {
        if (applyForce) AlterBallVelocity();
    }


    private void InitializeInputDevices()
    {
        if (!rightController.isValid ) InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref rightController);
        if (!leftController.isValid) InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref leftController);
    } 

    private void InitializeInputDevice (InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);
        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
    }

    public void OnGrabbed()
    {
        //SetHand().TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out Quaternion rotR);
        Debug.Log(handHold.eulerAngles);
    }

    public void ReleaseBall()
    {

        body.isKinematic = false;
        applyForce = true;
        //AlterBallVelocity();
        
    }

    private void AlterBallVelocity() 
    {
        body.velocity = new Vector3(body.velocity.x, Mathf.Clamp(body.velocity.y, 0f, 100f), body.velocity.z);
        body.AddTorque(transform.forward * (SetZRotation() * rotationScalar), ForceMode.Impulse);
        Debug.Log(body.velocity + " " + body.angularVelocity);
        applyForce = false;
    }

    private float SetZRotation () 
    {

        return Mathf.DeltaAngle(0, handHold.eulerAngles.z);    
    }

    //private float SetHandZRotation(InputDevice hand)
    //{

    //    hand.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation); 
        
    //    Debug.Log(rotation);
    //    if (rotation.x > 1)
    //        return 1;
    //    else if (rotation.x < -1)
    //        return -1;
    //    else
    //        return rotation.x;
    //}

    //private UnityEngine.XR.InputDevice SetHand()

    //{
    //    CheckHand();
    //    if (currentHand == HandType.LeftHand)
    //        return leftController;
    //    else if (currentHand == HandType.RightHand)
    //        return rightController;

    //    return new UnityEngine.XR.InputDevice();
    //    }

    //    private void CheckHand()
    //    {
    //    if (rightHandCheck.attachTransform.CompareTag("BowlingBall")) currentHand = HandType.RightHand;
    //    else if (leftHandCheck.attachTransform.CompareTag("BowlingBall"))currentHand = HandType.LeftHand;
    //    else currentHand = HandType.None;
    //    }

    }
