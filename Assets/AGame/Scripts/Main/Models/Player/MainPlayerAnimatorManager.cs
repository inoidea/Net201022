using Photon.Pun;
using UnityEngine;

public class MainPlayerAnimatorManager : MonoBehaviourPun, IMove
{
    const string SKILL_BUFF_BOOST = "skill_buff_boost";

    [SerializeField] private float _directionDampTime = 0.25f;
    [SerializeField] private float _speed = 2f;

    private bool _invulnerable;
    private bool _stunned;
    private Animator animator;

    public float Speed { get { return _speed; } set { _speed = value; } }
    public bool Invulnerable { get { return _invulnerable; } set { _invulnerable = value; } }
    public bool Stunned { get { return _stunned; } set { _stunned = value; } }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void Update()
    {

        // Prevent control is connected to Photon and represent the localPlayer
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        // failSafe is missing Animator component on GameObject
        if (!animator)
        {
            return;
        }

        // deal with Jumping
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // only allow jumping if we are running.
        if (stateInfo.IsName("Base Layer.Run"))
        {
            // When using trigger parameter
            if (Input.GetButtonDown("Fire2")) animator.SetTrigger("Jump");
        }

        // deal with movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        animator.SetFloat("Speed", v * Speed);
        animator.SetFloat("Direction", h, _directionDampTime, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(SKILL_BUFF_BOOST, out object skill);
            Debug.Log($"Кнопка нажата {skill}");
        }
    }
}
