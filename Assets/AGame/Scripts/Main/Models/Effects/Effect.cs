using Photon.Pun;
using System;
using UnityEngine;

public class Effect : MonoBehaviourPunCallbacks
{
    const string SKILL_BUFF_BOOST = "skill_buff_boost";
    const string SKILL_BUFF_INVULNERABILITY = "skill_buff_invulnerability";
    const string SKILL_DEBUFF_SLOWDOWN = "skill_debuff_slowdown";
    const string SKILL_DEBUFF_STUN = "skill_debuff_stun";

    private EffectTypes _type;

    public EffectTypes Type => _type;

    private void Awake()
    {
        //SetRandomEffect();
        _type = EffectTypes.Boost;

        PhotonNetwork.LocalPlayer.CustomProperties.Add(SKILL_BUFF_BOOST, false);
        PhotonNetwork.LocalPlayer.CustomProperties.Add(SKILL_BUFF_INVULNERABILITY, false);
        PhotonNetwork.LocalPlayer.CustomProperties.Add(SKILL_DEBUFF_SLOWDOWN, false);
        PhotonNetwork.LocalPlayer.CustomProperties.Add(SKILL_DEBUFF_STUN, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (PhotonNetwork.LocalPlayer)
        //    Debug.Log($"photonView.IsMine YES {photonView.IsMine}");
        //else
        //    Debug.Log($"photonView.IsMine NO {photonView.IsMine}");

        //if (photonView.IsMine)
        //{
            if (other.TryGetComponent(out PhotonView playerPhotonView))
            {
                if (playerPhotonView.IsMine)
                {
                    switch (_type)
                    {
                        case EffectTypes.Boost:
                            var skillProps = PhotonNetwork.LocalPlayer.CustomProperties;
                            skillProps[SKILL_BUFF_BOOST] = true;
                            PhotonNetwork.LocalPlayer.SetCustomProperties(skillProps);
                            Debug.Log("СКИЛЛ ДОБАВЛЕН");
                            break;
                        //case EffectTypes.Invulnerability:
                        //    skills.InvulnEffectExists.Value = true; break;
                        //case EffectTypes.Slowdown:
                        //    skills.SlowdownEffectExists.Value = true; break;
                        //case EffectTypes.Stun:
                        //    skills.StunEffectExists.Value = true; break;
                        default: break;
                    }

                    gameObject.SetActive(false);
                }
            }
        //}
    }

    public void SetRandomEffect()
    {
        var effectIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(EffectTypes)).Length);
        _type = (EffectTypes)effectIndex;
    }
}