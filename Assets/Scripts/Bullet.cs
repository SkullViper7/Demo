using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public Rigidbody rb;

    private void Update()
    {
        if (!base.IsOwner)
        {
            return;
        }
    }

    //Freeze bullet when touching ground
    private void OnCollisionEnter(Collision other)
    {
       if (other.gameObject.tag == "Ground")
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
    }
}
