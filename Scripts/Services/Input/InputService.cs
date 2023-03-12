using UnityEngine;

namespace Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        private const string Button = "Fire";

        public abstract Vector2 Axis { get; }

        public bool IsActionButtonUp()
        {
            return SimpleInput.GetButtonUp(Button);
        }

        public bool IsMoving()
        {
            return Axis.sqrMagnitude > Constants.Epsilon;
        }

        protected static Vector2 SimpleInputAxis()
        {
            return new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
        }
    }
}