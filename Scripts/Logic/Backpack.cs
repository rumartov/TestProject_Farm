using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Logic.Vegetation;
using Services.Input;
using Services.PersistentProgress;
using UnityEngine;

namespace Logic
{
    public class Backpack : MonoBehaviour
    {
        public List<GameObject> Container { get; private set; }
        public Transform itemLook;

        private int _size;
        private GameObject _backpackVisualItem;
    
        private IInputService _inputService;
        private IPersistentProgressService _progressService;

        private bool _isAnimationPlaying;

        public void Construct(int size, IInputService inputService, IPersistentProgressService progressService)
        {
            _size = size;
            Container = new List<GameObject>(_size);
            _isAnimationPlaying = false;
            
            _inputService = inputService;
            _progressService = progressService;
        }

        private void Update()
        {
            RotateBackpack();

            PlayAnimation(_backpackVisualItem);
        }

        private void PlayAnimation(GameObject target)
        {
            if (!_isAnimationPlaying && !IsBackpackVisualNull() && IsPlayerMoving())
            {
                _isAnimationPlaying = true;
                target.transform.DOShakePosition(0.1f, 0.04f)
                    .OnComplete(() => _isAnimationPlaying = false);
            }
        }

        private void RotateBackpack()
        {
            if (!IsBackpackVisualNull())
            {
                _backpackVisualItem.transform.LookAt(itemLook);
            }
        }

        private bool IsPlayerMoving()
        {
            return _inputService.Axis.sqrMagnitude > Constants.Epsilon;
        }

        private bool IsBackpackVisualNull()
        {
            return _backpackVisualItem == null;
        }

        public void PackItem(GameObject item)
        {
            if (Container.Count == _size)
                return;
        
            Container.Add(item);
        
            if (_backpackVisualItem == null)
                EnableVisual(item);
            
            _progressService.Progress.WorldData.LootData.StackData.Add(1);
        }

        public void UnPackItem(GameObject item, Transform target)
        {
            if (Container.Count > 0)
            {
                item.SetActive(true);
                item.GetComponent<Harvest>()
                    .PlayPickUpAnimation(target, 
                        () =>
                        {
                            Container.Remove(item);
                            item.transform.SetParent(target);
                            if (Container.Count == 0)
                                DisableVisual();
                        });
            }
            
            _progressService.Progress.WorldData.LootData.StackData.Remove(1);
        }

        public void UnPackAllItems(Transform target)
        {
            DisableVisual();
            StartCoroutine(Unpack(Container.ToList(), target, 0.1f));
        }

        private IEnumerator Unpack(List<GameObject> container, Transform target, float decay)
        {
            foreach (GameObject item in container)
            {
                yield return new WaitForSeconds(decay);
                UnPackItem(item, target);
            }
        }

        private void EnableVisual(GameObject item)
        {
            item.SetActive(true);
            _backpackVisualItem = item;
            _backpackVisualItem.GetComponent<Harvest>().enabled = false;
            _backpackVisualItem.transform.position = transform.position;
        }

        private void DisableVisual()
        {
            _backpackVisualItem = null;
        }

        public bool IsFull()
        {
            return Container.Count == _size;
        }
    }
}