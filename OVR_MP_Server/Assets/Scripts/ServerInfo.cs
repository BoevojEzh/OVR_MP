using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerInfo : NetworkBehaviour {

	// Use this for initialization
	void Start ()
    {
        GameObject.Find("TextServerIP").transform.GetComponent<Text>().text = NetworkManager.singleton.networkAddress + ":" + NetworkManager.singleton.networkPort;
    }
	
	// Update is called once per frame
	void Update () {


    }


}
