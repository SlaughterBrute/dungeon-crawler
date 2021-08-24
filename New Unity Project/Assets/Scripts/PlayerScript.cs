using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float vertical;
	private Vector3 moveDirection;
	[SerializeField]
	private float moveSpeed = 5f;
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
		GetMovementInput();
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

    void GetMovementInput()
    {
		float moveX = 0f;
		float moveY = 0f;

		if(Input.GetKey(KeyCode.W))
		{
			moveY = +1f;
		}
		if(Input.GetKey(KeyCode.S))
		{
			moveY = -1f;
		}
		if (Input.GetKey(KeyCode.A))
		{
			moveX = -1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			moveX = +1f;
		}

		moveDirection = new Vector3(moveX, moveY).normalized;
	}

	private void FixedUpdate()
	{
		rb.velocity = moveDirection * moveSpeed;
	}

	public void TriggerWithEventExample()
    {
        Debug.Log("I am triggered with a lose coupled signal event system");
    }
}
