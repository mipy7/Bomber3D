using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    public float bombTimer = 3;
	public GameObject bombAudioPrefub;

	[SerializeField]
	private GameObject boomPrefub;
	private MenuBehaviour _menu;
	private Vector3[] boomPattern = {
		new Vector3(0, 0, 0), // center
		new Vector3(1, 0, 0), new Vector3(2, 0, 0), new Vector3(3, 0, 0), // left
		new Vector3(0, 0, 1), new Vector3(0, 0, 2), new Vector3(0, 0, 3), // up
		new Vector3(-1, 0, 0), new Vector3(-2, 0, 0), new Vector3(-3, 0, 0), // right
		new Vector3(0, 0, -1), new Vector3(0, 0, -2), new Vector3(0, 0, -3), // down
	};

    // Start is called before the first frame update
    void Start()
    {
		_menu = FindObjectsByType<MenuBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0];
	}

    // Update is called once per frame
    void Update()
    {
		TimerUpdate();
		CheckBombBoom();
	}

	void TimerUpdate()
	{
		if (_menu.isMenuOpened)
		{
			return;
		}

		if (bombTimer > 0)
		{
			bombTimer -= Time.deltaTime;
		}
	}

	void CheckBombBoom()
	{
		if(bombTimer <= 0)
		{
			SpawnBoom();
			SpawnBoomSound();
			Destroy(this.gameObject);
		}
	}

	void SpawnBoom()
	{
		Vector3 pos = transform.position;
		foreach(var shift in boomPattern)
		{
			RaycastHit hit = new RaycastHit();
			if(!Physics.SphereCast(pos, 0.4f, shift, out hit, Vector3.Distance(Vector3.zero, shift), 1<<0, QueryTriggerInteraction.Ignore))
			{
				Instantiate(boomPrefub, pos + shift, Quaternion.identity);
			}
		}
	}

	void SpawnBoomSound()
	{
		Vector3 pos = transform.position;
		Instantiate(bombAudioPrefub, pos, Quaternion.identity).GetComponent<AudioSource>().Play();
	}
}
