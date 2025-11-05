using MultiStateCharacterController.Scripts.Movement;
using UnityEngine;

namespace MultiStateCharacterController.Scripts.SimpleTriggers
{
    public class AnimationTrigger : MonoBehaviour
    {
        [Tooltip("The animator trigger parameter to be set")]
        public string triggerName = "Ani Trigger";

        //Sets the animator trigger
        private void OnTriggerEnter(Collider other)
        {
            MultistateCharacterController player = other.transform.GetComponent<MultistateCharacterController>();
            if (player != null)
            {
                player.localReferences.animator.SetTrigger(triggerName);
                gameObject.SetActive(false);
            }
        }
    }
}
