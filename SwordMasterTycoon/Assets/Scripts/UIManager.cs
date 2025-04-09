using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("RGBText")]
    [SerializeField] private Text Rtext;
    [SerializeField] private Text widthtext;
    [SerializeField] private Text Gtext;
    [SerializeField] private Text heightText;
    [SerializeField] private Text Btext;
    [SerializeField] private Text speedText;
    [SerializeField] private Text timerText;

    [Header("Player")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Slider HPBar; // 체력
    [SerializeField] private Image HPFill;
    [SerializeField] private Text HPText;
    [SerializeField] private Text preHPText;
    [SerializeField] private Slider STBar; // 스태미나

    [Header("EndPage")]
    [SerializeField] private GameObject lastPanel;
    [SerializeField] private Text R;
    [SerializeField] private Text G;
    [SerializeField] private Text B;
    [SerializeField] private Text finalSec;
    [SerializeField] private Text score;
    private float timer;
    private bool isFailed = false;
    [SerializeField] private Text failText;
    [SerializeField] private Text clearText;

    [Header("Settings")]
    [SerializeField] private GameObject SettingsObj;
    public bool onSettings = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log(onSettings);
            onSettings = !onSettings;
            OnOffSettings(onSettings);
        }

        SetRGBText();
        SetHP();
        SetHPBarColor();
        SetHPTextColor(50);
        SetPreHPText();
        SetST();
        SetTimer();

        if(player.HP > 0)
        {
            Timer();
        }
        else
        {
            isFailed = true;
            OnLastPanel();
        }

        
    }

    private void SetRGBText()
    {
        Rtext.text = player.Rpoint.ToString();
        Gtext.text = player.Gpoint.ToString();
        Btext.text = player.Bpoint.ToString();

        int widthInt = (int)(1 + player.Rpoint / 100.0f);
        int widthDec = (int)(player.Rpoint % 100.0f);

        int heightInt = (int)(1 + player.Gpoint / 100.0f);
        int heightDec = (int)(player.Gpoint % 100.0f);

        int speedInt = (int)(1 + player.Bpoint * 2.0f / 300.0f);
        int speedDec = (int)(player.Bpoint * 2.0f % 300.0f);

        widthtext.text = "길이 증가 : x" + widthInt + '.' + widthDec.ToString("D2");
        heightText.text = "최대 체력 : " + player.maxHP;
        speedText.text = "속도 : x" + speedInt + '.' + speedDec.ToString("D2");
    }

    private void SetHP()
    {
        HPBar.value = player.HP / player.maxHP;
    }
    private void SetHPBarColor()
    {
        float php = 1 - player.HP / player.maxHP;
        //Debug.Log(php);

        Color newColor = new Color(1.0f * php, (200.0f - (120.0f * php))/255.0f, 80.0f/255.0f); //  0 200 80 -> 255 80 80
        //Debug.Log(newColor);

        HPFill.color = newColor;
    }
    private void SetHPTextColor(float off)
    {
        float php = 1 - player.HP / player.maxHP;

        HPText.color = new Color((255 * php - off)/255.0f, (200 - (120 * php) - off)/255.0f, (80 - off)/255.0f); //  0 200 80 -> 255 80 80
    }

    private void SetPreHPText()
    {
        preHPText.text = player.HP + "/" + player.maxHP;
    }

    private void SetST()
    {
        STBar.value = player.stamina / player.maxStamina;
    }

    private void Timer()
    {
        timer += 1 * Time.deltaTime;
    }

    public void OnLastPanel()
    {
        lastPanel.SetActive(true);
        failText.gameObject.SetActive(isFailed);
        clearText.gameObject.SetActive(!isFailed);

        R.text = player.Rpoint.ToString();
        G.text = player.Gpoint.ToString();
        B.text = player.Bpoint.ToString();

        int lastInt = (int)timer;
        finalSec.text = lastInt.ToString() + "s";

        score.text = ((player.Rpoint * 1.0f + player.Gpoint * 1.5f + player.Bpoint * 1.2f) * (1 + (1000 - lastInt)/1000)).ToString();
    }

    private void OnOffSettings(bool _isOn)
    {
        SettingsObj.gameObject.SetActive(_isOn);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetTimer()
    {
        timerText.text = ("누적 시간 : " + ((int)timer).ToString() + "s");
    }
}
