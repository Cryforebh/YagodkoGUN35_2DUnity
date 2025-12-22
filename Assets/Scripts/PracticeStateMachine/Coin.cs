using UnityEngine;

public class Coin : MonoBehaviour
{
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
