using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class CameraBehaviour : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 10, -6);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		TryGetPlayer();

		if (_player != null)
        {
			transform.position = _player.transform.position + cameraOffset;
		}
	}

	void TryGetPlayer()
	{
		if (_player == null)
		{
			_player = GameObject.Find("Player");
		}
	}
}
