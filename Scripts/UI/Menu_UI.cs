using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.LowLevel;

public class Menu_UI : MonoBehaviour
{
    [SerializeField] private TMP_InputField userName_F = null;

    public void userName_UI(string name)
    {
        Player_.userName = userName_F.text;
    }

    public void PlayGame()
    {
        if (string.IsNullOrWhiteSpace(userName_F.text))
        {
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
