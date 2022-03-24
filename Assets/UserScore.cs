using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;

public class UserScore
{
    public Player player;
    public int score;

    public UserScore(Player player, int score)
    {
        this.player = player;
        this.score = score;
    }
}
