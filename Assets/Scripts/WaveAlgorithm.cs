using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAlgorithm : MonoBehaviour
{
    [HideInInspector]
    public int[,] grid = new int[MapGrid.mapSize.x, MapGrid.mapSize.y];

	private Queue<Vector2Int> queue = new Queue<Vector2Int>();
	private Vector2Int[] pattern =
	{
		new Vector2Int(1, 0), // right
		new Vector2Int(0, 1), // up
		new Vector2Int(-1, 0), // left
		new Vector2Int(0, -1) // down
	};

	// Start is called before the first frame update
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		
	}

	void InitializeGrid()
    {
		for (int i = 0; i < MapGrid.mapSize.x; i++)
		{
			for (int j = 0; j < MapGrid.mapSize.y; j++)
			{
				grid[i, j] = -1;
				MapGrid.mapGrid[i, j].ForEach(elem =>
				{
					if (elem.tag == "Wall" || elem.tag == "Bot")
					{
						grid[i, j] = -2;

						//Debug.Log("grid[" + i + "," + j + "] = " + grid[i, j]);
					}
				});
			}
		}
	}

	void FillGrid(Vector2Int startPos, Vector2Int endPos)
	{
		queue.Clear();
		queue.Enqueue(new Vector2Int(endPos.x, endPos.y));
		int trigger = grid[endPos.x, endPos.y];
		
		grid[endPos.x, endPos.y] = 0;

		while (queue.Count > 0)
		{
			Vector2Int currentCell = queue.Dequeue();
			if (currentCell != startPos)
			{
				foreach(var shift in pattern)
				{
					var shiftCell = currentCell + shift;

					if(shiftCell.x >= 0 && shiftCell.x < MapGrid.mapSize.x && shiftCell.y >= 0 && shiftCell.y < MapGrid.mapSize.y)
					{
						if (grid[shiftCell.x, shiftCell.y] > grid[currentCell.x, currentCell.y] + 1 || grid[shiftCell.x, shiftCell.y] == -1)
						{
							grid[shiftCell.x, shiftCell.y] = grid[currentCell.x, currentCell.y] + 1;
							queue.Enqueue(shiftCell);
						}
					}
				}
			}
		}
	}

	private List<Vector2Int> MakePath(Vector2Int startPos, Vector2Int endPos)
	{
		List<Vector2Int> path = new List<Vector2Int>();

		Vector2Int minCell = startPos;
		Vector2Int currentCell = startPos;
		int minDist = 1;

		while (currentCell != endPos && minDist > 0)
		{
			Vector2Int nearestDir = -Vector2Int.one;
			minDist = -1;

			foreach (var shift in pattern)
			{
				var shiftCell = currentCell + shift;

				if (shiftCell.x >= 0 && shiftCell.x < MapGrid.mapSize.x && shiftCell.y >= 0 && shiftCell.y < MapGrid.mapSize.y)
				{
					if (grid[shiftCell.x, shiftCell.y] >= 0 && (grid[shiftCell.x, shiftCell.y] < minDist || minDist < 0))
					{
						minDist = grid[shiftCell.x, shiftCell.y];
						nearestDir = shift;
						minCell = shiftCell;
					}
				}
			}

			if (minDist >= 0)
			{
				path.Add(nearestDir);
				currentCell = minCell;
			}
		}

		return path;
	}

	public List<Vector2Int> FindPath(Vector2Int startPos, Vector2Int endPos)
	{
		InitializeGrid();
		FillGrid(startPos, endPos);
		return MakePath(startPos, endPos);
	}
}
