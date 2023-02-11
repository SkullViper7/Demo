using FishNet.Component.Observing;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Helping;
using FishNet.Observing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    //bullet
    public Rigidbody rb;

    //public GameObject shooter;

    public bool canTP = false;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            return;
        }
    }

    //private void Start()
    //{
    //    shooter = gameObject.GetComponent<PlayerMovement>().gameObject;
    //}

    //Freeze bullet when touching ground
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            canTP = true;
        }
    }
}
