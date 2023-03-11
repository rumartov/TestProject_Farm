using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Infrastructure.Factory;
using Logic;
using Logic.Vegetation;
using Packages.Rider.Editor.UnitTesting;
using Services.PersistentProgress;
using Services.StaticData;
using StaticData;
using UnityEngine;

public class BarnShop : MonoBehaviour
{
    private IPersistentProgressService _progressService;
    private IStaticDataService _staticData;
    private IGameFactory _factory;
    private bool _sellingItem;

    public void Construct(IPersistentProgressService progressService, IStaticDataService staticData, 
        IGameFactory factory)
    {
        _progressService = progressService;
        _staticData = staticData;
        _factory = factory;
        _sellingItem = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_sellingItem)
            {
                StartCoroutine(SellStack(other));
            }
        }
    }

    private IEnumerator SellStack(Collider other)
    {
        Backpack backpack = other.GetComponentInChildren<Backpack>();

        if (backpack.IsEmpty())
            yield break;

        _sellingItem = true;
        
        GameObject item = backpack.Container.First();
        
        VegetationType plantType = item.GetComponent<Harvest>().VegetationType;

        backpack.UnPackItem(item, transform);
            
        yield return new WaitForSeconds(Constants.StackToCoinDecay);

        PlayAnimation(() => SellItem(plantType));
    }
    
    private void PlayAnimation(Action onComplete)
    {
        GameObject coinIcon = _factory.CreateCoinIcon(gameObject);

        GameObject hud = _factory.Hud;
        coinIcon.transform.SetParent(hud.transform);

        Transform moneyCounter = hud.GetComponentInChildren<MoneyCounter>().transform;
        
        Tweener doMove = coinIcon.transform.DOMove(
            moneyCounter.position, Constants.CoinMove);
        
        Tweener doScale = coinIcon.transform.DOScale(
            Vector3.zero, Constants.CoinScale).Pause();

        doMove.OnUpdate(() =>
        {
            if (Vector3.Distance(moneyCounter.position, coinIcon.transform.position) > 0.5f)
                doMove.ChangeEndValue(moneyCounter.position, true);
            doScale.Play();
        });

        doMove.OnComplete(() =>
        {
            onComplete?.Invoke();
            Destroy(coinIcon);
        });
        
    }

    private void SellItem(VegetationType plantType)
    {
        _progressService.Progress.WorldData.LootData.MoneyData
            .Add(_staticData.ForPlant(plantType).SellCost);
        _sellingItem = false;
    }
}
