using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player2UI : MonoBehaviour
{
    public TMP_Text HPPlayer2;

    Player2Health playerStats;
    public void Start()
    {
        playerStats = GetComponent<Player2Health>();
    }

    private void Update()
    {
        SetStats();
    }

    void SetStats()
    {
        HPPlayer2.text = playerStats.currentHealth.ToString();
    }
}
