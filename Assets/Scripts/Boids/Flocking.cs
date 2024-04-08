using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : SteeringBehaviour
{
    public Flocking(Transform t, float ms, float ar) : base(t, ms, ar) {}

    public Vector3 Separation(HashSet<Boid> boids, float range)
    {
        if (boids.Count < 1) return Vector3.zero;

        Vector3 v = Vector3.zero;

        foreach (var item in boids)
        {
            if (Vector3.Distance(item.transform.position, transform.position) > range)
                continue;

            var a = item.transform.position - transform.position;
            a *= -1;
            v += a;
        }

        return v;
    }

    public Vector3 Cohesion(HashSet<Boid> boids)
    {
        if (boids.Count <= 3) return Vector3.zero;

        int count = 0;
        var v = Vector3.zero;

        foreach (var item in boids)
        {
            v += item.transform.position;
            count++;
        }

        v /= count;

        return v;
    }

    public Vector3 Alignment(HashSet<Boid> boids)
    {
        if (boids.Count < 1) return Vector3.zero;

        int count = 0;
        var v = Vector3.zero;

        foreach(var item in boids)
        {
            v += item.flocking.Vel;
            count++;
        }

        v /= count;

        return v;
    }
}
