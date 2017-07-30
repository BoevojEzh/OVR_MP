using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSwitcher : NetworkBehaviour {

    private Light nLight;
    
    private bool nLightState;
    private string switchName;

    private void OnTriggerEnter(Collider other)
    {
        if (isLocalPlayer)
        {
            if (other.GetComponent<Collider>().tag == "Switcher")
            {
                CmdSwitchLight(!nLight.enabled, other.name);
            }
        }
            

    }


    // Use this for initialization
    void Start ()
    {
        nLight = GameObject.FindGameObjectWithTag("Spotlight").GetComponent<Light>();
        if (!isServer)
        {
            CmdRequestLightState();
        }
    }

    [Command]
    private void CmdRequestLightState()
    {
        if(isServer)
        {
            RpcSwitchLight(nLight.enabled, "Switcher1");
        }
    }

    [Command]
    private void CmdSwitchLight(bool newLightState, string colName)
    {
        if (isServer)
        {
            if (newLightState != nLight.enabled)
            {
                switchName = colName;
                nLightState = newLightState;
                SwitchLight(nLightState, switchName);
                RpcSwitchLight(nLightState, switchName);
            }
        }
    }

    [ClientRpc]
    private void RpcSwitchLight(bool newLightState, string colName)
    {
        if(!isServer)
        {
            SwitchLight(newLightState, colName);
        }
    }

    private void SwitchLight(bool newLightState, string colName)
    {
        
        nLight.enabled = newLightState;
        switch (nLight.enabled)
        {
            case true:
                {
                    foreach (GameObject sw in GameObject.FindGameObjectsWithTag("Switcher"))
                    {
                        sw.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/mSwitcherOff");
                    }
                    GameObject.Find(colName).GetComponent<Renderer>().material = Resources.Load<Material>("Materials/mSwitcherOn");
                    break;
                }
            case false:
                {
                    foreach (GameObject sw in GameObject.FindGameObjectsWithTag("Switcher"))
                    {
                        sw.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/mSwitcherOff");
                    }
                    break;
                }
        }
    }


}
