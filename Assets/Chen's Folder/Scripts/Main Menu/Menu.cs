using UnityEngine;
using UnityEngine.UI;

namespace Chen_s_Folder.Scripts.Main_Menu
{
    public class Menu : MonoBehaviour
    {
        [Header("First Selected Button")]
        [SerializeField] private Button firstSelected;

        protected virtual void OnEnable()
        {
            if (firstSelected != null)
                SetFirstSelected(firstSelected);
        }

        protected void SetFirstSelected(Button firstSelectedButton)
        {
            firstSelectedButton.Select();
        }
    }
}
