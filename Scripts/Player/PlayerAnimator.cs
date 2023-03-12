using System;
using Logic;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int RunHash = Animator.StringToHash("Run");
        private static readonly int SliceHash = Animator.StringToHash("Slice");
        [SerializeField] private CharacterController _characterController;
        [SerializeField] public Animator _animator;

        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _runningStateHash = Animator.StringToHash("Run");
        private readonly int _slicingStateHash = Animator.StringToHash("Slice");
        public bool IsSlicing => State == AnimatorState.Slice;

        private void Update()
        {
            _animator.SetFloat(RunHash, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);
        }

        public AnimatorState State { get; private set; }

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public void PlaySlice()
        {
            _animator.SetTrigger(SliceHash);
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _slicingStateHash)
                state = AnimatorState.Slice;
            else if (stateHash == _runningStateHash)
                state = AnimatorState.Run;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}