using System.Collections;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

public class Garden : MonoBehaviour
{
    private VegetationType _vegetationType;
    private List<Sprout> _sprouts;
    private IGameFactory _factory;
    
    public void Construct(VegetationType vegetationType, IGameFactory factory)
    {
        _vegetationType = vegetationType;
        _factory = factory;

        _sprouts = new List<Sprout>(GetComponentsInChildren<Sprout>());
        ConstructSprouts();
    }

    private void ConstructSprouts()
    {
        foreach (Sprout sprout in _sprouts)
        {
            sprout.Construct(_vegetationType, _factory);
        }
    }
}

public enum VegetationType
{
    Wheat
}
