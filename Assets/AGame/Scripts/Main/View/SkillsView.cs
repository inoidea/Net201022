using UnityEngine;
using UnityEngine.UI;

public class SkillsView : MonoBehaviour
{
    [SerializeField] private Button _boostBtn;
    [SerializeField] private Button _invulnerabilityBtn;
    [SerializeField] private Button _slowdownBtn;
    [SerializeField] private Button _stunBtn;

    private void Awake()
    {
        _boostBtn.onClick.AddListener(SpeedUp);
        _invulnerabilityBtn.onClick.AddListener(MakeInvulnerable);
        _slowdownBtn.onClick.AddListener(SpeedDown);
        _stunBtn.onClick.AddListener(Stop);
    }

    private void SpeedUp()
    {
        Debug.Log("SpeedUp");
    }

    private void SpeedDown()
    {
        Debug.Log("SpeedDown");
    }

    private void MakeInvulnerable()
    {
        Debug.Log("MakeInvulnerable");
    }

    private void Stop()
    {
        Debug.Log("Stop");
    }
}
