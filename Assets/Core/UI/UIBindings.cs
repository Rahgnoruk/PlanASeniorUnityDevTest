using TMPro;
using UnityEngine;

public class UiBindings : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text movesText;
    GameState state;

    public void Bind(GameState s) { state = s; Refresh(); }
    public void Refresh()
    {
        scoreText.text = $"{state.Score}";
        movesText.text = $"{state.Moves}";
    }
}
