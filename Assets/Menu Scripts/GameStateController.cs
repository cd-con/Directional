using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{
    private readonly Vector3 oo5 = new Vector3(0.05f, 0.05f, 1f);
    public static GameStateController Instance;


    [Header("Menu layers control")]

    public Transform menuTransform;
    public Transform pauseTransform;
    public Transform gameOverTransform;
    public Text causeText;
    public Vector3 menuStartScale;
    public Vector3 pauseStartScale;
    public Vector3 gameOverStartScale;

    [Header("Menu theme control")]
    public float globalVolume = 1f;
    public AudioSource menuThemePlayer;
    public AudioSource holyImpulsePlayer;

    public enum GameState
    {
        MENU,
        GAME,
        GAME_OVER,
        PAUSE
    }

    public enum DeathCause
    {
        UNDEFINED,
        WALL_HIT,
        ENEMY_HIT,
        ENEMY_HIT_BULLET
    }

    public DeathMessages[] deathMessages;
    public DeathCause cause;

    public GameState currentGameState = GameState.MENU;
    void Start()
    {
        Instance = this;
        menuStartScale = menuTransform.localScale;
        pauseStartScale = pauseTransform.localScale;
        gameOverStartScale = gameOverTransform.localScale;
        menuThemePlayer.volume = 0;
        holyImpulsePlayer.volume = 0;
        menuTransform.localScale = oo5;
        pauseTransform.localScale = oo5;
        gameOverTransform.localScale = oo5;
    }

    // Update is called once per frame
    private bool isSceneLoadedActionOccurred = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.GAME)
            currentGameState = GameState.PAUSE;

        Vector2 targetPos = (Vector2)PlayerController.Instance.mainCameraTransform.position;
        menuTransform.position = targetPos;
        pauseTransform.position = targetPos;
        gameOverTransform.position = targetPos;

        if (currentGameState == GameState.MENU || currentGameState == GameState.PAUSE || currentGameState == GameState.GAME_OVER)
        {
            PlayerController.Instance.cutscenePlaying = true;
            menuThemePlayer.volume = Mathf.Lerp(menuThemePlayer.volume, globalVolume, Time.deltaTime * 2f);
            holyImpulsePlayer.volume = 1 - menuThemePlayer.volume;
        }

        if (currentGameState == GameState.MENU || currentGameState == GameState.GAME)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.deltaTime * 4f);
            pauseTransform.localScale = Vector3.Lerp(pauseTransform.localScale, oo5, Time.deltaTime * 2f);
            holyImpulsePlayer.volume = 1 - menuThemePlayer.volume;
        }

        if (currentGameState == GameState.PAUSE || currentGameState == GameState.GAME_OVER)
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.25f, Time.deltaTime * 2f);

        switch (currentGameState)
        {
            case GameState.MENU:
                PlayerController.Instance.demoMode = true;
                if (isSceneLoadedActionOccurred)
                {
                    isSceneLoadedActionOccurred = false;
                    SceneManager.UnloadSceneAsync(1);
                }
                gameOverTransform.localScale = Vector3.Lerp(gameOverTransform.localScale, oo5, Time.deltaTime * 2f);
                menuTransform.localScale = Vector3.Lerp(menuTransform.localScale, menuStartScale, Time.deltaTime * 2f);
                break;
            case GameState.GAME:
                if (!isSceneLoadedActionOccurred)
                {
                    isSceneLoadedActionOccurred = true;
                    SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
                }

                if ((oo5 - menuTransform.localScale).magnitude > 0.025 || (oo5 - pauseTransform.localScale).magnitude > 0.025 || (oo5 - gameOverTransform.localScale).magnitude > 0.025)
                {
                    menuThemePlayer.volume = Mathf.Lerp(menuThemePlayer.volume, 0, Time.deltaTime * 2f);
                    menuTransform.localScale = Vector3.Lerp(menuTransform.localScale, oo5, Time.deltaTime * 2f);
                    gameOverTransform.localScale = Vector3.Lerp(gameOverTransform.localScale, oo5, Time.deltaTime * 2f);
                }
                else
                {
                    PlayerController.Instance.cutscenePlaying = false;
                    PlayerController.Instance.demoMode = false;
                    // Принудительно прижимаем плеер
                    menuThemePlayer.volume = 0;
                }
                break;
            case GameState.PAUSE:
                PlayerController.Instance.cutscenePlaying = true;
                // Чтобы замедление не влияло на время открытия меню - просто умножаем на 4
                pauseTransform.localScale = Vector3.Lerp(pauseTransform.localScale, pauseStartScale, Time.deltaTime * 2f);
                break;
            case GameState.GAME_OVER:
                if (isSceneLoadedActionOccurred)
                {
                    isSceneLoadedActionOccurred = false;
                    SceneManager.UnloadSceneAsync(1, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                }
                gameOverTransform.localScale = Vector3.Lerp(gameOverTransform.localScale, gameOverStartScale, Time.deltaTime * 8f);
                break;
            default:
                break;
        }
    }

    public void GenerateDeathCause()
    {
        foreach (DeathMessages message in deathMessages)
        {
            if (message.cause == cause)
            {
                causeText.text = "Причина смерти: " + message.deathMessage[Random.Range(0, message.deathMessage.Length - 1)];
                break;
            }
        }
    }

    public void PlayClick()
    {
        if (currentGameState == GameState.MENU)
        {
            PlayerController.Instance.fatalImpulse = true;
            PlayerController.Instance.UseHolyImpulse();
        }
        currentGameState = GameState.GAME;
    }

    public void RestartClick()
    {
        var removeList = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in removeList)
        {
            Destroy(enemy);
        }
        PlayClick();
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void MenuClick() => currentGameState = GameState.MENU;
}
