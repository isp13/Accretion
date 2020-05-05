// Simple Sun Shader // Copyright 2016 Kybernetik //

using UnityEngine;

namespace Kybernetik
{
    public sealed class DemoCamera : MonoBehaviour
    {
        /************************************************************************************************************************/

        private const float
            StartupTime = 7,
            SlideDistance = 41,
            SlideSpeed = 1,
            SlideTime = SlideDistance / SlideSpeed,
            SlideWaitTime = 5,
            ViewEachTime = 10,
            ViewEachDistance = 14;

        [SerializeField]
        private GameObject[] _Texts;

        [SerializeField]
        private Transform[] _Targets;

        private Vector3 _StartPosition;
        private float _DemoTimer;
        private Rect _DisplayArea;
        private int _FrameCount;
        private double _FrameTimer;
        private string _FrameText = "60 FPS";

        /************************************************************************************************************************/

        private void Awake()
        {
            _StartPosition = transform.localPosition;
            _DisplayArea = new Rect(Screen.width * 0.5f - 30, 0, 60, 25);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
                Application.Quit();

            _DemoTimer += Time.deltaTime;

            if (_DemoTimer < StartupTime)
                return;

            if (_DemoTimer < StartupTime + SlideTime)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + SlideSpeed * Time.deltaTime, _StartPosition.y, _StartPosition.z);
                return;
            }

            if (_DemoTimer < StartupTime + SlideTime + SlideWaitTime)
            {
                for (int i = 0; i < _Texts.Length; i++)
                {
                    _Texts[i].SetActive(false);
                };
                return;
            }

            transform.localRotation = Quaternion.Euler(0, 0, -90);

            for (int i = 0; i < _Targets.Length; i++)
            {
                if (_DemoTimer < StartupTime + SlideTime + SlideWaitTime + ViewEachTime * (i + 1))
                {
                    Vector3 target = _Targets[i].position;
                    target.z -= ViewEachDistance;
                    transform.localPosition = target;
                    return;
                }
            }

            if (!_Texts[0].activeSelf)
            {
                for (int i = 0; i < _Texts.Length; i++)
                {
                    _Texts[i].SetActive(true);
                };

                transform.localPosition = _StartPosition;
            }

            transform.localRotation = Quaternion.identity;

            if (transform.localPosition.x < _StartPosition.x + SlideDistance)
                transform.localPosition = new Vector3(transform.localPosition.x + SlideSpeed * Time.deltaTime, _StartPosition.y, _StartPosition.z);
        }

        /************************************************************************************************************************/

        private void OnPostRender()
        {
            _FrameCount++;

            _FrameTimer += Time.deltaTime / Time.timeScale;

            if (_FrameTimer >= 1)
            {
                _FrameText = _FrameCount + " FPS";
                _FrameCount = 0;
                _FrameTimer -= 1;
            }
        }

        /************************************************************************************************************************/

        private void OnGUI()
        {
            GUI.Box(_DisplayArea, _FrameText);
        }

        /************************************************************************************************************************/
    }
}