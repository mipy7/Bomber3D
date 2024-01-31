using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class BotBehaviour : MonoBehaviour
{
	public float speed = 0.5f;
	public int priority;

	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public PlayerBehaviour playerBehaviour;
	[HideInInspector]
	public Vector3 targerPos;

	private enum Direction
	{
		up, down, left, right, nullDir
	}

	private bool isMoving = false;
	private bool isNeedMove = false;
	private bool isNeedPass = false;
	private Rigidbody _rb;
	private MenuBehaviour _menu;
	private Direction movDir;
	private WaveAlgorithm waveAlgorithm;
	private List<BotBehaviour> botBehaviours = new List<BotBehaviour>();

	// Start is called before the first frame update
	void Start()
    {
		player = GameObject.Find("Player");
		playerBehaviour = player.GetComponent<PlayerBehaviour>();
		waveAlgorithm = GetComponent<WaveAlgorithm>();
		_rb = GetComponent<Rigidbody>();
		_menu = FindObjectsByType<MenuBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0];
	}

    // Update is called once per frame
    void Update()
    {
		MoveController();
		RotateToward();
		Steering();
		SetOnMapGrid();
	}

	// MoveController call MoveToDir func in update cycle if bot need to move
	// make wave algorithm
	private void MoveController()
	{
		if (_menu.isMenuOpened)
		{
			return;
		}

		if (!isNeedMove && player != null && !playerBehaviour.isFinish)
		{
			var botPos = GetGridPos();
			var playerPos = playerBehaviour.GetGridPos();

			List<Vector2Int> directionList = waveAlgorithm.FindPath(botPos, playerPos);

			if (directionList.Count > 0)
			{
				movDir = Vec2ToDir(directionList[0]);

				isNeedMove = true;
			}
		}
		else
		{
			_rb .velocity = Vector2.zero;
		}

		MoveToDir(movDir);
	}

	void RotateToward()
	{
		gameObject.transform.LookAt(transform.position + DirToVec3(movDir));
	}

	// MoveToDir move bot to one cell on given direction, return false if bot cant move to given dir, true if bot can move, 
	// func calls while bot moves to given cell, when bot finish move, set move trigger flag to false state
	// (use MapGrid as check is it possible to move or not)
	private bool MoveToDir(Direction dir)
	{

		if (isNeedPass)
		{
			return false;
		}
		
		if (!isMoving && isNeedMove)
		{
			if (Physics.Raycast(transform.position, DirToVec3(dir), 1f, 1 << 0, QueryTriggerInteraction.Ignore))
			{
				isMoving = false;
				isNeedMove = false;
				return false;
			}

			targerPos = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z)) + DirToVec3(dir);
			isMoving = true;
		}
		else if(isMoving)
		{
			transform.position = Vector3.MoveTowards(transform.position, targerPos, speed * Time.deltaTime);

			if (transform.position == targerPos)
			{
				isMoving = false;
				isNeedMove = false;
			}
		}

		return isMoving;
	}

	void Steering()
	{
		isNeedPass = false;
		botBehaviours.ForEach(bot =>
		{
			if(bot.botBehaviours.Count > 0 && bot.botBehaviours.IndexOf(this) >= 0)
			{
				if (bot.targerPos == targerPos && bot.priority < priority)
				{
					isNeedPass = true;
				}
			}
			else
			{
				isNeedPass = true;
			}
		});
	}

	void RemoveElemBotList(BotBehaviour botElem)
	{
		botBehaviours = botBehaviours.Where(elem => elem != botElem).ToList();
	}

	void SetOnMapGrid()
	{
		RemoveFromMapGrid();
		Vector2Int pos = GetGridPos();
		MapGrid.mapGrid[pos.x, pos.y].Add(gameObject);
	}

	void RemoveFromMapGrid()
	{
		MapGrid.RemoveObject(gameObject);
	}

	Vector3 DirToVec3(Direction dir)
	{
		switch (dir)
		{
			case Direction.up: return new Vector3(0, 0, 1);
			case Direction.down: return new Vector3(0, 0, -1);
			case Direction.left: return new Vector3(-1, 0, 0);
			case Direction.right: return new Vector3(1, 0, 0);
			default: return Vector3.zero;
		}
	}

	Direction Vec2ToDir(Vector2Int vec)
	{
		switch (vec)
		{
			case Vector2Int vector when vector.Equals(Vector2Int.up): return Direction.up;
			case Vector2Int vector when vector.Equals(Vector2Int.down): return Direction.down;
			case Vector2Int vector when vector.Equals(Vector2Int.left): return Direction.left;
			case Vector2Int vector when vector.Equals(Vector2Int.right): return Direction.right;
			default: return Direction.nullDir;
		}
	}

	public Vector2Int GetGridPos()
	{
		int xCord = Mathf.RoundToInt(transform.position.x - MapGrid.zeroCords.x);
		int yCord = Mathf.RoundToInt(transform.position.z - MapGrid.zeroCords.y);
		return new Vector2Int(xCord, yCord);
	}

	private void OnCollisionExit(Collision collision)
	{
		if(collision.gameObject.name == "Player")
		{
			isNeedMove = false;
			isMoving = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Boom")
		{
			RemoveFromMapGrid();
			Destroy(this.gameObject);
		}
		if(other.tag == "Bot")
			botBehaviours.Add(other.gameObject.GetComponent<BotBehaviour>());
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Bot")
			RemoveElemBotList(other.gameObject.GetComponent<BotBehaviour>());
	}
}