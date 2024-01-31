using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class BotSpawnBehaviour : MonoBehaviour
{
    public float spawnTime = 5;
    public GameObject botPrefub;

    private float currentTime = 0;
    private GameObject player;
    private int priority = 1;
	private MenuBehaviour _menu;

	// Start is called before the first frame update
	void Start()
    {
		_menu = FindObjectsByType<MenuBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0];
	}

    // Update is called once per frame
    void Update()
    {
		TryGetPlayer();
		SpawnMob();
	}

	void TryGetPlayer()
	{
		if(player == null)
        {
			player = GameObject.Find("Player");
		}
	}

    void SpawnMob()
    {
		if (_menu.isMenuOpened)
		{
			return;
		}
		if (player != null)
		{
			if (currentTime < 0)
			{
				Transform spawnTrasform = transform.GetChild(Random.Range(0, transform.childCount));
				var bot = Instantiate(botPrefub, spawnTrasform);
				bot.GetComponent<BotBehaviour>().priority = priority;
				currentTime = spawnTime;
				priority++;
			}
			currentTime -= Time.deltaTime;
		}
	}
}
