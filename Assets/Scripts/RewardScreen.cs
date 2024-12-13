using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardScreen : MonoBehaviour
{
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TMP_Text rewardName;

    public void SetRewardScreen(Sprite icon, string name)
    {
        rewardIcon.sprite = icon;
        rewardName.text = name;
    }
}
