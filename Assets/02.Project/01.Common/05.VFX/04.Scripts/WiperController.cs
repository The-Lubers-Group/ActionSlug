using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IgnoreSolutions
{
    public class WiperController : MonoBehaviour
    {
        private Animator _animator;
        private Image _image;

        private readonly int _circleSizeId = Shader.PropertyToID("_CircleSize");

        public float cricleSize = 0;

        private void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
            _image = gameObject.GetComponent<Image>();
        }

        private void Update()
        {
            _image.materialForRendering.SetFloat(_circleSizeId, cricleSize);
        }
    }

}