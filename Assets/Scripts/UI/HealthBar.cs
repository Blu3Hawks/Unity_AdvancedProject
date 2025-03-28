using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        public void UpdateHealthBar(float curHp, float maxHp)
        {
            if (maxHp == 0f) return;

            if (curHp < 0f)
                curHp = 0f;
            
            fillImage.fillAmount = curHp / maxHp;
        }
    }
}
