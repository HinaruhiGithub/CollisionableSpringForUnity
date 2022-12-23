using Scenes.Scripts.System;
using UnityEngine;

public class OneObject_CollisionableSpring : MonoBehaviour
{
    [SerializeField] private Transform leftStaticPoint;
    [SerializeField] private Transform rightStaticPoint;
    [SerializeField] private float myDampingRatio;
    [SerializeField] private float leftMagnitude;
    [SerializeField] private float rightMagnitude;
    [SerializeField] private float myMagnitude;
    private float _leftNaturalLength;
    private float _rightNaturalLength;
    private float _myNaturalLength;
    private Transform _myTransform;
    private Rigidbody2D _rigidbody;
    private Vector2 _pastLeftVelocity;
    private Vector2 _pastRightVelocity;

    private void Awake()
    {
        _myTransform = transform;
        _leftNaturalLength = GetLeftLength();
        _rightNaturalLength = GetRightLength();
        _myNaturalLength = _myTransform.localScale.x;
        _rigidbody = GetComponent<Rigidbody2D>();
        _pastLeftVelocity = Vector2.zero;
        _pastRightVelocity = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        var deltaTime = Time.deltaTime;
        var leftPower = GetLeftPower() + GetInnerLeftPower() + GetLeftDampingPower();
        var rightPower = GetRightPower() + GetInnerRightPower() + GetRightDampingPower();
        var leftAcceleration = leftPower / _rigidbody.mass / 2f;
        var rightAcceleration = rightPower / _rigidbody.mass / 2f;
        var leftVelocity = _pastLeftVelocity + leftAcceleration * deltaTime;
        var rightVelocity = _pastRightVelocity + rightAcceleration * deltaTime;
        var leftPosition = GetLeftPosition() + leftVelocity * deltaTime;
        var rightPosition = GetRightPosition() + rightVelocity * deltaTime;
        
        Debug.Log(_pastLeftVelocity);
        Debug.Log(_pastRightVelocity);
        
        UpdateTransformByTwoPoints(leftPosition, rightPosition);
        _pastLeftVelocity = leftVelocity;
        _pastRightVelocity = rightVelocity;
    }

    /// <summary>
    /// 左側にかかる自身のばねの力を計算する
    /// </summary>
    /// <returns></returns>
    private Vector2 GetInnerLeftPower()
    {
        Vector2 vector = GetRightPosition() - GetLeftPosition();
        var direction = vector.normalized;
        var magnitude = myMagnitude * (_myTransform.localScale.x - _myNaturalLength);
        return magnitude * direction;
    }

    /// <summary>
    /// 右側にかかる自身のばねの力を計算する
    /// </summary>
    /// <returns></returns>
    private Vector2 GetInnerRightPower()
    {
        Vector2 vector = GetLeftPosition() - GetRightPosition();
        var direction = vector.normalized;
        var magnitude = myMagnitude * (_myTransform.localScale.x - _myNaturalLength);
        return magnitude * direction;
    }

    /// <summary>
    /// 左の減衰力を計算する
    /// </summary>
    /// <returns></returns>
    private Vector2 GetLeftDampingPower()
    {
        return -_pastLeftVelocity * myDampingRatio;
    }

    /// <summary>
    /// 右の減衰力を計算する
    /// </summary>
    /// <returns></returns>
    private Vector2 GetRightDampingPower()
    {
        return -_pastRightVelocity * myDampingRatio;
    }

    
    /// <summary>
    /// 左側の力を計算する
    /// </summary>
    /// <returns></returns>
    private Vector2 GetLeftPower()
    {
        var position = GetLeftPosition();
        var vector = (Vector2)leftStaticPoint.position - position;
        var direction = vector.normalized;
        var magnitude = leftMagnitude * (vector.magnitude - _leftNaturalLength);
        return magnitude * direction;
    }
    
    /// <summary>
    /// 右側の力を計算する
    /// </summary>
    /// <returns></returns>
    private Vector2 GetRightPower()
    {
        var position = GetRightPosition();
        var vector = (Vector2)rightStaticPoint.position - position;
        var direction = vector.normalized;
        var magnitude = rightMagnitude * (vector.magnitude - _rightNaturalLength);
        return magnitude * direction;
    }
    
    /// <summary>
    /// 2点座標からTransformを更新する
    /// </summary>
    private void UpdateTransformByTwoPoints(
        Vector2 left, Vector2 right
    )
    {
        _myTransform.position = (left + right) / 2f;
        _myTransform.eulerAngles = Mathf.Atan2(right.y - left.y, right.x - left.x)
                                   * Vector3.forward;
        _myTransform.localScale = 
            new Vector3(
            (right - left).magnitude, 
            1f, 
            1f);
    }

    /// <summary>
    /// 左ばねの長さを取得
    /// </summary>
    /// <returns></returns>
    private float GetLeftLength()
    {
        return (GetLeftPosition() - (Vector2)leftStaticPoint.position).magnitude;
    }

    /// <summary>
    /// 右ばねの長さを取得
    /// </summary>
    /// <returns></returns>
    private float GetRightLength()
    {
        return (GetRightPosition() - (Vector2)rightStaticPoint.position).magnitude;
    }
    
    /// <summary>
    /// 棒の左側の座標の取得
    /// </summary>
    /// <returns></returns>
    private Vector2 GetLeftPosition()
    {
        Vector2 basePosition = _myTransform.position;
        var leftLength = - _myTransform.localScale.x / 2f;
        var rad = _myTransform.eulerAngles.z * Mathf.Deg2Rad;

        return basePosition + 
               new Vector2(
                   leftLength * Mathf.Cos(rad),
                   leftLength * Mathf.Sin(rad)
                   );
    }
    
    /// <summary>
    /// 棒の右側の座標の取得
    /// </summary>
    /// <returns></returns>
    private Vector2 GetRightPosition()
    {
        Vector2 basePosition = _myTransform.position;
        var leftLength = _myTransform.localScale.x / 2f;
        var rad = _myTransform.eulerAngles.z * Mathf.Deg2Rad;

        return basePosition + 
               new Vector2(
                   leftLength * Mathf.Cos(rad),
                   leftLength * Mathf.Sin(rad)
               );
    }
    
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        _myTransform.position = 
            other.transform.position + Vector3.down * 0.6f;
        var power = GetLeftPower() + GetRightPower();
        other.GetComponent<Rigidbody2D>().AddForce(power);
    }*/

    private void OnTriggerStay2D(Collider2D other)
    {
        _myTransform.position = other.transform.position;
        var myDegree = _myTransform.rotation.z + 90f;
        var leftPower = GetLeftPower().GetInnerProductWithDirection2D(myDegree);
        var rightPower = GetRightPower().GetInnerProductWithDirection2D(myDegree);
        var power = leftPower + rightPower;
        other.GetComponent<Rigidbody2D>().AddForce(power);
    }
    
    private void MoveMyselfByObject(Vector2 force)
    {
        var deltaTime = Time.deltaTime;
        var impulse = force * deltaTime;
        
        
    }
}
