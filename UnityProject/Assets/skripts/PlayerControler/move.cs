using UnityEngine;

public class move : MonoBehaviour
{
    public float panSpeed = 20F;
    public float panBorderThicknes = 10F;
    public Vector3 panLimit;
    public float sctollSpeed = 1000f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w")/* || Input.mousePosition.y >= Screen.height-panBorderThicknes*/)
        {
            pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s")/* || Input.mousePosition.y <= panBorderThicknes*/)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d")/* || Input.mousePosition.x >= Screen.width - panBorderThicknes*/)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a")/* || Input.mousePosition.x <= panBorderThicknes*/)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            pos.y += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            pos.y -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        panSpeed += scroll * sctollSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
        pos.y = Mathf.Clamp(pos.y, -panLimit.z, panLimit.z);

        transform.position = pos;
    }
}
