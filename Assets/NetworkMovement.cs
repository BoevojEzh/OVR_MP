using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{


    [SerializeField]
    private float posLerpRate = 15;
    [SerializeField]
    private float rotLerpRate = 15;
    [SerializeField]
    private float posThreshold = 0.1f;
    [SerializeField]
    private float rotThreshold = 1f;

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
            return;
        InterpolatePosition();
        InterpolateRotation();
    }

    private void FixedUpdate()
    {
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

        if (this.isServer)
        {

            RpcUpdatePosition(lastPosition);
            RpcUpdateRotation(lastRotation);

        }

    }


    [Command(channel = Channels.DefaultUnreliable)]
    private void CmdSendPosition(Vector3 pos)
    {
        if (this.isServer)
        {
            //здесь можно сделать проверку на валидность движения
            lastPosition = pos;
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
            this.transform.position = posNew;
        }
    }

    [ClientRpc(channel = Channels.DefaultUnreliable)]
    private void RpcUpdateRotation(Quaternion rotNew)
    {
        if (this.isClient)
        {
            this.transform.rotation = rotNew;
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


    private void InterpolateRotation()
    {
        transform.position = Vector3.Lerp(transform.position, lastPosition, Time.deltaTime * posLerpRate);
    }

    private void InterpolatePosition()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, lastRotation, Time.deltaTime * rotLerpRate);
    }

}
