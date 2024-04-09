using UnityEngine;
using System.Collections;
using Valve.VR;

[RequireComponent(typeof(SpringJoint))]
[RequireComponent(typeof(SteamVR_Behaviour_Pose))]
[RequireComponent(typeof(Collider))]
public class ViveSpringGrabber : Grabber
{
    public string grabAction = "GrabPinch";
    private SpringJoint joint;
    private HingeJoint hinge;
    public GameObject lid;
    private float hingeCheck;

    new void Start()
    {
        base.Start();
        joint = GetComponent<SpringJoint>();

        hingeCheck = hinge.angle;
    }

    protected override void Update()
    {
        if (joint.connectedBody == null && target != null && SteamVR_Input.GetStateDown(grabAction, controller.inputSource))
        {
            joint.connectedBody = target.GetComponent<Rigidbody>();

        }
        else if (joint.connectedBody != null && SteamVR_Input.GetStateUp(grabAction, controller.inputSource))
        {
            joint.connectedBody = null;
        }

        hinge = lid.GetComponent<HingeJoint>();

        CreakingFeedback();
    }

    public void CreakingFeedback()
    {
        print(hinge.angle);

        if (hinge.angle > hingeCheck+10 || hinge.angle < hingeCheck-10)
        {
            float duration = 0.1f;
            int frequency = 20;
            float strength = 0.75f;

            SteamVR_Actions.default_Haptic[controller.inputSource].Execute(0, duration, frequency, strength);

            hingeCheck = hinge.angle;

        }
    }
}
