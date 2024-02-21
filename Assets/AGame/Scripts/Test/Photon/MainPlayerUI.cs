using UnityEngine;
using UnityEngine.UI;

public class MainPlayerUI : MonoBehaviour
{
    #region Private Fields

    [Tooltip("Pixel offset from the player target")]
    [SerializeField]
    private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private Text playerNameText;

    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

    [SerializeField] private Slider _playerManaSlider;

    MainPlayerManager target;

    float characterControllerHeight;

    Transform targetTransform;

    Renderer targetRenderer;

    CanvasGroup _canvasGroup;

    Vector3 targetPosition;

    #endregion

    #region MonoBehaviour Messages

    void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();

        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    void Update()
    {
        // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        // Reflect the Player Health
        //if (playerHealthSlider != null)
        //{
        //    playerHealthSlider.value = target.Health;
        //}

        //if (_playerManaSlider != null)
        //    _playerManaSlider.value = target.Mana;

    }

    void LateUpdate()
    {

        // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
        if (targetRenderer != null)
        {
            this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
        }

        // #Critical
        // Follow the Target GameObject on screen.
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;

            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }

    }

    #endregion

    #region Public Methods

    public void SetTarget(MainPlayerManager _target)
    {

        if (_target == null)
        {
            Debug.LogError("<Color=Red><b>Missing</b></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }

        // Cache references for efficiency because we are going to reuse them.
        this.target = _target;
        targetTransform = this.target.GetComponent<Transform>();
        targetRenderer = this.target.GetComponentInChildren<Renderer>();


        CharacterController _characterController = this.target.GetComponent<CharacterController>();

        // Get data from the Player that won't change during the lifetime of this Component
        if (_characterController != null)
        {
            characterControllerHeight = _characterController.height;
        }

        if (playerNameText != null)
        {
            playerNameText.text = this.target.photonView.Owner.NickName;
        }
    }

    #endregion

}
