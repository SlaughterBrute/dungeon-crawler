using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent raisedEvent;
    [SerializeField]
    private SignalTrigger trigger;


    public void Raise()
    {
        raisedEvent.Invoke();
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
