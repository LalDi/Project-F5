using UnityEngine;

public static class transformExtention
{
    public static void SetX(this Transform transform, float x)
    {
        var newPosition = new Vector3(x, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }

    public static void SetY(this Transform transform, float y)
    {
        var newPosition = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = newPosition;
    }

    public static void SetZ(this Transform transform, float z)
    {
        var newPosition = new Vector3(transform.position.x, transform.position.y, z);
        transform.position = newPosition;
    }

    public static void MoveX(this Transform transform, float x)
    {
        var newPosition = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }

    public static void MoveY(this Transform transform, float y)
    {
        var newPosition = new Vector3(transform.position.x, transform.position.y + y, transform.position.z);
        transform.position = newPosition;
    }

    public static void MoveZ(this Transform transform, float z)
    {
        var newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + z);
        transform.position = newPosition;
    }

    public static void SetX(this RectTransform transform, float x)
    {
        var newPosition = new Vector3(x, transform.position.y, transform.position.z);
        transform.anchoredPosition = newPosition;
    }

    public static void SetY(this RectTransform transform, float y)
    {
        var newPosition = new Vector3(transform.position.x, y, transform.position.z);
        transform.anchoredPosition = newPosition;
    }

    public static void SetZ(this RectTransform transform, float z)
    {
        var newPosition = new Vector3(transform.position.x, transform.position.y, z);
        transform.anchoredPosition = newPosition;
    }

    public static void MoveX(this RectTransform transform, float x)
    {
        var newPosition = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);
        transform.anchoredPosition = newPosition;
    }

    public static void MoveY(this RectTransform transform, float y)
    {
        var newPosition = new Vector3(transform.position.x, transform.position.y + y, transform.position.z);
        transform.anchoredPosition = newPosition;
    }

    public static void MoveZ(this RectTransform transform, float z)
    {
        var newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + z);
        transform.anchoredPosition = newPosition;
    }
}