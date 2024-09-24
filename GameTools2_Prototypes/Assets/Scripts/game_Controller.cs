using UnityEngine;

public class game_Controller : MonoBehaviour
{
    public int target_FPS;


    private void Start()
    {
        Application.targetFrameRate = target_FPS;
        QualitySettings.vSyncCount = 0;
    }
}
