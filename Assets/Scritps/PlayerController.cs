using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    //public Camera playerCamera;
    public TMP_Text Puntos;
    //public TMP_Text Vida;
    public TMP_Text Ganar;
    private AudioSource audioSource;
    [SerializeField] private AudioClip misionF;

    [Header("General")]
    public int initialHealth = 100;

    private int currentHealth;
    private int totalScore;

    private void Start()
    {
        currentHealth = initialHealth;
        UpdateHealthUI(currentHealth);
        totalScore = 0;
        UpdateScoreUI();

        // Inicializar audioSource
        audioSource = GetComponent<AudioSource>();
    }

    public void Score(int score)
    {
        totalScore += score;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        Puntos.text = "Puntos: " + totalScore;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            SceneManager.LoadScene(0);
        }
        UpdateHealthUI(currentHealth);
    }

    private void UpdateHealthUI(int vida)
    {
        //Vida.text = "Vida: " + vida;
    }

    public void Victoria()
    {
        if (Ganar != null)
        {
            Ganar.text = "¡Bien Hecho Soldado, Ganaste!";
        }
        else
        {
            Debug.LogError("Ganar text component is not assigned.");
        }

        if (audioSource != null && misionF != null)
        {
            audioSource.PlayOneShot(misionF);
        }
        else
        {
            Debug.LogError("AudioSource or misionF is not assigned.");
        }

        Time.timeScale = 0f;
    }
}