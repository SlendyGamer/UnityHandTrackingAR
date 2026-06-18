using UnityEngine;

public class BaricentroCalc : MonoBehaviour
{
    public Transform[] pontos = new Transform[5];

    void Update()
    {
        Vector3 baricentro = CalcularBaricentro3D();
        transform.position = baricentro;
    }

    Vector3 CalcularBaricentro3D()
    {
        Vector3 soma = Vector3.zero;

        for (int i = 0; i < pontos.Length; i++)
        {
            soma += pontos[i].position;
        }

        return soma / pontos.Length;
    }
}