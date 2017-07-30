using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class NetworkManager_custom : NetworkManager
{


    public void JoinGame()
    {
        SetIPAddress();
        SetPort();

        NetworkManager.singleton.StartClient();
    }

    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.Find("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    void SetPort()
    {
        int port = int.Parse(GameObject.Find("InputFieldPort").transform.Find("Text").GetComponent<Text>().text);
        NetworkManager.singleton.networkPort = port;
    }
}
