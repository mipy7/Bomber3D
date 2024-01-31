using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    public static Vector2Int mapSize = new Vector2Int(33, 17);
    public static Vector2Int zeroCords = new Vector2Int(-29, -7);
    public static Vector2Int endCords = new Vector2Int(3, 9);

    [HideInInspector]
    public static List<GameObject>[,] mapGrid = new List<GameObject>[mapSize.x, mapSize.y];

	// Start is called before the first frame update
	void Awake()
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                mapGrid[i, j] = new List<GameObject>();

            }
        }
		
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		RemoveNull();
	}

	private static Vector2Int GetObjectIndex(GameObject obj)
    {
        Vector2Int index = Vector2Int.zero;

		for (int i = 0; i < mapSize.x; i++)
		{
			for (int j = 0; j < mapSize.y; j++)
			{
				if (mapGrid[i, j].IndexOf(obj) != -1)
                {
					index.x = i;
					index.y = j;

					return index;
                }
			}
		}

        return -Vector2Int.one;
	}

    public static int RemoveObject(GameObject obj)
    {
		Vector2Int index = GetObjectIndex(obj);

        if(index != -Vector2Int.one)
        {
            mapGrid[index.x, index.y].Remove(obj);
            return 1;
        }
        return 0;
	}

	void RemoveNull()
	{
		for (int i = 0; i < mapSize.x; i++)
		{
			for (int j = 0; j < mapSize.y; j++)
			{
				mapGrid[i,j] = mapGrid[i, j].Where(elem => elem != null).ToList();

			}
		}
		
	}
}
