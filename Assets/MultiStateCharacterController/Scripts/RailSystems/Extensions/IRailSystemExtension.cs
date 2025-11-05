using MultiStateCharacterController.Scripts.Movement;

namespace MultiStateCharacterController.Scripts.RailSystems.Extensions
{
     public interface IRailSystemExtension
     {
          void OnLatch(MultistateCharacterController characterController);
          void OnUnlatch(MultistateCharacterController characterController);
          void OnKeyUnlatch(MultistateCharacterController characterController);
     }
}
