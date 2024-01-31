using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.ParticleSystem;

public class BoomBehaviour : MonoBehaviour
{
	public string[] obstacles;

	[SerializeField]
	private float boomTimer = 2;
	private MenuBehaviour _menu;
	private ParticleSystem particles;

	// Start is called before the first frame update
	void Start()
	{
		_menu = FindObjectsByType<MenuBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0];
		particles = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update()
	{
		TimerUpdate();
		CheckBoomLifetime();
	}

	void TimerUpdate()
	{
		if(_menu != null && _menu.isMenuOpened)
		{
			particles.Pause();
			return;
		}

		if (boomTimer > 0)
		{
			boomTimer -= Time.deltaTime;
		}
	}

	void CheckBoomLifetime()
	{
		if (_menu != null && _menu.isMenuOpened)
		{
			particles.Pause();
			return;
		}
		else
		{
			particles.Play();
		}

		if (boomTimer <= 0)
		{
			Destroy(this.gameObject);
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		foreach (var obst in obstacles)
		{
			if (other.gameObject.tag == obst)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
