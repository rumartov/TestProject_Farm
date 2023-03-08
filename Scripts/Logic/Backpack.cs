using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.Input;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public Action Changed;
    private List<GameObject> Container { get; set; }
    private int _size;
    private GameObject _backpackVisualItem;
    
    private IGameFactory _factory;
    private IInputService _inputService;
    
    public void Construct(int size, IGameFactory factory, IInputService inputService)
    {
        _size = size;
        Container = new List<GameObject>(_size);

        _factory = factory;
        _inputService = inputService;
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
        if (Container.Count < _size)
        {
            Container.Add(item);
            item.SetActive(false);
        }
        if (_backpackVisualItem == null)
        {
            EnableVisual(item);
        }
    }

    public void RemoveItem(GameObject item)
    {
        Container.Remove(item);
        if (Container.Count == 0)
        {
            DisableVisual();
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