using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Task Board Sounds")]
    public AudioClip taskBoardOpen;
    public AudioClip taskBoardClose;
    public AudioClip taskAccept;
    public AudioClip taskComplete; // 添加任务完成音效

    [Header("Interaction Sounds")]
    public AudioClip interactionSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }
}