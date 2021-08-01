using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SignalTrigger : ScriptableObject
{
    private List<SignalEvent> signalEvents = new List<SignalEvent>();

    public void RaiseSignal()
    {
        for(int i = signalEvents.Count-1; i >= 0; i--)
        {
            signalEvents[i].Raise();
            Debug.Log("raised");
        }
    }

    public void AddSignal(SignalEvent signalEvent)
    {
        signalEvents.Add(signalEvent);
    }

    public void RemoveSignal(SignalEvent signalEvent)
    {
        signalEvents.Remove(signalEvent);
    }
}
