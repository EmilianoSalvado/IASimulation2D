using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingBehaviour : SteeringBehaviour, IPredatorUpdate
{
    public ChasingBehaviour(Transform t, float ms, float ar) : base(t, ms, ar) {}

    public void ArtificialUpdate(Predator predator)
    {
        Avoidance(predator);

        predator.sr.color = predator.chasing;

        if (predator.LookForPrey() == null)
        { predator.stateMachine.ChangeState(PredatorStates.Patrolling); return; }

        AddForce(Pursuit(predator.LookForPrey()));

        predator.SpendStamina();

        if (predator.CheckMinStamina())
            predator.stateMachine.ChangeState(PredatorStates.Resting);

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
