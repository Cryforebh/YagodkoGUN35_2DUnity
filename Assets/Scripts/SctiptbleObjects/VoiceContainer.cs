using UnityEngine;

[CreateAssetMenu(fileName = "VoiceContainer", menuName = "ScriptableObjects/VoiceContainer", order = 1)]
public class VoiceContainer : ScriptableObject
{
    [SerializeField] private AudioClip[] m_audioClient;
    [SerializeField] private AudioClip[] m_audioDeliveryMan;
    [SerializeField] private AudioClip[] m_audioHomeless;

    private int m_indexSoundClipClient;
    private int m_indexSoundClipDeliveryMan;
    private int m_indexSoundHomeless;


    public void VoiceClientPlay(BotBase botClient)
    {
        m_indexSoundClipClient++;
        if (m_indexSoundClipClient >= m_audioClient.Length) m_indexSoundClipClient = 0;

        botClient.AIAudioSource.PlayOneShot(m_audioClient[m_indexSoundClipClient]);
    }

    public void VoiceDeliveryManPlay(BotBase deliveryMan)
    {
        m_indexSoundClipDeliveryMan++;
        if (m_indexSoundClipDeliveryMan >= m_audioDeliveryMan.Length) m_indexSoundClipDeliveryMan = 0;

        deliveryMan.AIAudioSource.PlayOneShot(m_audioDeliveryMan[m_indexSoundClipDeliveryMan]);
    }

    public void VoiceHomelessPlay(BotBase homeless)
    {
        m_indexSoundHomeless++;
        if (m_indexSoundHomeless >= m_audioHomeless.Length) m_indexSoundHomeless = 0;

        homeless.AIAudioSource.PlayOneShot(m_audioHomeless[m_indexSoundHomeless]);
    }
}
