using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Pixelation.Example.Scripts
{
    public class Plane : MonoBehaviour
    {
        public float PlaneRSpeed;
        public float CubeSSpeed;
        public List<float> CubeRAmplitude = new List<float>(3);

        public List<GameObject> Cubes = new List<GameObject>();

        private float _angle;

        void Update()
        {
            UpdateRotation();
            UpdateCubes();
        }

        private void UpdateRotation()
        {
            transform.rotation *= Quaternion.AngleAxis(
                PlaneRSpeed*Time.deltaTime, Vector3.up);
        }

        private void UpdateCubes()
        {
            _angle += CubeSSpeed*Time.deltaTime;
            float rSin = (float) Math.Sin(_angle);
            for (int i = 0; i < Cubes.Count; i++)
            {
                Cubes[i].transform.localRotation = Quaternion.AngleAxis(rSin*CubeRAmplitude[i], Vector3.up);
            }
        }
    }
}