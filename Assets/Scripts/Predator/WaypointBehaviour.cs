using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehaviour : SteeringBehaviour, IPredatorUpdate
{
    Transform[] _waypoints;

    public WaypointBehaviour(Transform t, float ms, float ar, Transform[] waypoints) : base(t, ms, ar)
    {
        _waypoints = waypoints;
    }

    int count = 0;

    public void ArtificialUpdate(Predator predator)
    {
        predator.sr.color = predator.patrolling;

        Avoidance(predator);

        if (Vector3.Distance(transform.position, _waypoints[count].position) < .5f)
        {
            count++;

            if (count >= _waypoints.Length)
                count = 0;
        }

        if (predator.LookForPrey() != null)
            predator.stateMachine.ChangeState(PredatorStates.Chasing);

        predator.SpendStamina();
        if (predator.CheckMinStamina())
            predator.stateMachine.ChangeState(PredatorStates.Resting);

        AddForce(Seek(_waypoints[count].position));

        transform.up = _vel;
    }

    public void ArtificialFixed(Rigidbody2D rb)
    {
        rb.MovePosition((transform.position += _vel) * (_maxSpeed * Time.fixedDeltaTime));
    }

    public void Avoidance(Predator predator)
    {
        if (predator.obstacleAvoidance.DidHit())
        {
            AddForce(predator.obstacleAvoidance.GetVector());
            return;
        }
    }
}
