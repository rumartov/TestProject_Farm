using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public CharacterController CharacterController;
    
    private IInputService _inputService;
    private PlayerAnimator _animator;
    private Collider[] _hits = new Collider[3];
    private int _layerMask;
    // TODO integrate static data
    private float _radius;
    private void Awake()
    {
        _inputService = AllServices.Container.Single<IInputService>();
        _animator = GetComponent<PlayerAnimator>();
        
        _layerMask = 1 << LayerMask.NameToLayer("Vegetation");
        _radius = 1f;
    }

    private void Update()
    {
        if (_inputService.IsActionButtonUp() && !_animator.IsSlicing)
        {
            OnAction();
            _animator.PlaySlice();
        }
    }
    
    private void OnAction()
    {
        PhysicsDebug.DrawDebug(StartPoint() + transform.forward, 5, 1.0f);
        for (int i = 0; i < Hit(); ++i)
        {
            _hits[i].GetComponent<Sliceable>().Slice(_hits[i].transform.position, 
                _hits[i].transform.position);
        }
    }

    
    private int Hit() => 
        Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward,
            _radius, _hits, _layerMask);
    

    private Vector3 StartPoint() =>
        new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);
}
