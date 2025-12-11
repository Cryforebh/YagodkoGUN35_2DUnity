using UnityEngine;

[RequireComponent(typeof(Animation))]
public class RobotEffects : MonoBehaviour
{
    [SerializeField] private Animation m_animation;
    [SerializeField] private AudioClip m_sound;


    private AudioSource m_source;

    private void Awake()
    {
        m_animation = GetComponent<Animation>();
        m_source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        m_animation.Play();
        m_source.clip = m_sound;
        m_source.Play();
    }
}
