using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class XpBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        public void UpdateXpBar(float curXp, float maxXp)
        {
            if (maxXp != 0)
                fillImage.fillAmount = curXp / maxXp;
        }
    }
}