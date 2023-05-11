using UnityEngine;

/// <summary>
/// Responsible for the management of the turns in the various game modes
/// </summary>
public class TurnManager
{
    //! TESTING VARS
    private DummyGameManager _dummyManager;
    //! TESTING VARS

    private GameManager _manager;
    public PlayerTeam[] Teams => _teams;
    public float CurrTime => _currTime;

    private PlayerTeam[] _teams;
    private int _currentTeam;
    private float _turnTime;
    private float _currTime;
    private bool _startNextTurn;

    //! TESTING CONSTRUCTOR
    public TurnManager(DummyGameManager manager, int numberOfTeams, float turnTime = 120)
    {
        Debug.LogWarning("Used the dummy manager, not to be used in final");
        _dummyManager = manager;
        _teams = new PlayerTeam[numberOfTeams];

        for (int i = 0; i < numberOfTeams; i++)
        {
            _teams[i] = new PlayerTeam();
        }

        if (turnTime < 120)
        {
            _turnTime = 120;
        }
        else
        {
            _turnTime = turnTime;
        }

        _currentTeam = 0;
    }
    //! TESTING CONSTRUCTOR

    /// <summary>
    /// Constructor for TurnManager.
    /// </summary>
    /// <param name="numberOfTeams">Number of teams to be created</param>
    /// <param name="turnTime">The time for each turn. Default 120 seconds (2 min).</param>
    public TurnManager(GameManager manager, int numberOfTeams, float turnTime = 120)
    {
        _manager = manager;
        _teams = new PlayerTeam[numberOfTeams];

        for (int i = 0; i < numberOfTeams; i++)
        {
            _teams[i] = new PlayerTeam();
        }

        if (turnTime < 120)
        {
            _turnTime = 120;
        }
        else
        {
            _turnTime = turnTime;
        }

        _currentTeam = 0;
    }


    public void StartTurn() => OnStartTurn();
    public void UpdateTurnManager() => OnUpdate();
    public void EndTurn() => OnEndTurn();

    public PlayerTeam GetCurrentTeam() => _teams[_currentTeam];

    private void OnStartTurn()
    {
        Debug.Log("Started team " + _currentTeam + " turn \n" + 
                    "Turn time: " + _turnTime);

        _currTime = _turnTime;
        _startNextTurn = false;
    }

    private void OnUpdate()
    {
        _currTime = _currTime - Time.deltaTime;
        Debug.Log(_currTime);
        if (_currTime <= 0 && !_startNextTurn)
        {
            EndTurn();
            _startNextTurn = true;
        }
    }

    private void OnEndTurn()
    {
        Debug.Log("Ended team " + _currentTeam + " turn");
        _currentTeam++;
        if (_currentTeam >= _teams.Length)
        {
            _currentTeam = 0;
        }
    }
}
