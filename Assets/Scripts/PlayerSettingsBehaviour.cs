using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsBehaviour : MonoBehaviour
{
	public int playerMaxHp = 3;
	public void SetPlayerHp()
	{
		PlayerPrefs.SetInt("playerMaxHp", playerMaxHp);
		PlayerPrefs.SetInt("playerHp", playerMaxHp);
	}
}
