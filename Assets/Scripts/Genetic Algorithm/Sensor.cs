using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField]
    private float m_MaxDistance = 4.0f;

    [SerializeField]
    private LayerMask m_LayerMask;

    [SerializeField]
    private Color m_RayColorWithCollision = Color.red;

    [SerializeField]
    private Color m_RayColorWithoutCollision = Color.green;

    public float Distance { get; set; }

    public void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, m_MaxDistance, m_LayerMask))
        {
            Distance = hit.distance;
        }
        else
        {
            Distance = m_MaxDistance;
        }

        Debug.DrawRay(transform.position, transform.forward * Distance,  Color.Lerp(m_RayColorWithCollision, m_RayColorWithoutCollision, Distance/m_MaxDistance));
    }
}