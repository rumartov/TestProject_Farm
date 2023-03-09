using Infrastructure.Factory;
using Services;
using UnityEngine;

namespace Enemy
{
  public class RotateToHero : Follow
  {
    public float Speed;

    private Transform _heroTransform;
    private IGameFactory _gameFactory;
    private Vector3 _positionToLook;

    private void Start()
    {
      _gameFactory = AllServices.Container.Single<IGameFactory>();

      if (IsHeroExist())
        InitializeHeroTransform();
      else
        _gameFactory.PlayerCreated += HeroCreated;
    }

    private void Update()
    {
      if (IsInitialized())
        RotateTowardsHero();
    }

    private void OnDestroy()
    {
      if(_gameFactory != null)
        _gameFactory.PlayerCreated -= HeroCreated;
    }

    private bool IsHeroExist() => 
      _gameFactory.PlayerGameObject != null;

    private void RotateTowardsHero()
    {
      UpdatePositionToLookAt();

      transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
    }

    private void UpdatePositionToLookAt()
    {
      Vector3 positionDelta = _heroTransform.position - transform.position;
      _positionToLook = new Vector3(positionDelta.x, transform.position.y, positionDelta.z);
    }
    
    private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) =>
      Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

    private Quaternion TargetRotation(Vector3 position) =>
      Quaternion.LookRotation(position);

    private float SpeedFactor() =>
      Speed * Time.deltaTime;

    private bool IsInitialized() => 
      _heroTransform != null;
    
    private void HeroCreated() =>
      InitializeHeroTransform();

    private void InitializeHeroTransform() =>
      _heroTransform = _gameFactory.PlayerGameObject.transform;
  }
}