using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnBehaviour : MonoBehaviour
{
    public GameObject playePrefub;

    // Start is called before the first frame update
    void Start()
    {
        var player = Instantiate(playePrefub, transform.position, Quaternion.identity);
        player.name = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
