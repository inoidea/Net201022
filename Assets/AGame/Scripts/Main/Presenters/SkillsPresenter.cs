using Photon.Pun;
using UnityEngine;

public class SkillsPresenter : MonoBehaviour
{
    private SkillsView _skillsView;
    private MainPlayerAnimatorManager _animatorManager;
    private float _lowAlpha = 0.3f;

    public MainPlayerAnimatorManager AnimatorManager { get { return _animatorManager; } set { _animatorManager = value; } }

    private void Awake()
    {
        _skillsView = gameObject.GetComponent<SkillsView>();

        _skillsView.BoostBtn.onClick.AddListener(Boost);
        _skillsView.InvulnerabilityBtn.onClick.AddListener(Invulnerability);
        _skillsView.SlowdownBtn.onClick.AddListener(Slowdown);
        _skillsView.StunBtn.onClick.AddListener(Stun);

        SetAllSkillsVisibility(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Boost();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Invulnerability();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            Slowdown();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            Stun();
    }

    private void Boost()
    {
        var skillProps = PhotonNetwork.LocalPlayer.CustomProperties;
        skillProps.TryGetValue(Constants.SKILL_BUFF_BOOST, out object skillAvailable);
        var isAvailable = (bool)skillAvailable;

        Debug.Log($"Доступно умение SpeedUp {skillAvailable}");

        if (isAvailable)
            _animatorManager.Speed = 1;

        ChangeBoostVisible(false);

        skillProps[Constants.SKILL_BUFF_BOOST] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
    }

    private void Invulnerability()
    {
        var skillProps = PhotonNetwork.LocalPlayer.CustomProperties;
        skillProps.TryGetValue(Constants.SKILL_BUFF_INVULNERABILITY, out object skillAvailable);
        var isAvailable = (bool)skillAvailable;

        Debug.Log($"Доступно умение Invulnerability {skillAvailable}");

        if (isAvailable)
        {
            
        }

        ChangeInvulnerabilityVisible(false);

        skillProps[Constants.SKILL_BUFF_INVULNERABILITY] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
    }

    private void Slowdown()
    {
        var skillProps = PhotonNetwork.LocalPlayer.CustomProperties;
        skillProps.TryGetValue(Constants.SKILL_DEBUFF_SLOWDOWN, out object skillAvailable);
        var isAvailable = (skillAvailable != null) ? (bool)skillAvailable : false;

        Debug.Log($"Доступно умение Slowdown {skillAvailable}");

        if (isAvailable)
            _animatorManager.Speed = 0.2f;

        ChangeSlowdownVisible(false);

        skillProps[Constants.SKILL_DEBUFF_SLOWDOWN] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
    }

    private void Stun()
    {
        var skillProps = PhotonNetwork.LocalPlayer.CustomProperties;
        skillProps.TryGetValue(Constants.SKILL_DEBUFF_STUN, out object skillAvailable);
        var isAvailable = (bool)skillAvailable;

        Debug.Log($"Доступно умение Stun {skillAvailable}");

        if (isAvailable)
        {
        }

        ChangeStunVisible(false);

        skillProps[Constants.SKILL_DEBUFF_STUN] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
    }

    private void SetAllSkillsVisibility(bool visible)
    {
        ChangeBoostVisible(false);
        ChangeInvulnerabilityVisible(false);
        ChangeSlowdownVisible(false);
        ChangeStunVisible(false);
    }

    public void ChangeBoostVisible(bool visible)
    {
        _skillsView.ChangeButtonImageAlpha(_skillsView.BoostBtn, visible ? 1 : _lowAlpha);
    }

    public void ChangeInvulnerabilityVisible(bool visible)
    {
        _skillsView.ChangeButtonImageAlpha(_skillsView.InvulnerabilityBtn, visible ? 1 : _lowAlpha);
    }

    public void ChangeSlowdownVisible(bool visible)
    {
        _skillsView.ChangeButtonImageAlpha(_skillsView.SlowdownBtn, visible ? 1 : _lowAlpha);
    }

    public void ChangeStunVisible(bool visible)
    {
        _skillsView.ChangeButtonImageAlpha(_skillsView.StunBtn, visible ? 1 : _lowAlpha);
    }
}
