using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShooter : MonoBehaviour
{
	public Transform WaterParent;
	public GameObject WaterPrefab;
	public GameObject Player;
	public float WaterSpeed;
	public int WaterAmount;
	public int WaterMax;
	public uint WaterEvaporatingRate;
	uint WaterEvaporatingCycle;
	public GameObject WaterTank;
	public uint WaterBarMaxSize;
	public GameObject WaterBar;
	public GameObject UiManager;
	public float MinimalTankAlpha;
	public float MaximalTankAlpha;
	AudioSource audioSource;

	// Start is called before the first frame update
	void Start()
	{
		WaterAmount = WaterMax;
		WaterEvaporatingCycle = 0;
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		WaterEvaporatingCycle = (WaterEvaporatingCycle + 1) % WaterEvaporatingRate;
		if (WaterEvaporatingCycle==0) WaterAmount --;

		if (WaterAmount <= 0) UiManager.GetComponent<UiController>().SetGameOver("You ran out of water");
		if (Input.GetMouseButton(0))
		{
			WaterAmount--;
			Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player.transform.position;
			direction.z = 0;
			GameObject newWater = Instantiate(WaterPrefab, Player.transform.position, Quaternion.identity, WaterParent);
			newWater.GetComponent<WaterController>().speed = direction.normalized * WaterSpeed;
		}

		float waterLevel = (float)WaterAmount / WaterMax;
		WaterBar.transform.localScale = new Vector3(waterLevel*WaterBarMaxSize, 1, 1);
		Color waterTankColor = WaterTank.GetComponent<SpriteRenderer>().color;
		waterTankColor.a = MinimalTankAlpha + (MaximalTankAlpha - MinimalTankAlpha) * waterLevel;
		WaterTank.GetComponent<SpriteRenderer>().color = waterTankColor;
	}

	void Update()
	{
		if (Time.timeScale == 0) return;
		if (Input.GetMouseButtonDown(0))
		{
			audioSource.Play();
		}
		if (Input.GetMouseButtonUp(0))
		{
			audioSource.Stop();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Hydrant"))
		{
			if (WaterAmount < WaterMax) WaterAmount++;
		}
	}
}
