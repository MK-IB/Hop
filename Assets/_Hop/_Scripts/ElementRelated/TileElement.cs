using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

        //MOVING RELATED
        private bool _isMovingTile;
        private Vector3 _targetPos, _prevPos, tempPos;

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

        public void SetColor(Color color)
        {
            _visualMat.color = color;
            //else _visualMat.color = Color.cyan;
        }

        public IEnumerator EnableMovement()
        {
            float scaleX = transform.localScale.x;
            transform.DOMoveX(transform.position.x - scaleX, .2f);
            yield return new WaitForSeconds(0.35f);
            _prevPos = transform.position;
            _targetPos = transform.position + Vector3.right * scaleX * 2;
            _isMovingTile = true;
            Debug.Log("MOVING");
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

            if (_isMovingTile)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * 5f);

                if (Vector3.Distance(transform.position, _targetPos) < 0.1f)
                {
                    tempPos = _targetPos;
                    _targetPos = _prevPos;
                    _prevPos = tempPos;
                }
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