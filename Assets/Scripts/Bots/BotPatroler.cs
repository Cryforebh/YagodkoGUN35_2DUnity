using UnityEngine;

public class BotPatroler : BotBase
{
    [SerializeField] private string Name = "Патрулирующий Бот";

    public override void LastAwake()
    {
        name = Name;
    }

    private void Start()
    {
        CurrentState = StatePatrol;
        CurrentState.EnterState(this);
    }

    private void Update()
    {
        if (CurrentState.IsComplete == false)
        {
            CurrentState.UpdateState(this);
        }
        else
        {
            CurrentState.ExitState(this);
        }

        TimeDialogDeley();

        if (OtherNearBotsContainer.Count > 0 && IsDialogue == false)
        {
            SwitchState(StateDialogue);
        }
    }

    private void FixedUpdate()
    {
        CurrentState.FixedUpdateState(this);
    }

    public override void LastOnEnable()
    {
    }

    public override void LastOnDisable()
    {
    }
}
