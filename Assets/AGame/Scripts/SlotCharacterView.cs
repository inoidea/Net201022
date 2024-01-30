using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotCharacterView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _emptySlot;
    [SerializeField] private GameObject _infoCharacterSlot;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _levelLabel;
    [SerializeField] private TMP_Text _goldLabel;

    public Button SlotButton => _button;

    public void ShowInfoCharacterSlot(string name, string level, string gold)
    {
        _nameLabel.text = name;
        _levelLabel.text = $"Level {level}";
        _goldLabel.text = $"Gold {gold}";

        _infoCharacterSlot.SetActive(true);
        _emptySlot.SetActive(false);
    }

    public void ShowEmptySlot()
    {
        _infoCharacterSlot.SetActive(false);
        _emptySlot.SetActive(true);
    }
}
