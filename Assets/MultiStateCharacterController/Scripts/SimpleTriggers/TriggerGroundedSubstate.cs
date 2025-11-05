using MultiStateCharacterController.Scripts.Movement;
using UnityEngine;

namespace MultiStateCharacterController.Scripts.SimpleTriggers
{
    public class TriggerGroundedSubstate : MonoBehaviour
    {
        [Tooltip("New player grounded substate")]
        public GroundedMovementSubState groundedMovementSubState;
        [Tooltip("Is this trigger disabled after its activation?")]
        public bool disableOnTrigger = false;

        //Sets a new grounded substate
        private void OnTriggerEnter(Collider other)
        {
            MultistateCharacterController player = other.transform.GetComponent<MultistateCharacterController>();
            if (player != null)
            {
                player.SetGroundedMovementSubstate(groundedMovementSubState);
            }
        }
    }
}
