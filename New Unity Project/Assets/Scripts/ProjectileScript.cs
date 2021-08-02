using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 4;
    private Vector3 direction;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void setUp(Vector3 direction)
    {
        this.direction = direction;
        Debug.Log(direction);
        Destroy(this.gameObject, 10);
    }
}
