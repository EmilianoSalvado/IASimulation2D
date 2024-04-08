using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] float _maxSpeed;
    [SerializeField] float _arriveRadius;
    [SerializeField] float _separationStrength;
    [SerializeField] float _cohesionStrength;
    [SerializeField] float _alignmentStrength;
    [SerializeField] Rigidbody2D _rb;

    public Flocking flocking { get; private set; }

    [SerializeField] HashSet<Boid> _boids;
    [SerializeField] LayerMask _boidsMask;
    [SerializeField] float _sightRadiusForFlockmates;
    [SerializeField] float _sightRadiusForPredator;
    [SerializeField] float _sightRadiusForFood;
    [SerializeField] float _separationRange;
    [SerializeField] LayerMask _foodMask;
    [SerializeField] LayerMask _predatorMask;

    [Header("ObstacleAvoidance")]
    [SerializeField] float _avoidanceStrength;
    [SerializeField] float _rayOffset;
    [SerializeField] float _rayDistance;
    [SerializeField] LayerMask _obstacleMask;
    ObstacleAvoidance _obstacleAvoidance;

    [Header("Colors")]
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] Color _alone;
    [SerializeField] Color _flocking;
    [SerializeField] Color _fleeing;

    private void Start()
    {
        _boids = new HashSet<Boid>();
        CalculateMates();

        flocking = new Flocking(transform, _maxSpeed, _alignmentStrength);
        flocking.AddForce(new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0));

        _obstacleAvoidance = new ObstacleAvoidance(transform, _rayOffset, _rayDistance, _obstacleMask);

        _sr.color = _alone;

        FishManager.instance.Add(this);
    }

    private void Update()
    {
        transform.up = flocking.Vel;

        transform.position = Restrictor.instance.Loop(transform.position);

        var csPredator = Physics2D.OverlapCircleAll(transform.position, _sightRadiusForPredator, _predatorMask);
        var csFood = Physics2D.OverlapCircleAll(transform.position, _sightRadiusForFood, _foodMask);

        if (_obstacleAvoidance.DidHit())
        {
            flocking.AddForce(flocking.Seek(_obstacleAvoidance.GetVector() * _avoidanceStrength));
            return;
        }

        if (csPredator.Length > 0)
        {
            var a = csPredator[0].transform.position;
            flocking.AddForce(flocking.Evade(a));
            _sr.color = _fleeing;
            return;
        }

        if (csFood.Length > 0)
        {
            var a = csFood[0].transform.transform.position;
            flocking.AddForce(flocking.Arrive(a));
            _sr.color = _alone;
            return;
        }

        CalculateMates();
        CalculateFlocking();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition((transform.position += flocking.Vel) * (_maxSpeed * Time.fixedDeltaTime));
    }

    void CalculateMates()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _sightRadiusForFlockmates, _boidsMask);

        foreach (var item in colliders)
        {
            if (item.gameObject.TryGetComponent<Boid>(out var boid))
            {
                if (!_boids.Contains(boid))
                    _boids.Add(boid);
            }

            _sr.color = _flocking;
        }

        if (colliders.Length < _boids.Count)
        {
            foreach (var item in _boids)
            {
                if (Vector3.Distance(item.transform.position, transform.position) > _sightRadiusForFlockmates)
                { _boids.Remove(item); break; }
            }
        }
    }

    void CalculateFlocking()
    {
        flocking.AddForce(flocking.Separation(_boids, _separationRange) * _separationStrength);
        flocking.AddForce(flocking.Cohesion(_boids) * _cohesionStrength);
        flocking.AddForce(flocking.Alignment(_boids) * _alignmentStrength);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRadiusForFood);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightRadiusForPredator);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _sightRadiusForFlockmates);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + transform.right * _rayOffset, transform.position + transform.right * _rayOffset + transform.up * _rayDistance);
        Gizmos.DrawLine(transform.position - transform.right * _rayOffset, transform.position - transform.right * _rayOffset + transform.up * _rayDistance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Predator>())
            Resurrector.instance.Resurrect(this);
    }
}
