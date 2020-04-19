using System;
using UnityEngine;

namespace Assets.Pixelation.Example.Scripts
{
    public class PointLight : MonoBehaviour
    {
        public float RSpeed;
        public float HSpeed;
        public float HAmplitude;
        public GameObject Child;

        private float _angle;

        void Update()
        {
            UpdateRotation();
            UpdateHeight();
        }

        private void UpdateRotation()
        {
            transform.rotation *= Quaternion.AngleAxis(
                RSpeed*Time.deltaTime, Vector3.up);
        }

        private void UpdateHeight()
        {
            _angle += HSpeed*Time.deltaTime;
            float rSin = (float) Math.Sin(_angle);
            Vector3 pos = Child.transform.localPosition;
            pos.y = rSin*HAmplitude;
            Child.transform.localPosition = pos;
        }
    }
}