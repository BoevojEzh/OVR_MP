using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSwitcher : NetworkBehaviour {

    [SerializeField]
    private Light nLight;
    [SerializeField]
    private float periodSndRpc = 0.05f;

    private float timeRpcLast = 0;

    private bool nLightState;


    private void OnTriggerEnter(Collider other)
    {

            if (other.GetComponent<Collider>().tag == "Player")
            {
                nLightState = !nLight.enabled;              
                CmdSwitchLight(nLightState);

            }

    }

    // Use this for initialization
    void Start () {
       // nLight.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isServer)
        {
            if (timeRpcLast + periodSndRpc < Time.time)
            {
                RpcSwitchLight(nLight.enabled);
                timeRpcLast = Time.time;
            }
        }
    }


    [Command]
    private void CmdSwitchLight(bool newLightState)
    {
        if (isServer)
        {
            if (newLightState != nLight.enabled)
            {
                // RpcSwitchLight(newLightState);
                Debug.Log(newLightState);
                SwitchLight(newLightState);
            }
        }
    }

    [ClientRpc]
    private void RpcSwitchLight(bool newLightState)
    {
        if(true)
        {
            SwitchLight(newLightState);
        }
    }

    private void SwitchLight(bool newLightState)
    {
        nLight.enabled = newLightState;
        switch (nLight.enabled)
        {
            case true:
                {
                    this.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/mSwitcherOn");
                    break;
                }
            case false:
                {
                    this.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/mSwitcherOff");
                    break;
                }
        }
    }


}
