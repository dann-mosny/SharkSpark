using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpread : MonoBehaviour
{
	public float probabilityOfSpread;
	public GameObject FirePrefab;
	public Transform Parent;
	public bool CanHurtPlayer;

	// Start is called before the first frame update
	void Start()
	{
		CanHurtPlayer = false;
		StartCoroutine(AllowHurtingPlayer());
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (Random.Range(0f, 1f)>1-probabilityOfSpread)
		{
			Vector2 spreadDirection = Random.insideUnitCircle.normalized;
			GameObject newFire = Instantiate(FirePrefab, transform.position + new Vector3(spreadDirection[0], spreadDirection[1], 0), Quaternion.identity, Parent);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Wall")) Destroy(gameObject);
		if (collision.gameObject.CompareTag("Water")) Destroy(gameObject);
		else if (collision.gameObject.CompareTag("Fire"))
		{
			if (this.GetInstanceID() < collision.gameObject.GetInstanceID())
			{
				GetComponent<Animator>().SetBool("Destroyed", true);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Hydrant")) Destroy(gameObject);
	}

	void DestroyFire()
	{
		Destroy(gameObject);
	}

	IEnumerator AllowHurtingPlayer()
	{
		yield return new WaitForSeconds(1);
		CanHurtPlayer = true;
	}
}
