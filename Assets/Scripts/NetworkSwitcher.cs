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

    // Update is called once per frame
    void Update()
    {
        if (this.isServer)
        {
            //if ((timeRpcLast + periodSndRpc < Time.time) && (switchName != null) )
            //{
            //    RpcSwitchLight(nLightState, switchName);
            //    timeRpcLast = Time.time;
            //}
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
            //Debug.Log(colName);
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
                   GameObject.Find(colName).GetComponent<Renderer>().material = Resources.Load<Material>("Materials/mSwitcherOn");
                    break;
                }
            case false:
                {
                    GameObject.Find(colName).GetComponent<Renderer>().material = Resources.Load<Material>("Materials/mSwitcherOff");
                    break;
                }
        }
    }


}
