using UnityEngine;

public class GameState
{
    public int Score { get; private set; }
    public int Moves { get; private set; }

    public void Reset(int startingMoves = 5) { Score = 0; Moves = startingMoves; }
    public void AddScore(int amount) => Score += amount;

    public bool TryConsumeMove()
    {
        if (Moves <= 0) return false;
        Moves--;
        return true;
    }
}
