using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Oculus.Avatar;

public class NetworkPlayer : NetworkBehaviour {

    [SerializeField]
    private GameObject charTrackingSpace;
    [SerializeField]
    private CharacterController charController;
    [SerializeField]
    private OVRPlayerController charOVRController;
    [SerializeField]
    private OvrAvatar charAvatar;
    [SerializeField]
    private OVRCameraRig charCamera;
    [SerializeField]
    private OVRManager charOVRManager;


    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            charTrackingSpace.SetActive(true);
            charController.enabled = true;
            charOVRController.enabled = true;
            charAvatar.enabled = true;
            charCamera.enabled = true;
            charOVRManager.enabled = true;
        }
	}

}
