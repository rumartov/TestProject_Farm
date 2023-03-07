using CodeBase.Infrastructure.Factory;
using UnityEngine;

public class Sprout : MonoBehaviour
{
    // TODO add static data
    public float GrowTime = 100;
    
    private VegetationType _vegetationType;
    private float _growTimer;
    private IGameFactory _factory;
    private bool _isCropped;
    private GameObject _plant;

    public void Construct(VegetationType vegetationType, IGameFactory factory)
    {
        _factory = factory;
        _vegetationType = vegetationType;
        _isCropped = true;
        GrowTime = 100;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (IsTimeToGrow())
        {
            Grow();
        }
        else
        {
            CooldownTimer();
        }
    }

    private void CooldownTimer()
    {
        if (_growTimer <= 0)
        {
            _growTimer = 0;
        }
        else
        {
            _growTimer -= Time.deltaTime;
        }
    }

    private bool IsTimeToGrow()
    {
        return _growTimer == 0;
    }

    private void Crop()
    {
        _isCropped = true;
        ResetGrowTimer();
        UnSubscribeSlicedPlant();
    }

    private void Grow()
    {
        if (_isCropped)
        {
            _plant = _factory.CreatePlant(_vegetationType, transform.position, transform);
            SubscribeSlicedPlant();
        }

        _isCropped = false;
    }

    private void UnSubscribeSlicedPlant()
    {
        _plant.GetComponent<Sliceable>().Sliced -= Crop;
    }

    private void SubscribeSlicedPlant()
    {
        _plant.GetComponent<Sliceable>().Sliced += Crop;
    }

    private void ResetGrowTimer()
    {
        _growTimer = GrowTime;
    }
}
