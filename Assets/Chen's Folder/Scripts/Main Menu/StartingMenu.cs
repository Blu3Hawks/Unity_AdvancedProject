using UnityEngine;

namespace Chen_s_Folder.Scripts.Main_Menu
{
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

        private void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}
