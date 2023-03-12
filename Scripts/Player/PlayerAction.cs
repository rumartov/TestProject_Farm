using System.Collections;
using Logic.Vegetation;
using Services;
using Services.Input;
using UnityEngine;

namespace Player
{
    public class PlayerAction : MonoBehaviour
    {
        public CharacterController CharacterController;
        public GameObject tool;
        private PlayerAnimator _animator;
        private readonly Collider[] _hits = new Collider[10];

        private IInputService _inputService;
        private int _layerMask;
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
                StartCoroutine(OnAction());
                _animator.PlaySlice();
            }
        }

        private IEnumerator OnAction()
        {
            yield return new WaitForSeconds(0.5f);
            PhysicsDebug.DrawDebug(StartPoint() + transform.forward, _radius, 1.0f);
            for (var i = 0; i < Hit(); ++i)
            {
                var planeWorldPosition = tool.GetComponent<BoxCollider>().size;

                //PhysicsDebug.DrawDebug(planeWorldPosition, _radius, 10.0f);
                _hits[i].GetComponent<Sliceable>().Slice(
                    planeWorldPosition);
            }
        }


        private int Hit()
        {
            return Physics.OverlapSphereNonAlloc(StartPoint(),
                _radius, _hits, _layerMask);
        }


        private Vector3 StartPoint()
        {
            return new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);
        }
    }
}