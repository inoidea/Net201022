using UnityEngine;
using UnityEngine.UI;

public class EnterInGameWindow : MonoBehaviour
{
    [SerializeField] private Button _signInBtn;
    [SerializeField] private Button _createAccountBtn;
    [SerializeField] private Canvas _enterInGameCanvas;
    [SerializeField] private Canvas _signInCanvas;
    [SerializeField] private Canvas _createAccountCanvas;

    private void Start()
    {
        _signInBtn.onClick.AddListener(OpenSignInWindow);
        _createAccountBtn.onClick.AddListener(OpenCreateAccountWindow);
    }

    private void OpenSignInWindow()
    {
        _signInCanvas.gameObject.SetActive(true);
        _enterInGameCanvas.gameObject.SetActive(false);
    }

    private void OpenCreateAccountWindow()
    {
        _createAccountCanvas.gameObject.SetActive(true);
        _enterInGameCanvas.gameObject.SetActive(false);
    }
}
