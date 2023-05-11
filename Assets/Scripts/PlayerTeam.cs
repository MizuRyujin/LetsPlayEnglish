using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeam
{
    private int _score;

    public int Score => _score;

    public PlayerTeam()
    {
        _score = 0;
    }

    public void AddScore() => _score++;
}
