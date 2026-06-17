using Netologia.Quest;
using Netologia.Quest.Characters.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class Reflector : LaserElementBase
{
    private Vector3 _normal;

    public bool MoveAccess { get; private set; } = true;

    protected override void Start()
    {
        base.Start();
        InformationBureau.OnDialogClose += OnCancel;
    }

    public void SetNormal(Vector3 normal) => _normal = normal;

    public bool OnInteract(PlayerController controller)
    {
        if (MoveAccess)
        {
            MoveAccess = false;
            return true;
        }
        return false;
    }

    private void OnCancel()
    {
        if (MoveAccess) return;
        MoveAccess = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InformationBureau.OnDialogClose -= OnCancel;
    }
}
