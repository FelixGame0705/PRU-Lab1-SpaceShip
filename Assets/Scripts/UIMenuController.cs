using UnityEngine;

public class UIMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject Tutorial;

    [SerializeField]
    private GameObject TopScore;

    public void CloseTutorial()
    {
        Tutorial.SetActive(false);
    }

    public void OpenTutorial()
    {
        Tutorial.SetActive(true);
    }

    public void CloseTopScore()
    {
        TopScore.SetActive(false);
    }

    public void OpenTopScore()
    {
        TopScore.SetActive(true);
    }
}
