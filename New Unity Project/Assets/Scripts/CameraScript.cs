using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 change;
    private Transform goal;
    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        change = new Vector3(goal.position.x - transform.position.x, goal.position.y - transform.position.y, 0);
        //change = Vector3.Lerp(change)
        transform.Translate(change);
    }
}
