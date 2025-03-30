using UnityEngine;
using UnityEngine.InputSystem;

namespace Chen_s_Folder.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Objects/Character Data", order = 1)]
    public class CharacterData : ScriptableObject
    {
        public GameObject characterPrefab;
        public PlayerController playerController;
        public LevelUpSystem levelUpSystem;
        public PlayerInput playerInput;

    }
}
