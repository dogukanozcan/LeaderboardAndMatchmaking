using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingSystemPresenter : MonoBehaviour
{
    [SerializeField] private Image OpponentAvatar;

    public IEnumerator RandomOpponentAvatarAnimation_Handler(float animateTime)
    {
        float startTime = Time.time;
        float animateProgress = (Time.time - startTime) / animateTime;
        while (animateProgress < 1.0f)
        {
            animateProgress = (Time.time - startTime) / animateTime;
            yield return new WaitForFixedUpdate();
        }
    }

}
