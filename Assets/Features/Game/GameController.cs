using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] UiBindings ui;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] BoardController board;

    public GameState State { get; private set; } = new();

    void Start() => NewGame();

    public void NewGame()
    {
        State.Reset(5);
        ui.Bind(State);
        gameOverScreen.SetActive(false);
        ui.Refresh();
        board.BuildFresh();
    }

    public void OnScored(int points)
    {
        State.AddScore(points);
        ui.Refresh();
    }

    public bool SpendMoveAndCheckGameOver()
    {
        if (!State.TryConsumeMove()) return false;
        ui.Refresh();
        if (State.Moves == 0) gameOverScreen.SetActive(true);
        return true;
    }

    // Task 2 temporary button handler
    // +10 per spec of the test’s placeholder action
    public void MakeMoveTest()
    {
        if (!SpendMoveAndCheckGameOver()) return;
        OnScored(10);
    }
}
