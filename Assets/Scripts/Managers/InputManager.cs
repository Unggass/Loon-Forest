using UnityEngine;

public class InputManager : MonoBehaviour
{
    void OnPause()
    {
        Debug.Log("hit Button Pause!");
        GameManager.Instance.PauseGame();
    }
}