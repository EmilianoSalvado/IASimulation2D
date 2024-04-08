using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] GameObject _food;
    [SerializeField] float _cooldown;
    bool isPlaying = false;

    bool _auto = true;

    float h;
    float w;

    public void FakeStart()
    {
        h = Restrictor.instance.HeightDistance;
        w = Restrictor.instance.WidthDistance;
    }

    private void Update()
    {
        if (!isPlaying && _auto)
            StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        isPlaying = true;
        SpawnFood();
        yield return new WaitForSeconds(_cooldown);
        isPlaying = false;
    }

    public void SpawnFood()
    {
        Vector3 v = new Vector3(Random.Range(-w, w), Random.Range(-h, h), 0);
        Instantiate(_food, v, _food.transform.rotation);
    }

    public void SwitchAuto()
    {
        _auto = !_auto;
    }
}
