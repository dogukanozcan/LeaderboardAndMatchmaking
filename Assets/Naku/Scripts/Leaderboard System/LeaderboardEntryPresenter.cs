using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class LeaderboardEntryPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankLabel;
    [SerializeField] private TextMeshProUGUI playerNameLabel;
    [SerializeField] private TextMeshProUGUI scoreLabel;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void Init(LeaderboardEntry entry)
    {
        rankLabel.text = (entry.Rank+1).ToString();
        playerNameLabel.text = entry.PlayerName.ToString();
        scoreLabel.text = entry.Score.ToString();
        gameObject.SetActive(true);
    }
}
