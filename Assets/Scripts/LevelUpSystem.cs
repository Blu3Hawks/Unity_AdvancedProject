using UnityEngine;
using UnityEngine.Events;

public class LevelUpSystem : MonoBehaviour
{
    // Events
    public event UnityAction<int> OnLevelUp;
    
    [Header("Settings")] 
    [SerializeField] private int curLevel = 1;
    [SerializeField] private float baseXpRequirement = 100f;
    [SerializeField] private float xpGrowthFactor = 1.2f;

    [Header("References")] 
    [SerializeField] private PlayerController player;
    
    public float CurXp { get; private set; }
    public float XpToNextLevel { get; private set; }

    public void AddXp(float xpAmount)
    {
        CurXp += xpAmount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (CurXp >= GetXpToNextLevel())
        {
            CurXp -= GetXpToNextLevel();
            ++curLevel;
            OnLevelUp?.Invoke(curLevel);
        }
    }

    private float GetXpToNextLevel()
    {
        XpToNextLevel = baseXpRequirement * Mathf.Pow(xpGrowthFactor, curLevel - 1);
        return XpToNextLevel;
    }
}