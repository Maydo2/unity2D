using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text HPPlayer1;

    PlayerHealth playerStats;
    public void Start()
    {
        playerStats = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        SetStats();
    }

    void SetStats()
    {
        HPPlayer1.text = playerStats.currentHealth.ToString();
    }
}
