using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Load the game scene
    public void PlayGame()
    {
        // Assuming the game scene is at index 1 in the Build Settings
        SceneManager.LoadScene(1);
    }

    // Open the settings menu
    public void OpenSettings()
    {
        // This function could enable a settings panel or navigate to a settings scene
        Debug.Log("Settings button clicked");
        // For example, activate a settings UI panel if you have one:
        // settingsPanel.SetActive(true);
    }

    // Quit the game
    public void QuitGame()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();

        // Note: Application.Quit() only works in a built game, not in the editor.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
