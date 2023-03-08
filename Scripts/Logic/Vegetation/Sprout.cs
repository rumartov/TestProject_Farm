using CodeBase.Infrastructure.Factory;
using Services.StaticData;
using StaticData;
using UnityEngine;

public class Sprout : MonoBehaviour
{
    private float _growTime;

    private IStaticDataService _staticDataService;
    private IGameFactory _factory;
    
    private VegetationType _vegetationType;
    private float _growTimer;
    private bool _isCropped;
    private GameObject _plant;
    private PlantStaticData _plantStaticData;
    
    public void Construct(VegetationType vegetationType, IGameFactory factory, IStaticDataService staticDataService)
    {
        _factory = factory;
        _staticDataService = staticDataService;
        _vegetationType = vegetationType;
        _isCropped = true;
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
        DropHarvest();
        ResetGrowTimer();
        UnSubscribeSlicedPlant();
    }

    private void DropHarvest()
    {
        _factory.CreateHarvest(_vegetationType, transform.position);
    }

    private void Grow()
    {
        if (_isCropped)
        {
            Plant();
            SubscribeSlicedPlant();
        }

        _isCropped = false;
    }

    private void Plant()
    {
        _plant = _factory.CreatePlant(_vegetationType, transform.position, transform);
        _plantStaticData = _staticDataService.ForPlant(_vegetationType);
        _growTime = _plantStaticData.GrowTime;
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
        _growTimer = _growTime;
    }
}
