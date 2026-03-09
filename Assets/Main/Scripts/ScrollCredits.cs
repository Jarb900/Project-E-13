using UnityEngine;

namespace Main.Scripts
{
    public class ScrollCredits : MonoBehaviour
    {
        public float scrollSpeed = 10f;

        void Update()
        {
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
    }
}