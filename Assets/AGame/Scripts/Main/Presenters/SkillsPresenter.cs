using Photon.Pun;
using UnityEngine;

public class SkillsPresenter : MonoBehaviour
{
    [SerializeField] private TimerPool _timerPool;

    private SkillsView _skillsView;
    private float _skillDuration = 7f;
    private IMove _move;
    private float _lowAlpha = 0.3f;
    
    public IMove Move { get { return _move; } set { _move = value; } }

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

        if (isAvailable)
        {
            ITimer timer = _timerPool.CreateCallBackTimer("Boost");

            var prevSpeed = _move.Speed;
            _move.Speed = prevSpeed * 2;
            _move.Invulnerable = true;
            _timerPool.RunTimer(timer, _skillDuration, () => { _move.Speed = prevSpeed; _move.Invulnerable = false; });
        }

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
            ITimer timer = _timerPool.CreateCallBackTimer("Invulnerability");
            _move.Invulnerable = true;
            _timerPool.RunTimer(timer, _skillDuration, () => { _move.Invulnerable = false; });
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
            SlowdownPlayers(10);
        }

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
            SlowdownPlayers(10, 0);
        }

        ChangeStunVisible(false);

        skillProps[Constants.SKILL_DEBUFF_STUN] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
    }

    private void SlowdownPlayers(float radius, float speed = -1)
    {
        if (!_move.PlayerTransform) return;

        var currentPlayerName = _move.PlayerTransform.name;
        Collider[] colliders = Physics.OverlapSphere(_move.PlayerTransform.position, radius);

        foreach (Collider hit in colliders)
        {
            if (hit.TryGetComponent(out IMove playerMove))
            {
                var playerName = playerMove.PlayerTransform.name;

                if (playerName != currentPlayerName)
                {
                    ITimer timer = _timerPool.CreateCallBackTimer($"Slowdown {playerName}");

                    var prevSpeed = playerMove.Speed;
                    playerMove.Speed = (speed == -1) ? prevSpeed / 2 : speed;
                    _timerPool.RunTimer(timer, _skillDuration, () => { playerMove.Speed = prevSpeed; });
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
}
