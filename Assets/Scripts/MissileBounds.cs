using UnityEngine;
using System.Reflection;

public class MissileBounds : MonoBehaviour
{
    private Vector3 bounds;
    private float width;

    private void Start()
    {
        Debug.LogFormat("{0} Screen dimensions are ({1}, {2})", MethodBase.GetCurrentMethod(), Screen.width, Screen.height);

        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        Debug.LogFormat("{0} Bounds are {1}", MethodBase.GetCurrentMethod(), bounds);

        width = GetComponent<SpriteRenderer>().bounds.size.x;

        Debug.LogFormat("{0} Object width is {1}...", MethodBase.GetCurrentMethod(), width);
    }

    private void LateUpdate()
    {
        Debug.LogFormat("{0} My x position is {1}", MethodBase.GetCurrentMethod(), transform.position.x);

        Vector3 position = transform.position;

        if (position.x > bounds.x || position.y > bounds.y)
        {
            Debug.LogFormat("{0} Destroyed...", MethodBase.GetCurrentMethod());

            Destroy(this.gameObject);
        }
    }
}
