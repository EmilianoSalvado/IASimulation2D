using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingBehaviour : SteeringBehaviour, IPredatorUpdate
{
    float time = 0;
    Predator _predator;
    Vector3 _position;

    public RestingBehaviour(Transform t, float ms, float ar) : base(t, ms, ar) {}

    public void ArtificialUpdate(Predator predator)
    {
        Avoidance(predator);

        predator.sr.color = predator.resting;

        if (time < predator.restingTime)
        {
            time += Time.deltaTime;
            return;
        }

        time = 0;
        predator.stateMachine.ChangeState(PredatorStates.Patrolling);
    }

    public void ArtificialFixed(Rigidbody2D rb)
    {
        return;
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
