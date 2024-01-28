using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameView : MonoBehaviour
{
    const string HEALTH_POTION_ID = "health_potion_id";
    const string MANA_POTION_ID = "mana_potion_id";

    [Header("Potions")]
    [SerializeField] private Button _healthPotionBtn;
    [SerializeField] private TMP_Text _healthPotionCount;
    [SerializeField] private Button _manaPotionBtn;
    [SerializeField] private TMP_Text _manaPotionCount;

    private string _healthInstanceId;
    private string _manaInstanceId;

    void Start()
    {
        _healthPotionBtn.onClick.AddListener(ConsumeHealthPotion);
        _manaPotionBtn.onClick.AddListener(ConsumeManaPotion);

        //MakePurchase(HEALTH_POTION_ID);
        //MakePurchase(MANA_POTION_ID);

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result => ShowInventory(result.Inventory), OnError);
    }

    private void ShowInventory(List<ItemInstance> inventory)
    {
        inventory.ForEach(item =>
        {
            switch (item.ItemId)
            {
                case HEALTH_POTION_ID:
                    _healthPotionCount.text = item.RemainingUses.ToString();
                    _healthInstanceId = item.ItemInstanceId;
                    break;
                case MANA_POTION_ID:
                    _manaPotionCount.text = item.RemainingUses.ToString();
                    _manaInstanceId = item.ItemInstanceId;
                    break;
                default: break;
            }
        });
    }

    private void MakePurchase(string itemId)
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "main",
            ItemId = itemId,
            Price = 3,
            VirtualCurrency = "SC"
        }, result =>
        {
            Debug.Log("Complete PurchaseItem");
        }, OnError);
    }

    private void ConsumeHealthPotion() 
    {
        ConsumePotion(_healthInstanceId);
        // добавить здоровье
    }

    private void ConsumeManaPotion() 
    { 
        ConsumePotion(_manaInstanceId);
        // добавить ману
    }

    private void ConsumePotion(string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ConsumeCount = 1,
            ItemInstanceId = itemInstanceId,
        }, result =>
        {
            Debug.Log("Complete ConsumeItem");

            if (result.ItemInstanceId == _healthInstanceId)
                _healthPotionCount.text = result.RemainingUses.ToString();

            if (result.ItemInstanceId == _manaInstanceId)
                _manaPotionCount.text = result.RemainingUses.ToString();
        }, OnError);
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
