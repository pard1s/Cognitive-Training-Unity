using UnityEngine;

public class MazeTile : MonoBehaviour
{
    private bool walkable;

    public void SetWalkable(bool isWalkable)
    {
        walkable = isWalkable;
        // Update tile appearance based on walkable state
    }

    public bool IsWalkable()
    {
        return walkable;
    }
}
