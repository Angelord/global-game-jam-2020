using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        RUNNING,
        PAUSED,
        GAME_OVER
    }

    static GameState state;

    static void ChangeState(GameState newState)
    {
        GameManager.state = newState;
    }

}
