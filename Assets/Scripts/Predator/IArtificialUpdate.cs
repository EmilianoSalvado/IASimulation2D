using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPredatorUpdate
{
    void ArtificialUpdate(Predator predator);
    void ArtificialFixed(Rigidbody2D rb);
    void Avoidance(Predator predator);
}
