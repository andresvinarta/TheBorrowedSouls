using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class splash_script : MonoBehaviour
{

    enum SplashStates { moving, finish};

    SplashStates State;
    public Vector3 velocidad;

    float timeOut = 7.0f;

    float startTime;

    Image image;
    Color32 c;

    // Start is called before the first frame update
    void Start()
    {
        State = SplashStates.moving;
        startTime = Time.time;
        image = GetComponent<Image>();
        c = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case SplashStates.moving:
                transform.Translate(velocidad * Time.deltaTime);

                if (Time.time - startTime > 6.0)
                {
                    if (c.r > 0) c.r -= 2;
                    if (c.g > 0) c.g -= 2;
                    if (c.b > 0) c.b -= 2;
                    image.color = c;
                }

                if (Time.time - startTime > timeOut)
                    State = SplashStates.finish;

                if (Input.GetKey(KeyCode.Escape) ||
                    Input.GetKey(KeyCode.Return) ||
                    Input.GetKey(KeyCode.Space))
                    State = SplashStates.finish;
                break;
            case SplashStates.finish:
                SceneManager.LoadScene("Cinematica");
                break;
            default: break;
        }
    }
}
