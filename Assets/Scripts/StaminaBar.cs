using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Script sacado del video de Brackeys de como crear una barra de vida
public class StaminaBar : MonoBehaviour
{
   public Slider slider;
   public TextMeshProUGUI StaminaText;

   public void SetMaxStamina(float stamina)
   {
        slider.maxValue = (int) stamina;
        slider.value = (int) stamina;
        StaminaText.text = stamina.ToString() + "/" + stamina.ToString();
   }

   public void SetStamina(float stamina)
   {
        slider.value = (int) stamina;
        StaminaText.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
   }
}
