using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [Header("Escenario")]
    [SerializeField] private GameObject scene;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;
    [SerializeField] private GameObject enemy3;
    [SerializeField] private GameObject enemy4;
    [SerializeField] private GameObject tumbleweed;

    [Header("Paneles")]
    [SerializeField] private GameObject UIPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button restartButtonWin;
    [SerializeField] private Button mainMenuButtonWin;
    [SerializeField] private Button restartButtonGO;
    [SerializeField] private Button mainMenuButtonGO;

    [Header("Player")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Slider playerLifeBar;

    [Header("Enemy")]
    [SerializeField] private TextMeshProUGUI enemiesLeftText;
    [SerializeField] private TextMeshProUGUI nextEnemyNameText;
    [SerializeField] private Slider nextEnemyLifeBar;

    GuanacoController playerController;
    HareController enemy1Controller;
    FoxController enemy2Controller;
    PumaController enemy3Controller;
    HunterController enemy4Controller;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        playerController = Object.FindFirstObjectByType<GuanacoController>();

        UIPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);

        restartButtonWin.onClick.AddListener(RestartGame);
        mainMenuButtonWin.onClick.AddListener(ReturnToMainMenu);
        restartButtonGO.onClick.AddListener(RestartGame);
        mainMenuButtonGO.onClick.AddListener(ReturnToMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerUI();
        UpdateEnemyUI();
        CheckGameStatus();
    }

    void UpdatePlayerUI()
    {
        if (playerController == null) return;

        if (playerController.lifeGuanaco >= 0)
        {
            UIPanel.SetActive(true);
            gameOverPanel.SetActive(false);
            winPanel.SetActive(false);
        }

        // Debug para ver qué lee el manager exactamente
        Debug.Log($"MANAGER: Vida {playerController.lifeGuanaco} | Enemigos: {GameObject.FindGameObjectsWithTag("Enemy").Length}");
        // Actualizamos barra de vida (asegúrate que el Slider tenga MaxValue en 100 o la vida max)
        playerLifeBar.value = playerController.lifeGuanaco;

        // Actualizamos munición
        ammoText.text = "Munición: " + playerController.currentAmmo.ToString();

        Debug.Log("Vida del Guanaco: " + playerController.lifeGuanaco);
    }

    void UpdateEnemyUI()
    {
        // Aquí podrías buscar cuántos objetos con tag "Enemy" quedan
        int count = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemiesLeftText.text = "Enemigos : " + count;

        // Lógica para detectar al enemigo más cercano o activo y mostrar su vida
        // (Por ahora, si dejas a todos activos, esto mostrará la del que encuentres)
        EnemyBase currentEnemy = FindObjectOfType<EnemyBase>();
        if (currentEnemy != null)
        {
            nextEnemyLifeBar.gameObject.SetActive(true);
            // Suponiendo que agregas variable 'life' a EnemyBase
            nextEnemyLifeBar.value = currentEnemy.health; 
        }
    }

    void CheckGameStatus()
    {
        // Agregamos una validación de seguridad: solo verificar si ha pasado un instante
        if (Time.timeSinceLevelLoad < 0.1f) return;

        // Si no quedan enemigos
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (Time.timeSinceLevelLoad > 0.5f && enemyCount == 0 & playerController.lifeGuanaco > 0)
        {
            scene.SetActive(false);
            player.SetActive(false);
            enemy1.SetActive(false);
            enemy2.SetActive(false);
            enemy3.SetActive(false);
            enemy4.SetActive(false);
            tumbleweed.SetActive(false);
            winPanel.SetActive(true);
            gameOverPanel.SetActive(false);
            UIPanel.SetActive(false);
             Time.timeScale = 0f; // Pausa el juego
        }

        else if (playerController.lifeGuanaco <= 0)
        {
            UIPanel.SetActive(false);
            scene.SetActive(false);
            player.SetActive(false);
            enemy1.SetActive(false);
            enemy2.SetActive(false);
            enemy3.SetActive(false);
            enemy4.SetActive(false);
            tumbleweed.SetActive(false);
            gameOverPanel.SetActive(true);
            winPanel.SetActive(false);
            UIPanel.SetActive(false);
            Time.timeScale = 0f; // Pausa el juego
        }

        
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Nivel1");
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

}
