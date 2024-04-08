using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance
{
    Transform transform;
    float _offset;
    float _distance;
    LayerMask _layerMask;

    RaycastHit2D _hit;
    Ray2D R;
    Ray2D L;

    Vector3 v;

    public ObstacleAvoidance(Transform t, float offset, float distance, LayerMask lm)
    {
        transform = t;
        _offset = offset;
        _distance = distance;
        _layerMask = lm;
    }

    public bool DidHit()
    {
        var ar = transform.position + transform.right * _offset;
        var al = transform.position - transform.right * _offset;
        R = new Ray2D(ar, al);

        var br = transform.position + transform.right * _offset + transform.up;
        var bl = transform.position - transform.right * _offset + transform.up;
        L = new Ray2D(bl, ar);

        if (Physics2D.Raycast(R.origin, R.direction, _distance, _layerMask))
            v = -transform.right;
        else if (Physics2D.Raycast(L.origin, L.direction, _distance, _layerMask))
            v = transform.right;
        else
            return false;

        return true;
    }

    public Vector3 GetVector()
    {
        return v;
    }
}
