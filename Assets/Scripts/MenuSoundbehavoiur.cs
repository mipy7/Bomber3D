using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundbehavoiur : MonoBehaviour
{
    private AudioSource m_AudioSource;
	private float m_Volume;

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = gameObject.GetComponent<AudioSource>();
		m_AudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
		m_AudioSource.loop = true;
		m_AudioSource.Play();
	}

	void Update()
	{
		m_Volume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
		if(m_Volume != m_AudioSource.volume)
		{
			m_AudioSource.volume = m_Volume;
		}
	}
}
