using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour
{

    public GameObject menu;
    public GameObject optionMenu;

    public bool isMenuOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MenuUpdate();

	}

    private void MenuUpdate()
    {
        if (menu != null && optionMenu != null)
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                if(!isMenuOpened)
                {
					menu.SetActive(true);
				}
                else
                {
					menu.SetActive(false);
					optionMenu.SetActive(false);
				}

                isMenuOpened = !isMenuOpened;
			}
        }
    }

    public void OpenMenu()
    {
		menu.SetActive(true);
		isMenuOpened = true;
	}

	public void CloseMenu()
    {
        menu.SetActive(false);
		isMenuOpened = false;
	}
}
