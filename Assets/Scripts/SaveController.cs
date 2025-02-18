using TMPro;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI topScoreText;

    public void Save(int score)
    {
        if (score > SaveManager.GetInt("TopScore", 0))
            SaveManager.SetInt("TopScore", score);
        SaveManager.Save();
    }

    public void Load()
    {
        SaveManager.Load();
        topScoreText.text = SaveManager.GetInt("TopScore", 0).ToString();
    }
}
