using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotMenu;
    [SerializeField] private StartingMenu startingMenu;
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button backButton;


    private void Start()
    {
        DisableButtonsAccordingToData();
    }

    public void DisableButtonsAccordingToData()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            loadGameButton.interactable = false;
        }
    }
    public void OnBackClicked()
    {
        DeactivateMenu();
        startingMenu.ActivateMenu();
    }
    public void OnNewGameClicked()
    {
        saveSlotMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked()
    {
        saveSlotMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }


    public void OnLoadClicked()
    {
        DisableMenuButtons();
        SceneManager.LoadSceneAsync("Dungeon 1");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
        backButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonsAccordingToData();
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
