using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine<K, V>
{
    public V currentState { get; private set; }

    Dictionary<K, V> _states = new Dictionary<K, V>();
    public Dictionary<K, V> states { get { return _states; } }

    public void AddState(K key, V value)
    {
        if (!_states.ContainsKey(key))
            _states.Add(key, value);
    }

    public void ChangeState(K key)
    {
        if (_states.ContainsKey(key))
            currentState = _states[key];
    }
}