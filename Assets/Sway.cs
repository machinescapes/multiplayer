using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sway : MonoBehaviour
{
    [Header("Settings")]
    public float swayClamp = 0.09f;
    [Space]
    public float smoothing = 3;



    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(x:Input.GetAxisRaw("Mouse X"), y:Input.GetAxisRaw("Mouse Y"));

        input.x = Mathf.Clamp(input.x, -swayClamp, swayClamp);
        input.y = Mathf.Clamp(input.y, -swayClamp, swayClamp);

        Vector3 target = new Vector3(-input.x, y:-input.y, z:0);

        transform.localPosition = Vector3.Lerp(a:transform.localPosition, b:target + origin, Time.deltaTime * smoothing);
    }
}
