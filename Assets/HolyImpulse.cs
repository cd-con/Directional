using UnityEngine;

public class HolyImpulse : MonoBehaviour
{
    public static HolyImpulse Instance;
    public bool playAnim;
    public bool animationRollback;
    private CircleCollider2D stunnTrigger;

    public void Start()
    {
        Instance = this;
        stunnTrigger = GetComponent<CircleCollider2D>();
    }

    public void Use(Vector3 calledPoint)
    {
        transform.localScale = new Vector3(0f, 0f, 1f);
        transform.position = calledPoint;
        playAnim = true;
    }

    private void Update()
    {
        stunnTrigger.enabled = playAnim || animationRollback;

        if (playAnim)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 25, 3f * Time.deltaTime);
            playAnim = !(transform.localScale.x > 24.75f);
            animationRollback = !playAnim;
        }
        if (animationRollback)
        {
            if (transform.localScale.x < 0.1)
            {
                transform.localScale = new Vector3(0f, 0f, 1f);
                animationRollback = false;
                // Сбрасываем
                PlayerController.Instance.fatalImpulse = false;
            }
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 1.75f * Time.deltaTime);
        }
    }
}
