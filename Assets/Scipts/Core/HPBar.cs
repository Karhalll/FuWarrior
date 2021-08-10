using UnityEngine;
using UnityEngine.UI;

using FW.Attributes;

namespace FW.Core
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] Slider slider = null;
        [SerializeField] Player player = null;

        Health playerHealth = null;

        private void Awake() 
        {
            playerHealth = player.GetComponent<Health>();
        }

        void Start()
        {
            UpdateHPBar();
            playerHealth.onTakeDamage += UpdateHPBar;
        }

        void UpdateHPBar()
        {
            slider.value = playerHealth.GetPercent();
        }
    }
}
