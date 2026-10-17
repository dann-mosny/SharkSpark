using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	float speed;
	public float MinSpeed;
	public float MaxSpeed;
	Rigidbody2D rbody;
	public GameObject UiManager;
	uint FishRescued;
	public GameObject FishParent;

	// Start is called before the first frame update
	void Start()
	{
		speed = MinSpeed;
		rbody = GetComponent<Rigidbody2D>();
		FishRescued = 0;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		var shooter = GetComponent<WaterShooter>();
		float waterLevel = (float)shooter.WaterAmount / shooter.WaterMax;
		speed = MinSpeed + (1 - waterLevel) * (MaxSpeed - MinSpeed);

		Vector3 movement = Vector3.zero;
		if (Input.GetKey(KeyCode.W)) movement += new Vector3(0, speed, 0);
		if (Input.GetKey(KeyCode.S)) movement += new Vector3(0, -speed, 0);

		if (Input.GetKey(KeyCode.D))
		{
			movement += new Vector3(speed, 0, 0);
			GetComponent<SpriteRenderer>().flipX = true;
		}
		if (Input.GetKey(KeyCode.A))
		{
			movement += new Vector3(-speed, 0, 0);
			GetComponent<SpriteRenderer>().flipX = false;
		}
		rbody.MovePosition(transform.position + movement);

		if (FishRescued == FishParent.transform.childCount) UiManager.GetComponent<UiController>().SetWin();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Fire"))
		{
			if (collision.gameObject.GetComponent<FireSpread>().CanHurtPlayer)
			{
				UiManager.GetComponent<UiController>().SetGameOver("You burned in a fire");
			}
		}

		if (collision.gameObject.CompareTag("Fish"))
		{
			FishRescued++;
			AudioSource.PlayClipAtPoint(collision.gameObject.GetComponent<AudioSource>().clip, transform.position);
			collision.gameObject.SetActive(false);
		}
	}
}
