using System;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    [Serializable]
    public class PlayerMoveAndRotateAction
    {
        public float Speed = 1f;
        public float Acceleration = 0.5f;
        public float RotationSpeed = 0f;
        public bool RotateThenMove = false;

        public void MoveAndRotate(Character.Character character, PlayerInput playerInput)
        {
            if (Speed + Acceleration + RotationSpeed == 0)
                return;
            MoveAndRotate(character, playerInput.RelativeMovementInput);
        }

        public void MoveAndRotate(Character.Character character, Vector3 input)
        {
            var alignVector = input;
            if (RotateThenMove)
                input = character.transform.forward;

            if (Speed + Acceleration + RotationSpeed == 0)
                return;
            character.AlignToVector(alignVector, RotationSpeed);
            character.Move(Speed, input, Acceleration);
        }
    }
}
