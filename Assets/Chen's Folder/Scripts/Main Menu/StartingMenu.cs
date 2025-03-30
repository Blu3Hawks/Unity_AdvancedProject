using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingMenu : Menu
{
    [SerializeField] private MainMenu mainMenu;
    public void OnPlayClicked()
    {
        DeactivateMenu();
        mainMenu.ActivateMenu();
    }
    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
