using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Factory;
using Logic.Vegetation;
using Services.Input;
using Services.PersistentProgress;
using UnityEngine;

namespace Logic
{
    public class Backpack : MonoBehaviour
    {
        public List<GameObject> Container { get; private set; }
        private int _size;
        private GameObject _backpackVisualItem;
    
        private IInputService _inputService;
        private IPersistentProgressService _progressService;

        public void Construct(int size, IInputService inputService, IPersistentProgressService progressService)
        {
            _size = size;
            Container = new List<GameObject>(_size);

            _inputService = inputService;
            _progressService = progressService;
        }

        private void Update()
        {
            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                PlayAnimation();
            }
        }
    
        private void PlayAnimation()
        {
            //TODO Add dotween animation
        }

        public void AddItem(GameObject item)
        {
            if (Container.Count == _size)
                return;
        
            Container.Add(item);
            item.SetActive(false);
        
            if (_backpackVisualItem == null)
                EnableVisual(item);
            
            _progressService.Progress.WorldData.LootData.StackData.Add(1);

            PlayItemAnimation();
        }

        private void PlayItemAnimation()
        {
            // TODO add dotween animation
        }

        public void RemoveItem(GameObject item)
        {
            if (Container.Count > 0)
            {
                Container.Remove(item);
            }
            
            if (Container.Count == 0)
            {
                DisableVisual();
            }
            
            _progressService.Progress.WorldData.LootData.StackData.Remove(1);
        }

        public void RemoveAllItems()
        {
            foreach (GameObject item in Container.ToList())
            {
                RemoveItem(item);
            }
        }
        
        private void EnableVisual(GameObject item)
        {
            item.SetActive(true);
            _backpackVisualItem = item;
            _backpackVisualItem.GetComponent<Harvest>().enabled = false;
        }

        private void DisableVisual()
        {
            Destroy(_backpackVisualItem);
        }

        public bool IsFull()
        {
            return Container.Count == _size;
        }
    }
}