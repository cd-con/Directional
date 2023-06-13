using UnityEngine;

[CreateAssetMenu(fileName = "DeathCauseMessages", menuName = "GameMessages/Death", order = 1)]
public class DeathMessages : ScriptableObject
{
    public GameStateController.DeathCause cause;
    public string[] deathMessage;
}
