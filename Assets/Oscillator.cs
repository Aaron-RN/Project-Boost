using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 3f;

    [SerializeField] [Range(0,1)] float movementFactor;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { period = 3f; }
        float cycles = Time.time / period; //grows continuously as game is active

        const float tau = Mathf.PI * 2f; // approx. 6.28
        float rawSineWav = Mathf.Sin(cycles * tau); //value moves between -1 and 1
        movementFactor = rawSineWav / 2f + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
