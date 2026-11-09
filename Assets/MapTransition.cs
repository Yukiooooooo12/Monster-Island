using UnityEngine;
using Unity.Cinemachine;

public class MapTransition : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right }

    [SerializeField] PolygonCollider2D mapBoundry;
    [SerializeField] Direction direction;
    CinemachineConfiner2D confiner;

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundry;
            UpdatePlayerPosition(collision.gameObject);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += 2;
                break;
            case Direction.Down:
                newPos.y -= 2;
                break;
            case Direction.Left:
                newPos.x -= 2;
                break;
            case Direction.Right:
                newPos.x += 2;
                break;
        }

        player.transform.position = newPos;
    }
}