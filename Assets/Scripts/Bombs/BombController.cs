using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BombController : MonoBehaviour
{

    [SerializeField]
    private Sprite _bombSprite;
    [SerializeField]
    private ZombieBomb _zombieBomb;

    private void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = _bombSprite;
        spriteRenderer.sortingOrder = 1;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }

    void Update()
    {
        if (!_zombieBomb.IsReleased)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _zombieBomb.ReleaseBomb(transform.position);
                Destroy(gameObject);
            }
            DisplayBombOnPosition();
        }

    }

    private void DisplayBombOnPosition()
    {
        // Get the current mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen coordinates to world coordinates
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f; // Make sure the object stays on the same Z-axis

        // Update the position of the object
        transform.position = worldPosition;
    }
}
