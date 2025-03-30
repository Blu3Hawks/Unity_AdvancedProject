using System.Collections.Generic;
using Chen_s_Folder.Scripts.Save___Load;
using Chen_s_Folder.Scripts.Save___Load.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Chen_s_Folder.Scripts.Main_Menu
{
    public class SaveSlotsMenu : Menu
    {
        [Header("Menu Navigation")]
        [SerializeField] private StartingMenu startingMenu;

        private SaveSlot[] _saveSlots;

        [SerializeField] private Button backButton;

        [Header("Confirmation Menu")]
        [SerializeField] private ConfirmationMenu confirmationMenu;
        private bool _isLoadingGame;

        private void Awake()
        {
            _saveSlots = this.GetComponentsInChildren<SaveSlot>();
        }

        public void OnSaveSlotClicked(SaveSlot saveSlot)
        {
            DisableMenuButtons();
            if (_isLoadingGame)
            {
                DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                SaveGameAndLoadScene();
            }
            else if (saveSlot.HasData)
            {
                confirmationMenu.ActivateMenu(
                    "Starting a new game with this save slot will override the existing saved data. Are you sure?",
                    //in case we pressed 'yes'
                    () =>
                    {

                        DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                        DataPersistenceManager.Instance.NewGame();
                        SaveGameAndLoadScene();
                    },
                    //in case we pressed 'cancel'
                    () =>
                    {

                        //TODO - come back to this
                        this.ActivateMenu(_isLoadingGame);
                    }
                );

            }
            else
            {
                DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                DataPersistenceManager.Instance.NewGame();
                SaveGameAndLoadScene();
            }

        }

        private void SaveGameAndLoadScene()
        {
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadSceneAsync("Dungeon 1");
        }

        private void DisableMenuButtons()
        {
            foreach (SaveSlot saveSlot in _saveSlots)
            {
                saveSlot.SetInteractable(false);
            }
            backButton.interactable = false;
        }
        public void OnBackClicked()
        {
            startingMenu.ActivateMenu();
            this.DeactivateMenu();

        }

        public void ActivateMenu(bool isLoadingGame)
        {
            this.gameObject.SetActive(true);

            backButton.interactable = true;

            this._isLoadingGame = isLoadingGame;

            Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesData();

            GameObject firstSelected = backButton.gameObject;
            foreach (SaveSlot saveSlot in _saveSlots)
            {
                GameData profileData = null;
                profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
                saveSlot.SetData(profileData);
                if (profileData == null && isLoadingGame)
                {
                    saveSlot.SetInteractable(false);
                }
                else
                {
                    saveSlot.SetInteractable(true);
                    if (firstSelected.Equals(backButton.gameObject))
                    {
                        firstSelected = saveSlot.gameObject;
                    }
                }
            }
            Button firstSelectedButton = firstSelected.GetComponent<Button>();
            this.SetFirstSelected(firstSelectedButton);
        }

        public void OnClearClicked(SaveSlot saveSlot)
        {
            DisableMenuButtons();

            confirmationMenu.ActivateMenu(
                "Are you sure you want to delete this saved file?",
                () =>
                {
                    DataPersistenceManager.Instance.DeleteProfileId(saveSlot.GetProfileId());
                    ActivateMenu(_isLoadingGame);
                },
                () =>
                {
                    ActivateMenu(_isLoadingGame);
                }
            );
        }

        private void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}
