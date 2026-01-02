using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] private LayerMask m_botMask;
    [SerializeField] private PlayerHandHitBox m_hitBox;

    private Animator m_animator;
    private PlayerControls m_controls;
    private float m_lastHitTime;
    private bool m_isHit;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_controls = new PlayerControls();
        m_controls.Enable();
        m_controls.PlayerHitMap.LeftMouse.started += HitBot;
        m_hitBox.OnHitBotEnterEvent += OnHitBotEnter;
    }

    private void OnHitBotEnter(Collider obj)
    {
        if (m_isHit)
        {
            print("Попал");
            BotBase botBase = obj.transform.GetComponent<BotBase>();
            botBase.SetStateOfDanger(BotBase.StateOfDanger.Hostile);
        }
    }

    private void OnDisable()
    {
        m_hitBox.OnHitBotEnterEvent -= OnHitBotEnter;
        m_controls.PlayerHitMap.LeftMouse.started -= HitBot;
        m_controls.Disable();
    }

    private void HitBot(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!m_isHit)
        {
            m_animator.SetBool("Hit", true);
            m_isHit = true;
        }
    }

    private void Update()
    {
        if (m_isHit)
        {
            m_lastHitTime += Time.deltaTime;
        }
        if (m_lastHitTime >= 0.28f)
        {
            m_isHit = false;
            m_lastHitTime = 0;
            m_animator.SetBool("Hit", false);
        }
    }
}
