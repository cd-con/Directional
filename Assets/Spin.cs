using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float rotSpeed = 10;
    public float pulseSpeed = 0.75f;
    public float pulseOverdrive = 1f;
    private float scale = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotSpeed * Time.deltaTime);

        Debug.Log($"Scale: {Mathf.PingPong(Time.deltaTime * pulseSpeed, scale)}; Time: {Time.deltaTime * pulseSpeed}");
        transform.localScale = new Vector3(Mathf.PingPong(Time.deltaTime * pulseSpeed, scale) + 1.5f, Mathf.PingPong(Time.deltaTime * pulseSpeed, scale) + 1.5f, 1f);
    }
}
