using CodeBase.Hero;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private IInputService _inputService;
    private PlayerAnimator _animator;

    /*public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }*/

    private void Awake()
    {
        _inputService = AllServices.Container.Single<IInputService>();
        _animator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if (_inputService.IsActionButtonUp() && !_animator.IsSlicing)
        {
            _animator.PlaySlice();
        }
    }
}
