using UnityEngine;

public class RayGun : MonoBehaviour
{
    public Transform p0;
    public Transform p5;
    public Transform p17;

    public Transform palma;

    public LineRenderer lr;

    public float tamanho = 5f;

    void Start()
    {
        // Pega o LineRenderer do objeto
        lr = GetComponent<LineRenderer>();

        // Quantidade de pontos da linha
        lr.positionCount = 2;

        // Espessura
        lr.startWidth = 0.08f;
        lr.endWidth = 0.08f;

        // Cor
        lr.startColor = Color.cyan;
        lr.endColor = Color.cyan;

        // Material
        Material mat = new Material(Shader.Find("Sprites/Default"));

        // Intensidade do brilho
        mat.color = Color.cyan * 2f;

        lr.material = mat;
    }
    void Update()
    {
        // Vetores do plano
        Vector3 v1 = p5.position - p0.position;
        Vector3 v2 = p17.position - p0.position;

        // Normal = perpendicular ao plano
        Vector3 normal = Vector3.Cross(v2, v1).normalized;

        // Linha
        lr.SetPosition(0, palma.position);

        lr.SetPosition(
            1,
            palma.position + normal * tamanho
        );
    }
}