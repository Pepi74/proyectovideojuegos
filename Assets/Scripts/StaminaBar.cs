using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Script sacado del video de Brackeys de como crear una barra de vida
public class StaminaBar : MonoBehaviour
{
   public Slider slider;
   public TextMeshProUGUI staminaText;

   public void SetMaxStamina(float stamina)
   {
        slider.maxValue = (int) stamina;
        slider.value = (int) stamina;
        staminaText.text = stamina + "/" + stamina;
   }

   public void SetStamina(float stamina)
   {
        slider.value = (int) stamina;
        staminaText.text = slider.value + "/" + slider.maxValue;
   }
}
