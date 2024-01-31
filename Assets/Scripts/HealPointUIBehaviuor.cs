using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealPointUIBehaviuor : MonoBehaviour
{
    public RenderTexture voidHP;
    public RenderTexture filledHP;

    private GameObject fewHPUI;
    private GameObject manyHPUI;

    private RawImage[] healPointObjects;

    private TMP_Text manyHPUIText;

    private int maxHealPoints;
    private int healPoints;

    // Start is called before the first frame update
    void Start()
    {
		fewHPUI = transform.GetChild(0).gameObject;
		manyHPUI = transform.GetChild(1).gameObject;
        
        SetUpGui();
	}

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();
	}

    void SetUpGui()
    {
        maxHealPoints = PlayerPrefs.GetInt("playerMaxHp");
		healPoints = PlayerPrefs.GetInt("playerHp");

		manyHPUIText = manyHPUI.GetComponentInChildren<TMP_Text>();

		healPointObjects = fewHPUI.GetComponentsInChildren<RawImage>();

		if (maxHealPoints > 3)
        {
			fewHPUI.SetActive(false);
            manyHPUI.SetActive(true);
		}
        else
        {
			fewHPUI.SetActive(true);
			manyHPUI.SetActive(false);

			for (int i = 0; i < maxHealPoints; ++i)
			{
				healPointObjects[i].texture = filledHP;
			}

			for (int i = healPointObjects.Length; i > maxHealPoints; --i)
			{
                healPointObjects[i - 1].gameObject.SetActive(false);
			}
		}
	}

    void UpdateGUI()
    {
        if (maxHealPoints > 3)
        {
            manyHPUIText.text = "x" + healPoints.ToString();
        }
        else
        {
			for (int i = 0; i < maxHealPoints - healPoints; ++i)
			{
				healPointObjects[i].texture = voidHP;
			}
		}
    }
}
