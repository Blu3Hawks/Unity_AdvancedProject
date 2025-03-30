using Chen_s_Folder.Scripts.Save___Load.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Chen_s_Folder.Scripts.Main_Menu
{
    public class SaveSlot : MonoBehaviour
    {
        [Header("Profile")]
        [SerializeField] private string profileId;

        [Header("Content")]
        [SerializeField] private GameObject hasDataContent;
        [SerializeField] private GameObject noDataContent;

        [SerializeField] private TextMeshProUGUI nameText;
        [Header("Clear Data Button")]
        [SerializeField] private Button clearButton;
        public bool HasData { get; private set; } = false;

        private Button _saveSlotButton;


        private void Awake()
        {
            _saveSlotButton = GetComponent<Button>();
        }
        public void SetData(GameData data)
        {
            if (data == null)
            {
                HasData = false;
                noDataContent.SetActive(true);
                hasDataContent.SetActive(false);
                clearButton.gameObject.SetActive(false);
            }
            else
            {
                HasData = true;
                noDataContent.SetActive(false);
                hasDataContent.SetActive(true);
                clearButton.gameObject.SetActive(true);
            }

        }

        public string GetProfileId()
        {
            return this.profileId;
        }

        public void SetInteractable(bool interactable)
        {
            _saveSlotButton.interactable = interactable;
            clearButton.interactable = interactable;
        }
    }
}
