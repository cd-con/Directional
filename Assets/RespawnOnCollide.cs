using UnityEngine;

public class RespawnOnCollide : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (PlayerController.Instance.cutscenePlaying || PlayerController.Instance.demoMode)
            PlayerController.Instance.UseHolyImpulse();
        else
        {
            GameStateController.Instance.currentGameState = GameStateController.GameState.GAME_OVER;
            if (collision.collider.tag == "Enemy")
            {
                GameStateController.Instance.cause = GameStateController.DeathCause.ENEMY_HIT;
            }
            else
            {
                GameStateController.Instance.cause = GameStateController.DeathCause.WALL_HIT;
            }
            GameStateController.Instance.GenerateDeathCause();
        }
    }
}
