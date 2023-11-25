using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Script sacado del video de Brackeys de como crear una barra de vida
public class HealthBar : MonoBehaviour
{
   public Slider slider;
   public TextMeshProUGUI hpText;

   public void SetMaxHealth(int health)
   {
        slider.maxValue = health;
        slider.value = health;
        hpText.text = health + "/" + health;
   }

   public void SetHealth(int health)
   {
        slider.value = health;
        hpText.text = slider.value + "/" + slider.maxValue;
   }
}
