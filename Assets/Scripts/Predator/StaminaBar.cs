using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] Image _bar;
    [SerializeField] RectTransform _barRectTransform;
    [SerializeField] float _offset;
    [SerializeField] Predator _predator;

    void Update()
    {
        _barRectTransform.position = Position();

        _bar.fillAmount =
            _predator.stateMachine.currentState != _predator.stateMachine.states[PredatorStates.Resting] ?
            _predator.CurrentStamina / _predator.MaxStamina : 0;
    }

    Vector3 Position()
    {
        return Camera.main.WorldToScreenPoint(_predator.transform.position + Vector3.up * _offset);
    }
}
