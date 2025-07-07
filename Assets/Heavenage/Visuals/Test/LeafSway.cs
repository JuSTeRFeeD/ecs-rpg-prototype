using UnityEngine;

public class LeafSway : MonoBehaviour
{
    [Header("Настройки покачивания")]
    [SerializeField] private float swaySpeed = 1f;      // Скорость колебаний
    [SerializeField] private float swayAmountX = 5f;   // Амплитуда по оси X (градусы)
    [SerializeField] private float swayAmountZ = 10f;  // Амплитуда по оси Z (градусы)
    [SerializeField] private bool useRandomOffset = true; // Разные листья качаются несинхронно

    private Vector3 initialRotation;
    private float randomOffset;

    void Start()
    {
        initialRotation = transform.localEulerAngles;
        randomOffset = useRandomOffset ? Random.Range(0f, 100f) : 0f;
    }

    void Update()
    {
        float timeFactor = Time.time * swaySpeed + randomOffset;
        
        // Отдельные колебания по осям X и Z
        float swayX = Mathf.Sin(timeFactor) * swayAmountX;
        float swayZ = Mathf.Cos(timeFactor * 0.7f) * swayAmountZ; // Cos для разнообразия

        // Комбинируем повороты
        transform.localEulerAngles = initialRotation + new Vector3(swayX, 0, swayZ);
    }
}