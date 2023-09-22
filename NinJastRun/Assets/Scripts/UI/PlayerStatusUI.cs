using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] Slider playerHealth;

    void Start()
    {
        player.GetComponent<PlayerHealth>().onHealthChangeEvent += UpdatePlayerHealth;

        UpdatePlayerHealth();
    }


    void Update()
    {
        
    }

    public void UpdatePlayerHealth()
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();

        playerHealth.value = health.GetHealthPercent();
    }
}
