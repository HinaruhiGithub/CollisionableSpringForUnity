using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceAtOnce : MonoBehaviour
{
    private Rigidbody2D _mRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        _mRigidBody = GetComponent<Rigidbody2D>();
        
        _mRigidBody.AddForce(Vector2.right * 10f);
    }
}
