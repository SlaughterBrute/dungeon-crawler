using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 change;
    private Transform goal;
    [SerializeField] float cameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        change = new Vector3(goal.position.x, goal.position.y, transform.position.z);
        change = Vector3.Lerp(transform.position, change, cameraSpeed);
        transform.position = change;
        //change = Vector3.Lerp(change)
        //transform.Translate(change);
    }
}
