using Cysharp.Threading.Tasks;
using Naku.AuthenticateSystem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace Naku.LeaderboardSystem
{
    public class LeaderboardData : MonoBehaviour
    {
        public event Action OnGetEntryStart;
        public event Action<List<LeaderboardEntry>> OnGetEntryDone;
        public event Action<LeaderboardEntry> OnGetPersonalEntryDone;


        public string leaderboardDisplayName;
        [SerializeField] private string _leaderboardID;
        [SerializeField] private int _entryLimit = 8;


        public async UniTask GetScores()
        {
            if (string.IsNullOrEmpty(_leaderboardID))
            {
                Debug.LogError("LeaderboardData.leaderboardID is null or empty - NakuPacks");
                return;
            }
            if (!AuthManager.Instance.IsSignedIn)
            {
                Debug.LogWarning("Player not signed in - NakuPacks");
            }
            await UniTask.WaitUntil(() => AuthManager.Instance.IsSignedIn);

            OnGetEntryStart?.Invoke();

            var task = LeaderboardsService.Instance.GetPlayerScoreAsync(_leaderboardID)
                 .ContinueWith((response) => OnGetPersonalEntryDone?.Invoke(response.Result), TaskScheduler.FromCurrentSynchronizationContext());

            int offset = 0;
            _ = LeaderboardsService.Instance.GetScoresAsync(_leaderboardID,
                new GetScoresOptions { Offset = offset, Limit = _entryLimit })
                .ContinueWith((response) => OnGetEntryDone?.Invoke(response.Result.Results), TaskScheduler.FromCurrentSynchronizationContext());
        }

        public async void SetScore(int score)
        {
            await LeaderboardsService.Instance.AddPlayerScoreAsync(_leaderboardID, score);
        }


        public void DebugLoader()
        {
            OnGetPersonalEntryDone += LeaderboardData_OnGetPersonalEntryDone;
            OnGetEntryDone += LeaderboardData_OnGetEntryDone;
        }

        private void LeaderboardData_OnGetEntryDone(List<LeaderboardEntry> obj)
        {
            print("LeaderboardData_OnGetEntryDone Result: obj.Count:" + obj.Count);
        }

        private void LeaderboardData_OnGetPersonalEntryDone(LeaderboardEntry obj)
        {
            print("LeaderboardData_OnGetPersonalEntryDone Result: obj.Score:" + obj.Score);
        }
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                SetScore(UnityEngine.Random.Range(0, 811));
            }
        }
    }
}