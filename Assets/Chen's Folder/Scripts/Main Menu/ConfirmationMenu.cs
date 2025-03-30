using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Chen_s_Folder.Scripts.Main_Menu
{
    public class ConfirmationMenu : Menu
    {
        [SerializeField] private MainMenu mainMenu;
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        public void ActivateMenu(string displayText, UnityAction confirmAction, UnityAction cancelAction)
        {
            this.gameObject.SetActive(true);
            this.displayText.text = displayText;
            //removing any existing listeners added by code - just to make sure we don't make extra actions
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();

            confirmButton.onClick.AddListener(() =>
            {
                DeactivateMenu();
                mainMenu.DisableButtonsAccordingToData();
                confirmAction();
            });
            cancelButton.onClick.AddListener(() =>
            {
                DeactivateMenu();
                mainMenu.DisableButtonsAccordingToData();
                cancelAction();
            });
        }

        private void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}
