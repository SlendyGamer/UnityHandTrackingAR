using UnityEngine;

public class HandGrabDetector : MonoBehaviour
{
    public Transform grabAnchor; // ponto central da mão (precisa ser atribuído no Inspector!)
    public FingerTipSensor thumbSensor;
    public FingerTipSensor[] fingerSensors;
    //public LineRenderer lr;

    private GameObject heldObject;
    private Rigidbody heldRb;
    private bool isGrabbing;

    private float releaseTimer = 0f;       // Timer para soltar
    public float releaseDelay = 0.5f;      // Delay em segundos

    void Update()
    {
        // Se já está segurando, atualiza posição via física
        bool thumbTouch = thumbSensor.isTouching;
        bool otherTouch = false;
        if (isGrabbing && heldObject)
        {
            // Movimento físico suave (sem teleporte)
            heldRb.MovePosition(grabAnchor.position);
            heldRb.MoveRotation(grabAnchor.rotation);

            foreach (var sensor in fingerSensors)
            {
                if (sensor.isTouching && thumbTouch && sensor.touchedObject == thumbSensor.touchedObject)
                {
                    otherTouch = true;
                    break;
                }
            }

            // Se algum dedo ficou afastado, aumenta timer; se todos voltaram, reseta
            if (!thumbTouch || !otherTouch)
                releaseTimer += Time.deltaTime;
            else
                releaseTimer = 0f;

            // Solta se passou do delay
            if (releaseTimer >= releaseDelay)
                ReleaseObject();

            return;
        }

        // Detecta se todos os dedos (ou os principais) estão tocando
        GameObject candidate = null;

        foreach (var sensor in fingerSensors)
        {
            if (sensor.isTouching && thumbTouch && sensor.touchedObject == thumbSensor.touchedObject)
            {
                otherTouch = true;
                candidate = sensor.touchedObject;
                break;
            }
        }

        //GameObject lineCandidate = null;

        // dedos fechados
        /*
        if (fingersTouching)
        {
            Vector3 start = lr.GetPosition(0);
            Vector3 end = lr.GetPosition(1);

            Vector3 direction = (end - start).normalized;
            float distance = Vector3.Distance(start, end);

            RaycastHit hit;

            if (Physics.Raycast(
                start,
                direction,
                out hit,
                distance))
            {
                lineCandidate = hit.collider.gameObject;
                Debug.Log(hit.collider.name);
            }
        }
        */
        // Iniciar pegada
        if (!isGrabbing && thumbTouch && otherTouch)
        {
            if (candidate)
            {
                GrabObject(candidate);
                releaseTimer = 0f;
            }/*
            else if (lineCandidate)
            {
                GrabObject(lineCandidate);
                releaseTimer = 0f;
            }*/
        }

        // Soltar
        if (isGrabbing && (!thumbTouch || !otherTouch))
        {
            ReleaseObject();
        }
    }

    void GrabObject(GameObject obj)
    {
        heldObject = obj;
        heldRb = obj.GetComponent<Rigidbody>();

        if (!heldRb) return;

        // Desliga gravidade mas mantém física ativa
        heldRb.useGravity = false;

        // Evita colisão com a mão
        Collider[] handCols = GetComponentsInChildren<Collider>();
        Collider objCol = heldObject.GetComponent<Collider>();
        foreach (Collider c in handCols)
            Physics.IgnoreCollision(c, objCol, true);

        isGrabbing = true;
    }

    void ReleaseObject()
    {
        if (!heldObject || !heldRb) return;

        // Reativa gravidade
        heldRb.useGravity = true;

        // Libera colisão com a mão
        Collider[] handCols = GetComponentsInChildren<Collider>();
        Collider objCol = heldObject.GetComponent<Collider>();
        foreach (Collider c in handCols)
            Physics.IgnoreCollision(c, objCol, false);

        heldObject = null;
        heldRb = null;
        isGrabbing = false;
        releaseTimer = 0f;
    }
}
