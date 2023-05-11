using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serves as a test for the turn system
/// </summary>
public class DummyGameManager : MonoBehaviour
{
    public TurnManager TurnManager { get; private set; }

    [SerializeField] private int _numberOfTeams;

    // Update is called once per frame
    private void Update()
    {
        if(TurnManager != null)
        {
            TurnManager.UpdateTurnManager();
        }
    }

    public void StartGame()
    {
        TurnManager = new TurnManager(this, _numberOfTeams, 180);
        TurnManager.StartTurn();
    }
}
