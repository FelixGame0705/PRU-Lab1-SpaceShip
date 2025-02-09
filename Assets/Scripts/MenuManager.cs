using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _colorSelectPanel,
        _attackButton;

    [SerializeField]
    private TextMeshProUGUI _stateText;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnStateChanged;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void GameManagerOnOnStateChanged(GameState state)
    {
        _colorSelectPanel.SetActive(state == GameState.SelectColor);
        _stateText.text = _stateText.color.ToString();
        _stateText.color = Random.ColorHSV();
    }

    public void AttackPressed()
    {
        GameManager.Instance.UpdateGameState(GameState.EnemyTurn);
    }
}
