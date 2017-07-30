using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NetworkManager_custom : NetworkManager
{


    public void StartupServer()
    {
        SetPort();
        NetworkManager.singleton.StartServer();
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
