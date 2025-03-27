using UnityEngine;
using UnityEngine.Events;

public class LevelUpSystem : MonoBehaviour
{
    // Events
    public event UnityAction OnLevelUp;
    
    [Header("Settings")] 
    [SerializeField] private int curLevel = 1;
    [SerializeField] private float baseXpRequirement = 100f;
    [SerializeField] private float xpGrowthFactor = 1.2f;

    [Header("References")] 
    [SerializeField] private PlayerController player;
    
    private float _curXp;

    public void AddXp(float xpAmount)
    {
        _curXp += xpAmount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (_curXp >= GetXpToNextLevel())
        {
            _curXp -= GetXpToNextLevel();
            ++curLevel;
            LevelUp();
        }
    }

    private float GetXpToNextLevel()
    {
        return baseXpRequirement * Mathf.Pow(xpGrowthFactor, curLevel - 1);
    }

    public void LevelUp()
    {
        OnLevelUp?.Invoke();
    }
}