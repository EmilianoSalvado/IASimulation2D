using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restrictor : MonoBehaviour
{
    [SerializeField] float _heightDistance;
    [SerializeField] float _widthDistance;

    public float HeightDistance { get { return _heightDistance; } }
    public float WidthDistance { get { return _widthDistance; } }

    public static Restrictor instance;

    [SerializeField] FoodSpawner _foodSpawner;

    private void Start()
    {
        instance = this;
        _foodSpawner.FakeStart();
    }

    public Vector3 Loop (Vector3 position)
    {
        if (position.x > _widthDistance)
            position.x = -_widthDistance;
        if (position.x < -_widthDistance)
            position.x = _widthDistance;
        if (position.y > _heightDistance)
            position.y = -_heightDistance;
        if (position.y < -_heightDistance)
            position.y = _heightDistance;

        return position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-_widthDistance, _heightDistance), new Vector3(_widthDistance, _heightDistance));
        Gizmos.DrawLine(new Vector3(-_widthDistance, -_heightDistance), new Vector3(_widthDistance, -_heightDistance));
        Gizmos.DrawLine(new Vector3(-_widthDistance, -_heightDistance), new Vector3(-_widthDistance, _heightDistance));
        Gizmos.DrawLine(new Vector3(_widthDistance, -_heightDistance), new Vector3(_widthDistance, _heightDistance));
    }
}
