using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class WallBehaviour : MonoBehaviour
{
	public GameObject[] props;
	public GameObject podiumPrefub;
	// Start is called before the first frame update
	void Start()
    {
		int xCord = Mathf.RoundToInt(transform.position.x - MapGrid.zeroCords.x);
		int yCord = Mathf.RoundToInt(transform.position.z - MapGrid.zeroCords.y);
		MapGrid.mapGrid[xCord, yCord].Add(gameObject);

		GetComponent<MeshRenderer>().enabled = false;

		SetProp();
	}

	// Update is called once per frame
	void Update()
    {

	}

	void SetProp()
	{
		var random = Random.Range(0, props.Length-3);
		var prop = Instantiate(props[random], transform);
		prop.transform.position = transform.position - new Vector3(0, 0.5f, 0);
		prop.transform.rotation = Quaternion.AngleAxis(Random.Range(170, 190), new Vector3(0, 1, 0));

		var podium = Instantiate(podiumPrefub, transform);
		podium.transform.position = transform.position - new Vector3(0, 0.5f, 0);
		podium.transform.localScale = Vector3.one * 0.5f;
	}
}
