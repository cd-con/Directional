
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
 public static PlayerController Instance { get; private set; }
    
    public Transform mainCameraTransform;
    public Transform playerTransform;
    public Slider holyImpulseCooldown;

    // Camera settings
    [SerializeField]
    private float cameraMoveTriggerOverdrive = 0.75f;
    [SerializeField]
    private float cameraSpeedOverdrive = 0.75f;
    private bool isReached;

    // Player settings
    private Rigidbody2D playerRigidbody;
    public Vector2 movementDirection;
    public bool cutscenePlaying = true;
    public bool demoMode = true;
    public bool fatalImpulse = false;
    public int demoModeNextTurn = 100;

    public enum Direction
    {
        UP, LEFT, DOWN, RIGHT
    }

    private Direction directionSelector = Direction.UP;

    // Интерфейс
    public Text levelNum;

    // HOLY IMPULSE
    public AudioSource holySoundPlayer;
    public bool isUsingHolyImpulse;

    private void Start()
    {
        Instance = this;
        levelNum.text = $"Level: {PlayerPrefs.GetInt("level")}";
        playerRigidbody = playerTransform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        demoModeNextTurn--;
        if (demoModeNextTurn < 0 && demoMode)
        {
            // ИИ для деморежима
            int rnd = Random.Range(-1, 1);
            Debug.Log($"{directionSelector};{rnd}");
            directionSelector = (directionSelector + rnd < 0) ? Direction.RIGHT : directionSelector + rnd ;
            demoModeNextTurn = Random.Range(50, 500);
            
        }

        if (Input.GetMouseButtonDown(0) && !cutscenePlaying)
            directionSelector++;

        if (Input.GetMouseButtonDown(1) && !cutscenePlaying)
            // Так и должно быть
            directionSelector = directionSelector == 0 ? Direction.RIGHT : directionSelector - 1;

        if (Input.GetKeyDown(KeyCode.Space) && holyImpulseCooldown.value > 99.5 && !isUsingHolyImpulse)
            UseHolyImpulse();

        switch (directionSelector)
        {
            case Direction.UP:
                movementDirection = Vector2.Lerp(movementDirection, transform.up, Time.deltaTime);
                break;
            case Direction.LEFT:
                movementDirection = Vector2.Lerp(movementDirection, transform.right * -1, Time.deltaTime);
                break;
            case Direction.DOWN:
                movementDirection = Vector2.Lerp(movementDirection, transform.up * -1, Time.deltaTime);
                break;
            case Direction.RIGHT:
                movementDirection = Vector2.Lerp(movementDirection, transform.right, Time.deltaTime);
                break;
            default:
                directionSelector = Direction.UP;
                break;
        }
        holyImpulseCooldown.value = isUsingHolyImpulse ? Mathf.Lerp(holyImpulseCooldown.value, 0, Time.deltaTime * 4f) : holyImpulseCooldown.value = Mathf.Lerp(holyImpulseCooldown.value, 100, Time.deltaTime * 0.25f);
        
        
        if (holyImpulseCooldown.value < 0.5f)
            isUsingHolyImpulse = false;
    }
    public void UseHolyImpulse()
    {
        isUsingHolyImpulse = true;
        holySoundPlayer.Play();
        HolyImpulse.Instance.Use(playerTransform.position);
    }

    private void FixedUpdate()
    {
        // Логика камеры
        float x = Mathf.Abs(mainCameraTransform.position.x - playerTransform.position.x);
        float y = Mathf.Abs(mainCameraTransform.position.y - playerTransform.position.y);

        if ((Screen.width / 4 < x * cameraMoveTriggerOverdrive || Screen.height / 4 < y * cameraMoveTriggerOverdrive) && isReached)
            isReached = false;

        if (x < 0.05 && y < 0.05 && !isReached)
            isReached = true;

        if (!isReached)
        {
            Vector2 targetPosition = Vector2.Lerp(mainCameraTransform.position, playerTransform.position, Time.deltaTime * cameraSpeedOverdrive);
            mainCameraTransform.transform.position = new Vector3(targetPosition.x, targetPosition.y, mainCameraTransform.position.z);
        }

        // Обновляем позицию игрока
        playerRigidbody.MovePosition(playerRigidbody.position + movementDirection * 2.5f * Time.fixedDeltaTime);       
    }
}
