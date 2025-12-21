using UnityEngine;

[CreateAssetMenu(fileName = "VoiceContainer", menuName = "ScriptableObjects/VoiceContainer", order = 1)]
public class VoiceContainer : ScriptableObject
{
    [SerializeField] private AudioClip[] m_audioClient;
    [SerializeField] private AudioClip[] m_audioDeliveryMan;

    private int m_indexSoundClip;
    private int m_indexSoundClipDeliveryMan;

    public void VoiceClientPlay(BotClient botClient)
    {
        m_indexSoundClip++;
        if (m_indexSoundClip >= m_audioClient.Length) m_indexSoundClip = 0;

        botClient.AudioSource.PlayOneShot(m_audioClient[m_indexSoundClip]);
    }

    public void VoiceDeliveryManPlay(DeliveryMan deliveryMan)
    {
        m_indexSoundClipDeliveryMan++;
        if (m_indexSoundClipDeliveryMan >= m_audioDeliveryMan.Length) m_indexSoundClipDeliveryMan = 0;

        deliveryMan.AudioSource.PlayOneShot(m_audioDeliveryMan[m_indexSoundClipDeliveryMan]);
    }
}
