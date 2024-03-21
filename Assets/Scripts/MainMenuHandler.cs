using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public TextMeshProUGUI inputField;
    public static string test;

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerNickName"))
            inputField.text = PlayerPrefs.GetString("PlayerNickName");
    }

    public void OnJoinGameClicked()
    {
        PlayerPrefs.SetString("PlayerNickname", inputField.text);
        PlayerPrefs.Save();

        test = (PlayerPrefs.GetString("PlayerNickname"));

        SceneManager.LoadScene("Test");
    }

}
