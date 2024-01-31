using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
	
	public float speed = 0.5f;
	public bool isFinish = false;

	[SerializeField]
	private GameObject bombPrefub;
    private Rigidbody _rb;
	private MenuBehaviour _menu;
	private int healPoint = 1;
	private bool isGetHit = false;
	
	private List<GameObject> placedBombs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
		_menu = FindObjectsByType<MenuBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0];
		GetPlayerPrefsHp();
	}

	void Update()
	{
		bombController();
		CheckGetHit();
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		movingController();
		rotateToward();
	}

	void movingController()
	{
		_rb.velocity = Vector3.zero;

		if(_menu.isMenuOpened) {
			return;
		}

		if (Input.GetKey("w"))
		{
			_rb.velocity += new Vector3(0, 0, 1);
		}
		if (Input.GetKey("a"))
		{
			_rb.velocity += new Vector3(-1, 0, 0);
		}
		if (Input.GetKey("s"))
		{
			_rb.velocity += new Vector3(0, 0, -1);
		}
		if (Input.GetKey("d"))
		{
			_rb.velocity += new Vector3(1, 0, 0);
		}

		_rb.velocity = _rb.velocity.normalized * speed;
	}

	void rotateToward()
	{
		gameObject.transform.LookAt(transform.position + _rb.velocity);
	}

	void bombController()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Vector3 bombPos = new Vector3(Mathf.RoundToInt(transform.position.x), 0.0f, Mathf.RoundToInt(transform.position.z));
			bool isBombOnPos = false;

			placedBombs.ForEach(elem =>
			{
				if (elem.transform.position == bombPos)
				{
					isBombOnPos = true;
				}
			});
			
			if (!isBombOnPos)
			{
				GameObject bomb = Instantiate(bombPrefub, bombPos, Quaternion.identity);
				placedBombs.Add(bomb);
			}
			else
			{
				Debug.Log("Бомба уже стоит на этом месте");
			}
		}

		CleanUpBombList();
	}

	void CleanUpBombList()
	{
		placedBombs = placedBombs.Where(elem => elem != null).ToList();
	}

	public Vector2Int GetGridPos()
	{
		int xCord = Mathf.RoundToInt(transform.position.x - MapGrid.zeroCords.x);
		int yCord = Mathf.RoundToInt(transform.position.z - MapGrid.zeroCords.y);
		return new Vector2Int(xCord, yCord);
	}

	private void GameOver()
	{
		Destroy(gameObject);
		SceneManager.LoadScene("LoseGameScene");
	}

	private void CheckGetHit()
	{
		if (isGetHit)
		{
			healPoint -= 1;
			SetPlayerPrefsHp(healPoint);

			if (healPoint <= 0)
			{
				GameOver();
			}
			else
			{
				RestartScene();
			}

			isGetHit = false;
		}
	}

	private void SetPlayerPrefsHp(int hp)
	{
		PlayerPrefs.SetInt("playerHp", hp);
	}

	private void GetPlayerPrefsHp() {
		healPoint = PlayerPrefs.GetInt("playerHp");

	}

	private void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Boom" && !isFinish)
		{
			isGetHit = true;
		}
		if(other.tag == "Finish")
		{
			isFinish = true;
			SceneManager.LoadScene("WinGameScene");
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Bot" && !isFinish)
		{
			isGetHit = true;
		}
	}
}
