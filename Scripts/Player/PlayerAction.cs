using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public CharacterController CharacterController;
    public GameObject tool;
    
    private IInputService _inputService;
    private PlayerAnimator _animator;
    private Collider[] _hits = new Collider[10];
    private int _layerMask;
    // TODO integrate static data
    private float _radius;
    private void Awake()
    {
        _inputService = AllServices.Container.Single<IInputService>();
        _animator = GetComponent<PlayerAnimator>();
        
        _layerMask = 1 << LayerMask.NameToLayer("Vegetation");
        _radius = 1;
    }

    private void Update()
    {
        if (_inputService.IsActionButtonUp() && !_animator.IsSlicing)
        {
            OnAction();
            _animator.PlaySlice();
        }
    }
    
    //TODO Add OnTriggerEnter Slice Implementation

    private void OnAction()
    {
        PhysicsDebug.DrawDebug(StartPoint() + transform.forward, _radius, 1.0f);
        for (int i = 0; i < Hit(); ++i)
        {
            Vector3 planeWorldPosition = tool.GetComponent<BoxCollider>().size;

            //PhysicsDebug.DrawDebug(planeWorldPosition, _radius, 10.0f);
            _hits[i].GetComponent<Sliceable>().Slice(
                planeWorldPosition);
        }
    }

    
    private int Hit() => 
        Physics.OverlapSphereNonAlloc(StartPoint() /*+ transform.forward*/,
            _radius, _hits, _layerMask);
    

    private Vector3 StartPoint() =>
        new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);
}
