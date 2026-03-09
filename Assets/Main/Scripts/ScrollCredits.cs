using UnityEngine;

namespace Main.Scripts
{
    public class ScrollCredits : MonoBehaviour
    {
        public float scrollSpeed = 50f; // You may need a higher number since we are moving in UI pixels now
        
        private RectTransform rectTransform;

        void Start()
        {
            // Grab the RectTransform component when the script starts
            rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            // Move the Y anchored position up over time
            rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }
}