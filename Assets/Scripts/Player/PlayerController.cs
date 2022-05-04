using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Rigidbody playerRb;
	public float speed;
	private float currentSpeed;
    // Start is called before the first frame update
    void Start()
    {
		playerRb = gameObject.GetComponent<Rigidbody>();
		currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //Move the Rigidbody forwards constantly at currentSpeed you define (the blue arrow axis in Scene view)
            playerRb.velocity = transform.forward * currentSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            //Move the Rigidbody backwards constantly at the currentSpeed you define (the blue arrow axis in Scene view)
            playerRb.velocity = -transform.forward * currentSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //Rotate the sprite about the Y axis in the positive direction
            transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * (currentSpeed * 10), Space.World);
        }

        if (Input.GetKey(KeyCode.A))
        {
            //Rotate the sprite about the Y axis in the negative direction
            transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * (currentSpeed * 10), Space.World);
        }

		if (Input.GetKey(KeyCode.LeftShift)) {
			currentSpeed = speed * 2;
		} else {
			currentSpeed = speed;
		}
    }
}
