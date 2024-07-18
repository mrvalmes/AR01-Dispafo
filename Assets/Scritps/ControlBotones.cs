using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlBotones : MonoBehaviour
{
    public ArmaController armaController;
    public Button fireButton;
    public Button reloadButton;
    public Button exitButton;
    public Button restartButton;

    private void Start()
    {
        fireButton.onClick.AddListener(OnFireButtonClicked);
        reloadButton.onClick.AddListener(OnReloadButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);

    }

    private void OnFireButtonClicked()
    {
        armaController.Fire();
        Debug.Log("Dispara");
    }

    private void OnReloadButtonClicked()
    {
        armaController.Reload();
        Debug.Log("Recarga");
    }

    private void OnExitButtonClicked(){
        Application.Quit();
    }

    private void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
