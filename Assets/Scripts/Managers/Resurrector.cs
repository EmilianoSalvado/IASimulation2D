using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrector : MonoBehaviour
{
    [SerializeField] float _resurrectionTime;

    public static Resurrector instance;

    List<Boid> boids = new List<Boid>();

    private void Start()
    {
        instance = this;
    }

    public void Resurrect(Boid b)
    {
        if (!boids.Contains(b))
            boids.Add(b);
        else
        {
            boids.Remove(b);
            boids.Add(b);
        }

        StartCoroutine("Respawn");
    }

    IEnumerator Respawn()
    {
        int a = boids.Count - 1;
        boids[a].gameObject.SetActive(false);
        yield return new WaitForSeconds(_resurrectionTime);
        boids[a].gameObject.SetActive(true);
    }
}
