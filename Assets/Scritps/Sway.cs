using UnityEngine;

public class Sway : MonoBehaviour
{
    private Quaternion originLocalRotation;

    [SerializeField] private float swayAmount = 2f; // Factor de cantidad de sway
    [SerializeField] private float smoothFactor = 5f; // Factor de suavizado

    private void Start()
    {
        originLocalRotation = transform.localRotation;
    }

    void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        // Obtener la entrada del giroscopio
        float t_xLookInput = Input.acceleration.x;
        float t_yLookInput = Input.acceleration.y;

        // Ajustar los ángulos de sway
        Quaternion t_xAngleAdjustment = Quaternion.AngleAxis(t_xLookInput * swayAmount, Vector3.up);
        Quaternion t_yAngleAdjustment = Quaternion.AngleAxis(-t_yLookInput * swayAmount, Vector3.right);

        // Calcular la rotación objetivo
        Quaternion t_targetRotation = originLocalRotation * t_xAngleAdjustment * t_yAngleAdjustment;

        // Aplicar la rotación suavizada
        transform.localRotation = Quaternion.Lerp(transform.localRotation, t_targetRotation, Time.deltaTime * smoothFactor);
    }
}