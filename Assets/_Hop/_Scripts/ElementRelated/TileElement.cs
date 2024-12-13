using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Hop._Scripts.ElementRelated
{
    public class TileElement : MonoBehaviour
    {
        private Animator _animator;
        private Collider _collider;
        [SerializeField] private SpriteRenderer diamondSymbol;
        [SerializeField] private Renderer visual;
        private Material _visualMat;

        public float fadeDuration = 1f;
        private float currentLerpTime = 0f;
        private bool fadingOut = true;

        [SerializeField] private List<Color> tileColors;

        void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            _visualMat = visual.materials[0];
            BallController.instance.OnCheckPoint += InstanceOnCheckPoint;
        }

        private void OnDisable()
        {
            BallController.instance.OnCheckPoint -= InstanceOnCheckPoint;
        }

        private void InstanceOnCheckPoint()
        {
            //SetColor(tileColors[_colorCounter++]);
            Color col = TileSpawner.instance.GetCurrentColor();
            SetColor(col);
        }

        public void SetColor(Color color) {
            _visualMat.color = color;
            //else _visualMat.color = Color.cyan;
        }

        private void Update()
        {
            if (diamondSymbol == null) return;
            currentLerpTime += Time.deltaTime / fadeDuration;
            float alpha = fadingOut ? Mathf.Lerp(1, 0, currentLerpTime) : Mathf.Lerp(0, 1, currentLerpTime);
            Color color = diamondSymbol.color;
            color.a = alpha;
            diamondSymbol.color = color;

            if (currentLerpTime >= 1f)
            {
                currentLerpTime = 0f;
                fadingOut = !fadingOut;
            }
        }

        public void OnBallJumped()
        {
            //_collider.enabled = false;
            tag = "Untagged";
            _animator.enabled = true;
        }
    }
}