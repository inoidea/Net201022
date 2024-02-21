﻿using UnityEngine;
using UnityEngine.UI;

public class SkillsView : MonoBehaviour
{
    [SerializeField] private Button _boostBtn;
    [SerializeField] private Button _invulnerabilityBtn;
    [SerializeField] private Button _slowdownBtn;
    [SerializeField] private Button _stunBtn;

    public Button BoostBtn => _boostBtn;
    public Button InvulnerabilityBtn => _invulnerabilityBtn;
    public Button SlowdownBtn => _slowdownBtn;
    public Button StunBtn => _stunBtn;

    public void ChangeButtonImageAlpha(Button button, float alpha)
    {
        button.image.color = SetColorAlpha(button.image.color, alpha);
    }

    private Color SetColorAlpha(Color color, float value)
    {
        color.a = value;

        return color;
    }
}
