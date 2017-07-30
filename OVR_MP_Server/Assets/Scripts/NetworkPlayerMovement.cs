using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerMovement : NetworkBehaviour{


    [SerializeField]
    private float posLerpRate = 15;
    [SerializeField]
    private float rotLerpRate = 15;
    [SerializeField]
    private float posThreshold = 0.1f;
    [SerializeField]
    private float rotThreshold = 1f;
    [SerializeField]
    private float periodSndRpc = 0.05f;

    private float timeRpcLast = 0;

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (this.isLocalPlayer)
        {
            if (IsPositionChanged())
            {
                CmdSendPosition(transform.position);
            }

            if (IsRotationChanged())
            {
                CmdSendRotation(transform.rotation);
            }

             
        }


    }

    private void FixedUpdate()
    {



        if (this.isServer)
        {
            if (timeRpcLast + periodSndRpc < Time.time)
            {
                RpcUpdatePosition(lastPosition);
                RpcUpdateRotation(lastRotation);
                timeRpcLast = Time.time;
            }

        }


    }


    [Command(channel = Channels.DefaultUnreliable)]
    private void CmdSendPosition(Vector3 pos)
    {
        if (this.isServer)
        {
            //здесь можно сделать проверку на валидность движения
            lastPosition = pos;
            InterpolatePosition(pos);
        }
    }

    [Command(channel = Channels.DefaultUnreliable)]
    private void CmdSendRotation(Quaternion rot)
    {
        if (this.isServer)
        {
            lastRotation = rot;
        }
    }

    [ClientRpc(channel = Channels.DefaultUnreliable)]
    private void RpcUpdatePosition(Vector3 posNew)
    {
        if (this.isClient)
        {
            // this.transform.position = posNew;
            InterpolatePosition(posNew);
        }
    }

    [ClientRpc(channel = Channels.DefaultUnreliable)]
    private void RpcUpdateRotation(Quaternion rotNew)
    {
        if (this.isClient)
        {
            //this.transform.rotation = rotNew;
            InterpolateRotation(rotNew);
        }
    }

    private bool IsPositionChanged()
    {
        return Vector3.Distance(transform.position, lastPosition) > posThreshold;
    }

    private bool IsRotationChanged()
    {
        return Quaternion.Angle(transform.rotation, lastRotation) > rotThreshold;
    }


    private void InterpolatePosition(Vector3 LerpPos)
    {
        transform.position = Vector3.Lerp(transform.position, LerpPos, Time.deltaTime * posLerpRate);
    }

    private void InterpolateRotation(Quaternion LerpRot)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, LerpRot, Time.deltaTime * rotLerpRate);
    }


    public override int GetNetworkChannel()
    {
        return Channels.DefaultUnreliable;
    }

    public override float GetNetworkSendInterval()
    {
        return 0.01f;
    }


   

}
