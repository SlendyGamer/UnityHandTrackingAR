using System.Globalization;
using UnityEngine;

public class HandTracking3d : MonoBehaviour
{
    [Header("Input")]
    public UDPReceive udpReceive; // Pacote UDP das coordenadas da mao

    [Header("Debug Points")]
    public GameObject[] handPoints; // 21 pontos do MediaPipe

    [Header("Rig")]
    public Transform handRoot; //base da mao (pulso)

    [System.Serializable]
    public class BoneMap
    {
        public Transform bone;  // para cada osso, adicionar um ponto de inicio e um final, sendo atribuidos a coordenadas
        public int start;
        public int end;
        [HideInInspector] public Quaternion offset; //offset da posicao da mao na cena
    }

    [Header("Bones Mapping")]
    public BoneMap[] bones; //cria um mapeamento para os ossos

    Vector3[] landmarks = new Vector3[21]; //landmarks da mao
    bool offsetsCalculated = false;

    void Start()
    {
        Invoke(nameof(CalculateOffsets), 2f); // espera dados chegarem
    }

    void Update()
    {
        //------------- trecho de rastreamento dos pontos da mao -------------------//
        string data = udpReceive.data;
        if (string.IsNullOrEmpty(data)) return;

        string[] points = data.Split(',');
        if (points.Length < 63) return;

        float anguloX = 180f;
        Quaternion rotacaoX = Quaternion.Euler(anguloX, 0f, 0f);

        float xOffset = float.Parse(points[63], CultureInfo.InvariantCulture);
        float yOffset = float.Parse(points[64], CultureInfo.InvariantCulture) * -1;
        float zOffset = float.Parse(points[65], CultureInfo.InvariantCulture) * 5 * -1;

        for (int i = 0; i < 21; i++)
        {
            float x = float.Parse(points[i * 3], CultureInfo.InvariantCulture) + xOffset;
            float y = float.Parse(points[i * 3 + 1], CultureInfo.InvariantCulture) + yOffset;
            float z = float.Parse(points[i * 3 + 2], CultureInfo.InvariantCulture) + zOffset;

            Vector3 ponto = new Vector3(x, y, z) * 20f;

            // ajuste de eixo
            ponto = rotacaoX * ponto;

            ponto.z *= -1f;

            landmarks[i] = ponto;

            if (handPoints != null && handPoints.Length > i) //talvez desnecessario
                handPoints[i].transform.localPosition = ponto;
        }

        UpdateHand();
    }

    void UpdateHand()
    {
        if (landmarks[0] == Vector3.zero) return; //se naore cebeu info, nao atualiza mao

        // posição da mão
        handRoot.localPosition = landmarks[0]; //define pulso

        // rotação da palma
        Vector3 right = (landmarks[5] - landmarks[17]).normalized;
        Vector3 forward = (landmarks[9] - landmarks[0]).normalized;
        Vector3 up = Vector3.Cross(forward, right);

        if (forward != Vector3.zero && up != Vector3.zero)
        {
            Quaternion handRot = Quaternion.LookRotation(forward, up);
            handRoot.rotation = handRot;
        }

        // ossos dos dedos
        foreach (var b in bones)
        {
            Vector3 dir = (landmarks[b.end] - landmarks[b.start]).normalized;
            if (dir == Vector3.zero) continue;

            Quaternion targetRot = Quaternion.LookRotation(dir);

            // suavização opcional
            b.bone.rotation = Quaternion.Slerp(
                b.bone.rotation,
                targetRot * b.offset,
                Time.deltaTime * 15f
            );
        }
    }

    void CalculateOffsets()
    {
        if (offsetsCalculated) return;

        foreach (var b in bones)
        {
            Vector3 dir = (landmarks[b.end] - landmarks[b.start]).normalized;
            if (dir == Vector3.zero) continue;

            b.offset = Quaternion.Inverse(Quaternion.LookRotation(dir)) * b.bone.rotation;
        }

        offsetsCalculated = true;
        Debug.Log("Offsets calculados!");
    }
}