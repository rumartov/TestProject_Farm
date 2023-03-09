using System.Collections.Generic;
using Infrastructure.Factory;
using Services.StaticData;
using UnityEngine;

namespace Logic.Vegetation
{
    public class Garden : MonoBehaviour
    {
        private VegetationType _vegetationType;
        private List<Sprout> _sprouts;
        private IGameFactory _factory;
        private IStaticDataService _staticDataService;

        public void Construct(VegetationType vegetationType, IGameFactory factory, IStaticDataService staticDataService)
        {
            _vegetationType = vegetationType;
            _factory = factory;
            _staticDataService = staticDataService;
        
            _sprouts = new List<Sprout>(GetComponentsInChildren<Sprout>());
            ConstructSprouts();
        }

        private void ConstructSprouts()
        {
            foreach (Sprout sprout in _sprouts)
            {
                sprout.Construct(_vegetationType, _factory, _staticDataService);
            }
        }
    }
}