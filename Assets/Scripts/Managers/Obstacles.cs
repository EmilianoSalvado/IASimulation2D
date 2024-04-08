using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] GameObject[] _obstacles;

    int _count = 0;

    public void Switch()
    {
        _count++;

        if (_count >= _obstacles.Length)
            _count = 0;

        for (int i = 0; i < _obstacles.Length; i++)
        {
            _obstacles[i].SetActive(i == _count);
        }
    }
}