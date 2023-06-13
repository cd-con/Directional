using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    private Spin spinner;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Current level: {PlayerPrefs.GetInt("level")}");
        spinner = GetComponent<Spin>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spinner.rotSpeed > 1024)
        {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spinner.rotSpeed += spinner.rotSpeed < 1025 ? 4 : 0;
            PlayerController.Instance.cutscenePlaying = true;
            PlayerController.Instance.movementDirection = transform.position - collision.transform.position;
            if (spinner.rotSpeed == 340 && PlayerController.Instance.isUsingHolyImpulse)
                PlayerController.Instance.UseHolyImpulse();
        }
    }
}
