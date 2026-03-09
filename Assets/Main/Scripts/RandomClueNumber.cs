using TMPro;
using UnityEngine;

namespace Main.Scripts
{
    public class RandomClueNumber : MonoBehaviour
    {
        [SerializeField] private int digitIndex; // which digit to display (0 or 3)
        [SerializeField] private TMP_Text text;

        private void Start()
        {
            if (CodeManager.instance == null)
            {
                Debug.LogError("CodeManager not found in scene!");
                return;
            }
            
            if (CodeManager.instance != null)
            {
                int digit = CodeManager.instance.digits[digitIndex];
                text.text = digit.ToString();
            }
        }
    }
}