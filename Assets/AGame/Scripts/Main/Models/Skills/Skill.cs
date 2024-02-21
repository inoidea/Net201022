using Photon.Pun;
using System;
using UnityEngine;

public class Skill : MonoBehaviourPunCallbacks
{
    [SerializeField] private SkillsPresenter _skillsPresenter;

    private SkillTypes _type;

    public SkillTypes Type => _type;
    //public SkillsPresenter SkillsPresenter { get { return _skillsPresenter; } set { _skillsPresenter = value; } }

    private void Awake()
    {
        SetRandomSkill();
        //_type = SkillTypes.Boost;

        SetCustomProperties();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PhotonView playerPhotonView))
        {
            if (playerPhotonView.IsMine)
            {
                var skillProps = PhotonNetwork.LocalPlayer.CustomProperties;

                _skillsPresenter.AnimatorManager = other.GetComponent<MainPlayerAnimatorManager>();

                switch (_type)
                {
                    case SkillTypes.Boost:
                        skillProps[Constants.SKILL_BUFF_BOOST] = true;
                        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
                        _skillsPresenter.ChangeBoostVisible(true);
                        break;
                    case SkillTypes.Invulnerability:
                        skillProps[Constants.SKILL_BUFF_INVULNERABILITY] = true;
                        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
                        _skillsPresenter.ChangeInvulnerabilityVisible(true);
                        break;
                    case SkillTypes.Slowdown:
                        skillProps[Constants.SKILL_DEBUFF_SLOWDOWN] = true;
                        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
                        _skillsPresenter.ChangeSlowdownVisible(true);
                        break;
                    case SkillTypes.Stun:
                        skillProps[Constants.SKILL_DEBUFF_STUN] = true;
                        PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
                        _skillsPresenter.ChangeStunVisible(true);
                        break;
                    default: break;
                }
            }

            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void SetRandomSkill()
    {
        var effectIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(SkillTypes)).Length);
        _type = (SkillTypes)effectIndex;
    }

    private void SetCustomProperties()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Add(Constants.SKILL_BUFF_BOOST, false);
        PhotonNetwork.LocalPlayer.CustomProperties.Add(Constants.SKILL_BUFF_INVULNERABILITY, false);
        PhotonNetwork.LocalPlayer.CustomProperties.Add(Constants.SKILL_DEBUFF_SLOWDOWN, false);
        PhotonNetwork.LocalPlayer.CustomProperties.Add(Constants.SKILL_DEBUFF_STUN, false);
    }
}