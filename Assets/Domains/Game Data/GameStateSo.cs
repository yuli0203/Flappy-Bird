using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameState", order = 1)]
public class GameStateSo : ScriptableObject
{
    public GameStateData gameState;
}

[Serializable]
public class GameStateData 
{
    public bool gameRunning = false;
    public bool gameOver = false;
    public bool restart = false;

    public int diamonds;
}

