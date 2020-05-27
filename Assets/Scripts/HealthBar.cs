using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth (int health)
    {
        slider.maxValue = health;
        slider.value = health;
        Debug.Log("max health asetettu");
    }


    public void SetHealth(int health)
    {
        slider.value = health;
        Debug.Log("Health updated");

    }

}
