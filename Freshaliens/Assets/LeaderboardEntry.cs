using UnityEngine;
using TMPro;

namespace Freshaliens.UI
{
    public class LeaderboardEntry : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private TextMeshProUGUI nameLabel;
        [SerializeField] private TextMeshProUGUI timeLabel;

        public void SetActive(bool value) => gameObject.SetActive(value);
        public void SetText((string name, string time) args) => SetText(args.name, args.time);
        public void SetText(string name, string time) {
            nameLabel.SetText(name);
            timeLabel.SetText(time);
        }
    }
}