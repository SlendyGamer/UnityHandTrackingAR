using System.Globalization;
using UnityEngine;

public class HandTracking2d : MonoBehaviour
{
    public UDPReceive udpReceive;
    public GameObject[] handPoints;
    void Start()
    {

    }

    void Update()
    {
        string data = udpReceive.data;
        if (string.IsNullOrEmpty(data)) return;

        string[] points = data.Split(',');
        if (points.Length < 63) return;

        float z = float.Parse(points[63], CultureInfo.InvariantCulture) * 50;
        float anguloX = -90f;

        Quaternion rotacaoX = Quaternion.Euler(anguloX, 0f, 0f);

        for (int i = 0; i < 21; i++)
        {
            float x = float.Parse(points[i * 3], CultureInfo.InvariantCulture) / 70; //fazer /100 mas tentar mudar no pycharm primeiro
            float y = float.Parse(points[i * 3 + 1], CultureInfo.InvariantCulture) / 70;

            Vector3 ponto = new Vector3(x, y, z);
            ponto = rotacaoX * ponto;
            handPoints[i].transform.localPosition = ponto;
        }
    }
}
