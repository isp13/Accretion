using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    /// <summary>
    /// Скорость слежения камеры за объектом
    /// </summary>
    public float FollowSpeed = 2f;

    /// <summary>
    /// Объект слежения
    /// </summary>
    public Transform Target;

    /// <summary>
    /// Желаемая координата Z камеры в трехмерном пространстве
    /// </summary>
    public int targetZPostition = -10;

    /// <summary>
    /// Начальный зум камеры
    /// </summary>
    public float Zoom1 = 1f;

    /// <summary>
    /// Желаемый зум камеры
    /// </summary>
    public float Zoom2 = 2f;

    /// <summary>
    /// Интервал плавного зума в секундах
    /// </summary>
    public float duration = 1.0f;

    /// <summary>
    /// 3 интервальная переменная для осуществления плавного зума
    /// </summary>
    /// 
    private float elapsed = 0.0f;

    /// <summary>
    /// Разрешено ли изменять зум
    /// </summary>
    private bool transition = true;


    /// <summary>
    /// Устанавливает стандартную координату камеры при инициализации объекта
    /// </summary>
    private void Start()
    {
        this.transform.position = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Перемещает вслед за объектом слежения. 
    /// </summary>
    private void FixedUpdate()
    {
        Vector3 newPosition = Target.position;
        newPosition.z = targetZPostition;
        transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);


        if (transition && this.tag == "MainCamera")
        {
            elapsed += Time.deltaTime / duration;
            this.GetComponent<Camera>().orthographicSize = Mathf.Lerp(Zoom1, Zoom2, elapsed);
            //this next line i'm not sure of, I'm not familiar with CameraMovement.ypos
           // Camera.main.GetComponent<CameraMovement>().ypos = Mathf.Lerp(ypos1, ypos2, elapsed);
            if (elapsed > 1.0f)
            {
                transition = false;
                Zoom1 = Zoom2;
            }
        }
    }

    /// <summary>
    /// Осуществляет плавный зум
    /// </summary>
    /// <param name="max">вторая координата для зума</param>
    public void SmoothChangeOrthographicSize(float max)
    {
        Zoom2 = max;
        transition = true;
        elapsed = 0.0f;
    }
}
