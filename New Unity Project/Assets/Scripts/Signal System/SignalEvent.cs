using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent raiseEvent;
    [SerializeField]
    private SignalTrigger trigger;


    public void Raise()
    {
        raiseEvent.Invoke();
    }

    private void OnEnable()
    {
        trigger.AddSignal(this);
    }

    private void OnDisable()
    {
        trigger.RemoveSignal(this);
    }
}
