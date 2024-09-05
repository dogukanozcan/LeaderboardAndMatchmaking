using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class ChangeNicknameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _nicknameLabel;
    [SerializeField] private TMP_InputField _nicknameInput;

    private string _nickname;

    private void Start()
    {
        _ = GetNickname();
    }


    private async UniTask GetNickname()
    {
        _nickname = await AuthenticationService.Instance.GetPlayerNameAsync();
        UpdateNicknameLabel();
    }
    private void UpdateNicknameLabel()
    {
        if (string.IsNullOrEmpty(_nickname))
        {
            _nicknameLabel.text = "OFFLINE";
            return;
        }

        var nicknameParts = _nickname.Split("#");
        _nicknameLabel.text = nicknameParts[0] + "<color=grey>#" + nicknameParts[1];
    }


    public void ApplyButton()
    {
        _ = ChangeNickname();
    }

    private async UniTask ChangeNickname()
    {
        var newNickname = _nicknameInput.text;
        if (string.IsNullOrEmpty(newNickname))
            return;

        _nicknameInput.text = "";
        _nickname = await AuthenticationService.Instance.UpdatePlayerNameAsync(newNickname);
        UpdateNicknameLabel();
    }
   
}
