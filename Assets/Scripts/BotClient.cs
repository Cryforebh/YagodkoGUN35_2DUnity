using UnityEngine;

public class BotClient : MonoBehaviour
{
    private bool m_isClient = true;

    public bool IsClient { get => m_isClient; set => m_isClient = value; }

    private void Awake()
    {
    }
}
