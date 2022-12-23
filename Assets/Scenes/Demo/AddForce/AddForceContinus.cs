using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceContinus : MonoBehaviour
{
    private Rigidbody2D _mRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        _mRigidBody = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        _mRigidBody.AddForce(Vector2.right * 10f);
    }
}
