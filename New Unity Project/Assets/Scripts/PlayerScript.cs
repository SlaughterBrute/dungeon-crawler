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
    [SerializeField]
    private float shootDelay = 1f; // could connect to Scriptable object to be able to change (could be useful for powerups)
    private float timeSinceLastShot = 0;
    [SerializeField]
    GameObject pfProjectile;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
    }

    void Shoot()
    {
        float verticalShootDirection = Input.GetAxisRaw("Shoot vertical");
        float horizontalShootDirection = Input.GetAxisRaw("Shoot horizontal");
        timeSinceLastShot += Time.deltaTime;
        if((horizontalShootDirection != 0 || verticalShootDirection != 0 ) && timeSinceLastShot >= shootDelay)
        {
            timeSinceLastShot = 0;
            if(pfProjectile != null)
            {
                Transform projectileTransform = Instantiate(pfProjectile, transform.position, Quaternion.identity).transform;
                ProjectileScript projectile = projectileTransform.GetComponent<ProjectileScript>();
                Vector3 direction = new Vector3(horizontalShootDirection, verticalShootDirection, 0);
                //Debug.Log(direction);
                projectile.setUp(direction);
                //projectile. (transform.position, transform.position, projectileSpeed);
            }
            
        }
    }

    void Move()
    {
		change = Vector3.zero;
		change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        change.Normalize();
        change *= speed * Time.deltaTime;
        change += transform.position;

        rb.MovePosition(change);
        //rb.transform.Translate(change);
    }

    public void TriggerWithEventExample()
    {
        Debug.Log("I am triggered with a lose coupled signal event system");
    }
}
