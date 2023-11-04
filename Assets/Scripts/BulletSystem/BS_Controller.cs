using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(BS_Controller))]
public class BS_ControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BS_Controller controller = (BS_Controller)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Clear Cache"))
        {
            controller.ClearCache();
        }
    }
}
#endif

public class BS_Controller : MonoBehaviour
{

    [Range(0, 40)] public int emitterAmount;
    // emiter cfg
    [Range(-360.0f, 360.0f)] public float pitch;
    [Range(-360.0f, 360.0f)] public float verticalPitch;
    [Range(-360, 360)] public int spreadDegrees = 0;
    [Range(-1f, 1f)] public float spreadRadius = 0;
    public bool autoCompRadius;
    [Range(-360.00f,360.00f)] public float centerRotation = 0;

    public GameObject emitterPrefab;
    private List<GameObject> emitters = new List<GameObject>();
    private List<GameObject> cache = new List<GameObject>();
    private bool isInstantiatingEmitters = false; // Nueva variable para evitar la llamada a SendMessage durante la instanciación

    private void Awake() {
        GameObject[] emitters_ = GameObject.FindGameObjectsWithTag("Emitter");
        foreach(GameObject e in emitters_){
            emitters.Add(e);
        }
    }

    private void Start() {
        OnValidate();
    }

    private void FixedUpdate() {
        OnValidate();
    }

    private void OnValidate()
    {
        // Limpiar emisores existentes
        for (int i = emitters.Count - 1; i >= emitterAmount; i--)
        {
            GameObject emitter = emitters[i];
            if(emitter != null){
                emitter.SetActive(false);
                emitter.name = emitter.name + " (Cached)";
                emitters.RemoveAt(i);
                cache.Add(emitter);
            }
        }
        
        // Instanciar nuevos emisores
        if (emitterAmount > emitters.Count && !isInstantiatingEmitters)
        {
            StartCoroutine(InstantiateEmittersDelayed());
        }
        UpdateEmittersSpread();
        RotateEmittersPitch();
    }

    private IEnumerator InstantiateEmittersDelayed()
    {
        isInstantiatingEmitters = true;
        yield return null; // Esperar hasta la siguiente frame para evitar la llamada a SendMessage
        InstantiateEmitters();
        UpdateEmittersSpread(); // Recalcular parametros
        RotateEmittersPitch();
        isInstantiatingEmitters = false;
    }

    private void InstantiateEmitters()
    {
        for (int i = emitters.Count; i < emitterAmount; i++)
        {
            GameObject newEmitter = Instantiate(emitterPrefab, transform);
            if(newEmitter != null){ // 96
                newEmitter.transform.SetParent(transform, false); // Usar "false" para no llamar a SendMessage
                emitters.Add(newEmitter);
            }
        }
    }

    private void UpdateEmittersSpread()
    {
        float angleIncrement = spreadDegrees / (float)(emitters.Count - 1);
        float currentAngle = -spreadDegrees * 0.5f;

        for (int i = 0; i < emitters.Count; i++)
        {
            GameObject emitter = emitters[i];
            Vector3 emitterPosition = Vector3.zero;
            if(emitter != null){
                if (autoCompRadius)
                {
                    float radians = Mathf.Deg2Rad * (currentAngle + centerRotation);
                    float x = Mathf.Cos(radians) * spreadRadius;
                    float y = Mathf.Sin(radians) * spreadRadius;
                    emitterPosition = new Vector3(x, 0f, y);
                }

                emitter.transform.localPosition = emitterPosition;
                currentAngle += angleIncrement;
            }
        }
    }

    private void RotateEmittersPitch()
{
    Vector3 origin = transform.position;
    for (int i = 0; i < emitters.Count; i++)
    {
        GameObject emitter = emitters[i];
        Vector3 emitterPosition = emitter.transform.position;
        Vector3 direction = origin - emitterPosition;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(verticalPitch, -angle + 90 + pitch, 0);
        emitter.transform.rotation = rotation;
    }
}


    // UNITY EDITOR

#if UNITY_EDITOR
    public void ClearCache()
    {
        foreach (GameObject emitter in cache)
        {
            DestroyImmediate(emitter);
        }
        cache.Clear();
    }
#endif
}
