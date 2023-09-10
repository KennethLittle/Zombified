using UnityEngine;

public class KeepMusicPlaying : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
