using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Script sacado del video de Brackeys de como crear una barra de vida
public class HealthBar : MonoBehaviour
{
   public Slider slider;
   public TextMeshProUGUI HPText;

   public void SetMaxHealth(int health)
   {
        slider.maxValue = health;
        slider.value = health;
        HPText.text = health.ToString() + "/" + health.ToString();
   }

   public void SetHealth(int health)
   {
        slider.value = health;
        HPText.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
   }
}
