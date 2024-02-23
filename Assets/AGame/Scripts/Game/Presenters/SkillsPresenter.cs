using Photon.Pun;
using UnityEngine;

public class SkillsPresenter : MonoBehaviour
{
    [SerializeField] private TimerPool _timerPool;

    private SkillsView _skillsView;
    private float _skillDuration = 7f;
    private IMove _playerMove;
    private float _lowAlpha = 0.3f;

    public IMove PlayerMove { get { return _playerMove; } set { _playerMove = value; } }

    private void Awake()
    {
        _skillsView = gameObject.GetComponent<SkillsView>();

        _skillsView.BoostBtn.onClick.AddListener(Boost);
        _skillsView.InvulnerabilityBtn.onClick.AddListener(Invulnerability);
        _skillsView.SlowdownBtn.onClick.AddListener(Slowdown);
        _skillsView.StunBtn.onClick.AddListener(Stun);

        SetAllSkillsVisibility(false);
        HideSkillTooltip();
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
        var isAvailable = (skillAvailable != null) ? (bool)skillAvailable : false;

        Debug.Log($"Доступно умение Boost {skillAvailable}");

        if (isAvailable)
        {
            ITimer timer = _timerPool.CreateCallBackTimer("Boost");

            _playerMove.Speed = 30;
            _playerMove.Invulnerable = true;
            _timerPool.RunTimer(timer, _skillDuration, () => { _playerMove.Speed = 15; _playerMove.Invulnerable = false; });
        }

        ChangeBoostVisible(false);

        skillProps[Constants.SKILL_BUFF_BOOST] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
    }

    private void Invulnerability()
    {
        var skillProps = PhotonNetwork.LocalPlayer.CustomProperties;
        skillProps.TryGetValue(Constants.SKILL_BUFF_INVULNERABILITY, out object skillAvailable);
        var isAvailable = (skillAvailable != null) ? (bool)skillAvailable : false;

        Debug.Log($"Доступно умение Invulnerability {skillAvailable}");

        if (isAvailable)
        {
            ITimer timer = _timerPool.CreateCallBackTimer("Invulnerability");
            _playerMove.Invulnerable = true;
            _timerPool.RunTimer(timer, _skillDuration, () => { _playerMove.Invulnerable = false; });
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
        {
            SlowdownPlayers(10, SkillTypes.Slowdown);
        }

        ChangeSlowdownVisible(false);

        skillProps[Constants.SKILL_DEBUFF_SLOWDOWN] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
    }

    private void Stun()
    {
        var skillProps = PhotonNetwork.LocalPlayer.CustomProperties;
        skillProps.TryGetValue(Constants.SKILL_DEBUFF_STUN, out object skillAvailable);
        var isAvailable = (skillAvailable != null) ? (bool)skillAvailable : false;

        Debug.Log($"Доступно умение Stun {skillAvailable}");

        if (isAvailable)
        {
            SlowdownPlayers(10, SkillTypes.Stun);
        }

        ChangeStunVisible(false);

        skillProps[Constants.SKILL_DEBUFF_STUN] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
    }

    private void SlowdownPlayers(float radius, SkillTypes skillType)
    {
        if (!_playerMove.PlayerTransform) return;

        var currentPlayerViewID = _playerMove.PlayerViewID;
        Collider[] colliders = Physics.OverlapSphere(_playerMove.PlayerTransform.position, radius);

        foreach (Collider hit in colliders)
        {
            if (hit.TryGetComponent(out IMove playerMove))
            {
                var playerViewID = playerMove.PlayerViewID;
                Debug.Log($"currentPlayerViewID {currentPlayerViewID} playerViewID {playerViewID} = {playerViewID != currentPlayerViewID}");

                if (playerViewID != currentPlayerViewID)
                {
                    
                    playerMove.UseSkillOnPlayer(playerViewID, skillType);
                }
            }
        }
    }

    private void SetAllSkillsVisibility(bool visible)
    {
        ChangeBoostVisible(false);
        ChangeInvulnerabilityVisible(false);
        ChangeSlowdownVisible(false);
        ChangeStunVisible(false);
    }

    public void ChangeBoostVisible(bool visible) =>
        _skillsView.ChangeButtonImageAlpha(_skillsView.BoostBtn, visible ? 1 : _lowAlpha);
    public void ChangeInvulnerabilityVisible(bool visible) =>
        _skillsView.ChangeButtonImageAlpha(_skillsView.InvulnerabilityBtn, visible ? 1 : _lowAlpha);
    public void ChangeSlowdownVisible(bool visible) =>
        _skillsView.ChangeButtonImageAlpha(_skillsView.SlowdownBtn, visible ? 1 : _lowAlpha);
    public void ChangeStunVisible(bool visible) =>
        _skillsView.ChangeButtonImageAlpha(_skillsView.StunBtn, visible ? 1 : _lowAlpha);

    public void SetSkillTooltip(string text) 
    {
        _skillsView.SkillTooltipGO.SetActive(true);
        _skillsView.SkillTooltip.text = text; 
    }

    public void HideSkillTooltip() => 
        _skillsView.SkillTooltipGO.SetActive(false);
}
