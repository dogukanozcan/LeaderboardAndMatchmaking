using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace Naku.LeaderboardSystem
{
    public class LeaderboardPresenter : MonoBehaviour
    {
        [SerializeField] private LeaderboardEntryPresenter _playerEntryPresenterPrefab;
        [SerializeField] private LeaderboardEntryPresenter _personalEntryPresenter;
        [SerializeField] private Transform _leaderboardMainParent;

        [SerializeField] private GameObject _loadingObjectBox;
        [SerializeField] private GameObject _emptyObjectBox;

        [SerializeField] private TextMeshProUGUI _leaderboardHeader;
        [SerializeField] private Button _changeNicknameButton;

        [Header("DATA LINK")]
        [SerializeField] private LeaderboardData _leaderboardData;

        [Header("UI LINK")]
        [SerializeField] private ChangeNicknameManager _changeNicknamePanel;

        private readonly List<LeaderboardEntryPresenter> _entryPresenters = new();



        private void Awake()
        {
            UnityServices.InitializeAsync();
            _changeNicknamePanel = FindAnyObjectByType<ChangeNicknameManager>(findObjectsInactive: FindObjectsInactive.Include);
            if(_changeNicknamePanel == null)
            {
                _changeNicknameButton.gameObject.SetActive(false);
                Debug.LogError("LeaderboardPresenter -> DataName:" + _leaderboardData?.leaderboardDisplayName + " _changeNicknamePanel is null");
            }

        }

        private void OnEnable()
        {
           
            if (_leaderboardData)
            {
                _leaderboardData.OnGetEntryStart += OnGetEntryStart;
                _leaderboardData.OnGetEntryDone += OnGetEntryDone;
                _leaderboardData.OnGetPersonalEntryDone += OnGetPersonalEntryDone;
                _ = _leaderboardData.GetScores();
            }
            else
            {
                Debug.LogError(name + " _leaderboardData is null");
            }

            if (_changeNicknamePanel)
            {
                _changeNicknameButton.onClick.AddListener(() => _changeNicknamePanel.gameObject.SetActive(true));
            }
            else
            {
                Debug.LogError("LeaderboardPresenter -> DataName:" + _leaderboardData?.leaderboardDisplayName + " _changeNicknamePanel is null");

            }

        }
        private void OnDisable()
        {
            if (_leaderboardData)
            {
                _leaderboardData.OnGetEntryStart -= OnGetEntryStart;
                _leaderboardData.OnGetEntryDone -= OnGetEntryDone;
                _leaderboardData.OnGetPersonalEntryDone -= OnGetPersonalEntryDone;
            }
        }

        private void Start()
        {
            if (_leaderboardData)
            {
                _ = _leaderboardData.GetScores();
                _leaderboardHeader.text = _leaderboardData.leaderboardDisplayName;
            }
        }
        private void OnGetEntryStart()
        {
            _loadingObjectBox.SetActive(true);
        }

        public void OnGetPersonalEntryDone(LeaderboardEntry leaderboardEntry)
        {
            _personalEntryPresenter.transform.parent.gameObject.SetActive(true);
            _personalEntryPresenter.Init(leaderboardEntry);
        }
        public void OnGetEntryDone(List<LeaderboardEntry> leaderboardEntries)
        {
            _loadingObjectBox.SetActive(false);

            foreach (var item in _entryPresenters)
            {
                Destroy(item.gameObject);
            }
            _entryPresenters.Clear();

            if (leaderboardEntries.Count > 0)
            {
                _emptyObjectBox.SetActive(false);
            }
            else
            {
                _emptyObjectBox.SetActive(true);
            }

            foreach (var entry in leaderboardEntries)
            {
                CreatePlayerScoreObject(entry);
            }

        }
        public void CreatePlayerScoreObject(LeaderboardEntry entry)
        {
            var playerScorePresenter = Instantiate(_playerEntryPresenterPrefab, _leaderboardMainParent);
            playerScorePresenter.Init(entry);
            _entryPresenters.Add(playerScorePresenter);
        }
    }
}