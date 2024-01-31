using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeBehaviour : MonoBehaviour
{
    private Scrollbar musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.2f);
        }

		gameObject.GetComponent<Scrollbar>().value = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
		musicVolume = gameObject.GetComponent<Scrollbar>();
	}

    // Update is called once per frame
    void Update()
    {
        if (musicVolume != null)
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        }
    }
}
