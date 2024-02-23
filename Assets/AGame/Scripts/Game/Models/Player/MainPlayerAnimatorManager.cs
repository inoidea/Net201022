using Photon.Pun;
using UnityEngine;

public class MainPlayerAnimatorManager : MonoBehaviourPun//, IMove
{
    [SerializeField] private float _directionDampTime = 0.25f;
    [SerializeField] private float _speed = 20f;

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
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        animator.SetFloat("Speed", moveVertical * Speed);
        animator.SetFloat("Direction", moveHorizontal, _directionDampTime, Time.deltaTime);
    }
}
