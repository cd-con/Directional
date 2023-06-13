using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DirectionTag : MonoBehaviour
{
    public bool isPointed;
    public Sprite enabledSprite;
    public Sprite disabledSprite;

    private void Start()
    {
       transform.localScale = new Vector3(0.1f,0.1f,1f);
    }

    private void FixedUpdate()
    {
        if (isPointed)
        {
            GetComponent<SpriteRenderer>().sprite = enabledSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = disabledSprite;
        }
    }
}
