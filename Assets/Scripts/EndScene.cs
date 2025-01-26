using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(ToTitle), 5f);
    }

    public void ToTitle() {
        SceneManager.LoadScene(0);
    }
}
