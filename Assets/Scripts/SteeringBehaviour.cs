using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SteeringBehaviour
{
    public Transform transform { get; protected set; }
    protected float _maxSpeed;
    protected float _arriveRadius;

    protected Vector3 _vel;
    public Vector3 Vel { get { return _vel; } }

    public SteeringBehaviour(Transform t, float ms, float ar)
    { 
        transform = t;
        _maxSpeed = ms;
        _arriveRadius = ar;
        
        _vel = Vector3.zero;
    }

    public void AddForce(Vector3 v)
    {
        _vel = Vector3.ClampMagnitude(_vel + v, _maxSpeed);
    }

    public Vector3 Seek(Vector3 target)
    {
        var result = Calculate((target - transform.position).normalized);
        return result;
    }

    public Vector3 Calculate(Vector3 v)
    {
        var result = Vector3.ClampMagnitude(v - _vel, _maxSpeed);
        return result;
    }

    public Vector3 Arrive(Vector3 target)
    {
        var d = Vector3.Distance(transform.position, target);

        if (d > _arriveRadius)
            return Seek(target);

        var result = (target - transform.position).normalized;
        result *= _maxSpeed * (d / _arriveRadius);

        return Seek(result);
    }

    public Vector3 Pursuit(SteeringBehaviour targetSB)
    {
        var v = targetSB.transform.position + targetSB.Vel;

        var d = Vector3.Distance(transform.position, targetSB.transform.position);
        d *= d;

        if (d < targetSB.Vel.sqrMagnitude)
            return Seek(targetSB.transform.position);

        var result = Seek(v);
        return result;
    }

    public Vector3 Evade(Vector3 target)
    {
        var result = -Seek(target);
        return result;
    }
}