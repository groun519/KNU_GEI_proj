using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject easyTap;
    [SerializeField] private TextMeshProUGUI text;

    private bool canEasy = false;

    public void OnEasyTap()
    {
        easyTap.SetActive(true);
    }

    public void OffEasyTap()
    {
        easyTap.SetActive(false);
        text.text = "";
    }

    public void StartGame_Easy()
    {
        if(canEasy)
            SceneManager.LoadScene("Easy");
        else
            OnEasyTap();
    }

    public void StartGame_Normal()
    {
        SceneManager.LoadScene("Normal");
    }

    // Update is called once per frame
    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoodText()
    {
        Debug.Log(text.text);
        string txt = text.text.Trim();
        if (txt == "저는 노말 모드도 깨지 못하는 패배자입니다.​")
        {
            canEasy = true;
            OffEasyTap();
            Debug.Log(canEasy);

            StartGame_Easy();
        }
    }
}
