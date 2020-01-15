using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FuWarrior.Attributes;

namespace FuWarrior.Core
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
            slider.value = playerHealth.GetHealthInPercent();
        }
    }
}
