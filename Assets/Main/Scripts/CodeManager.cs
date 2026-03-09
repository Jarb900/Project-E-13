using UnityEngine;

namespace Main.Scripts
{
    public class CodeManager : MonoBehaviour
    {
        public static CodeManager instance;

        public int keypadCode;
        public int[] digits = new int[4];

        private void Awake()
        {
            // Singleton instance
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            GenerateCode();
        }

        private void GenerateCode()
        {
            digits[0] = Random.Range(0, 10); // random
            digits[1] = 7;                   // fixed
            digits[2] = 6;                   // fixed
            digits[3] = Random.Range(0, 10); // random
                           

            keypadCode = int.Parse($"{digits[0]}{digits[1]}{digits[2]}{digits[3]}");

            Debug.Log("Generated Safe Code: " + keypadCode);
        }
    }
}