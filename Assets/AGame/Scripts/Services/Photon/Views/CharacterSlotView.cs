using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _emptyLabel;
    [SerializeField] private GameObject _characterInfoLabel;
    [SerializeField] private TMP_Text _characterNameLabel;
    [SerializeField] private TMP_Text _characterLevelLabel;

    public Button SlotButton => _button;

    public void ShowCharacterInfoSlot(CharacterModel character)
    {
        _characterNameLabel.text = character.Name;
        _characterLevelLabel.text = $"LVL. {character.Level}";

        _button.interactable = false;

        _characterInfoLabel.SetActive(true);
        _emptyLabel.SetActive(false);
    }

    public void ShowEmptySlot()
    {
        _characterInfoLabel.SetActive(false);
        _emptyLabel.SetActive(true);
    }
}
