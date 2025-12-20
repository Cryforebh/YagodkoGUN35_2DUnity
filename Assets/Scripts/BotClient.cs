using UnityEngine;

public class BotClient : MonoBehaviour
{
    private bool m_isClient = true;
    private AudioSource m_audioSource;

    public bool IsClient { get => m_isClient; set => m_isClient = value; }

    public AudioSource AudioSource => m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
}
