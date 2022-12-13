using UnityEngine;

public class Util : MonoBehaviour {
    public static Vector3 DivideElementWise3(Vector3 a, Vector3 b) {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    public static Vector2 DivideElementWise2(Vector2 a, Vector2 b) {
        var res = DivideElementWise3(new Vector3(a.x, a.y, 1), new Vector3(b.x, b.y, 1));
        return new Vector2(res.x, res.y);
    }
}
