using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour
{
    public enum GameState
    {
        RUNNING,
        PAUSED,
        OVER
    }

    public static GameState state = GameState.RUNNING;

    public static void ChangeState(GameState newState)
    {
        GameMaster.state = newState;
        if (GameMaster.state == GameState.PAUSED || GameMaster.state == GameState.OVER)
        {
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
        }
    }

}
