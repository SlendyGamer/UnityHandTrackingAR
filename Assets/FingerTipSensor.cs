using UnityEngine;

public class FingerTipSensor : MonoBehaviour
{
    public bool isTouching;
    public GameObject touchedObject;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            isTouching = true;
            touchedObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == touchedObject)
        {
            isTouching = false;
            touchedObject = null;
        }
    }
}
