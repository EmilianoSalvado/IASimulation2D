using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PredatorStates { Patrolling, Chasing, Resting}

public class Predator : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _arriveRadius;

    public FiniteStateMachine<PredatorStates, IPredatorUpdate> stateMachine { get; private set; }

    WaypointBehaviour _waypointBehaviour;
    ChasingBehaviour _chasingBehaviour;
    RestingBehaviour _restingBehaviour;

    [Header("Patrolling")]
    [SerializeField] Transform[] _waypoints;

    [Header("Hunting")]
    [SerializeField] float _sightRadius;
    [SerializeField] LayerMask _preysMask;
    [SerializeField] float maxStamina;
    public float MaxStamina { get { return maxStamina; } }
    [SerializeField] float currentStamina;
    public float CurrentStamina { get { return currentStamina; } }
    [SerializeField] float _restingTime;
    public float restingTime { get { return _restingTime; } }
    [SerializeField] float _staminaReducer;
    SteeringBehaviour _prey;

    [Header("ObstacleAvoidance")]
    [SerializeField] float _avoidanceStrength;
    [SerializeField] float _rayOffset;
    [SerializeField] float _rayDistance;
    [SerializeField] LayerMask _obstacleMask;
    public ObstacleAvoidance obstacleAvoidance { get; private set; }

    public SpriteRenderer sr;
    public Color patrolling;
    public Color chasing;
    public Color resting;

    private void Start()
    {
        currentStamina = maxStamina;

        stateMachine = new FiniteStateMachine<PredatorStates, IPredatorUpdate>();

        _waypointBehaviour = new WaypointBehaviour(transform, _maxSpeed, _arriveRadius, _waypoints);
        _chasingBehaviour = new ChasingBehaviour(transform, _maxSpeed, _arriveRadius);
        _restingBehaviour = new RestingBehaviour(transform, _maxSpeed, _arriveRadius);

        stateMachine.AddState(PredatorStates.Patrolling, _waypointBehaviour);
        stateMachine.AddState(PredatorStates.Chasing, _chasingBehaviour);
        stateMachine.AddState(PredatorStates.Resting, _restingBehaviour);

        obstacleAvoidance = new ObstacleAvoidance(transform, _rayOffset, _rayDistance, _obstacleMask);

        stateMachine.ChangeState(PredatorStates.Patrolling);
    }

    private void Update()
    {
        stateMachine.currentState.ArtificialUpdate(this);
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.ArtificialFixed(_rb);
    }

    public SteeringBehaviour LookForPrey()
    {
        var preys = Physics2D.OverlapCircleAll(transform.position, _sightRadius, _preysMask);

        if (preys.Length < 1)
        {
            _prey = null;
            return null;
        }

        if (_prey == null)
            _prey = preys[0].GetComponent<Boid>()?.flocking;

        return _prey;
    }

    public void SpendStamina()
    {
        currentStamina -= _staminaReducer * Time.deltaTime;
    }

    public bool CheckMaxStamina()
    {
        if (currentStamina > maxStamina)
        { currentStamina = maxStamina; return true; }
        return false;
    }

    public bool CheckMinStamina()
    {
        if (currentStamina <= 0)
        { currentStamina =  maxStamina; return true; }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + transform.right * _rayOffset, transform.position + transform.right * _rayOffset + transform.up * _rayDistance);
        Gizmos.DrawLine(transform.position - transform.right * _rayOffset, transform.position - transform.right * _rayOffset + transform.up * _rayDistance);
    }
}
