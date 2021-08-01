using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSprite : MonoBehaviour
{
    [SerializeField]
    private SignalTrigger trigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        trigger.RaiseSignal();
    }
}
