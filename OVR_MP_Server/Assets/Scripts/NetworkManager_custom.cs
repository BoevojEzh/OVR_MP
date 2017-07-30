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
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.maxConnections = 4;
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


    public override void OnServerConnect(NetworkConnection conn)
    {
        string playersCount = (NetworkManager.singleton.numPlayers + 1).ToString();
        GameObject.Find("TextPlayers").transform.GetComponent<Text>().text = playersCount;
        GameObject.Find("ServerInfoPanel").transform.Find("Player"+ playersCount +"Info").gameObject.SetActive(true);
        GameObject.Find("ServerInfoPanel").transform.Find("Player" + playersCount + "Info").gameObject.transform.Find("PlayerText").GetComponent<Text>().text = conn.address; //returns IPv6 address instead of IPv4
        GameObject.Find("ServerInfoPanel").transform.Find("Player" + playersCount + "Info").gameObject.name = "Player" + conn.connectionId + "Info";
        Debug.Log("Player" + conn.connectionId + "Info");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        string playersCount = (NetworkManager.singleton.numPlayers + 1).ToString();
        GameObject.Find("ServerInfoPanel").transform.Find("Player" + conn.connectionId + "Info").gameObject.SetActive(false);
        GameObject.Find("TextPlayers").transform.GetComponent<Text>().text = (NetworkManager.singleton.numPlayers).ToString();

        Destroy(conn.playerControllers[0].gameObject);
        NetworkServer.Destroy(conn.playerControllers[0].gameObject);

    }


}
