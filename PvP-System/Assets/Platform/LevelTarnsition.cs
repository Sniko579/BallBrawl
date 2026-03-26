using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTarnsition : MonoBehaviour
{
    [SerializeField] Text _text;
    int LevelNumber;


    private void Start()
    {

        int x = SceneManager.GetActiveScene().buildIndex + 1;
        _text.text = "Test Number : " + x;

    }
    public void NextLevel()
    {
        LevelNumber = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(++LevelNumber);
    }
    public void BackLevel()
    {
        LevelNumber = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(--LevelNumber);
    }
    public void ReLoad()
    {
        LevelNumber = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(LevelNumber);
    }
}
