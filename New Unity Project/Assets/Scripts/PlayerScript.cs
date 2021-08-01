using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float vertical;
	private Vector3 change;
	[SerializeField]
	private float speed = 0.8f;
	Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        //rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    void Move()
    {
		change = Vector3.zero;
		change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        change *= speed * Time.deltaTime;
        rb.transform.Translate(change);

        
        //rb.velocity = new Vector2(change.x*speed, change.y*speed);
        //rb.transform.position = change;

    }

    public void TriggerWithEventExample()
    {
        Debug.Log("I am triggered with a lose coupled signal event system");
    }
}
