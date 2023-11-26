using UnityEngine;


public class TrailRendererDestroy : MonoBehaviour
{
    [SerializeField] private float destroyTime = 1f;
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}