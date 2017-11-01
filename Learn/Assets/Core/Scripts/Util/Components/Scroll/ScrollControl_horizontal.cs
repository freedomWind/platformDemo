using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;

namespace LeboUnity.Engine
{
    /// <summary>
    /// this script is put on ScrollView object
    /// </summary>
    public class ScrollControl_horizontal : MonoBehaviour, IEndDragHandler, IBeginDragHandler
    {
        private ScrollRect rect;                        //滑动组件  
        private float targethorizontal = 0;             //滑动的起始坐标  
        private bool isDrag = false;                    //是否拖拽结束  
        private List<float> posList = new List<float>();            //求出每页的临界角，页索引从0开始  
        private int currentPageIndex = -1;
        private bool stopMove = true;
        private float startTime;
        private float startDragHorizontal;
        private float _lastFrameTime = 0;
        private int _autoIndex = -1;

        public Action<int> OnPageChanged;
        public float smooting = 4;      //滑动速度  
        public float sensitivity = 0;
        //Pagination
        public GameObject _pagination;


        void Awake()
        {
            rect = transform.GetComponent<ScrollRect>();
            float horizontalLength = rect.content.rect.width - GetComponent<RectTransform>().rect.width;
            posList.Add(0);
            for (int i = 1; i < rect.content.transform.childCount - 1; i++)
            {
                posList.Add(GetComponent<RectTransform>().rect.width * i / horizontalLength);
            }
            posList.Add(1);
            _lastFrameTime = Time.time;
        }

        private void OnEnable()
        {
            _lastFrameTime = Time.time;
        }

        void Update()
        {
            if((Time.time - _lastFrameTime) > 3f && !isDrag)
            {
                _autoIndex += 1;
                pageTo(_autoIndex);
                _lastFrameTime = Time.time;
            }
            if (!isDrag && !stopMove)
            {
                startTime += Time.deltaTime;
                float t = startTime * smooting;
                rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, t);
                if (t >= 1)
                    stopMove = true;
            }
        }

        public void pageTo(int index)
        {
            if (index >= 0 && index < posList.Count)
            {
                rect.DOHorizontalNormalizedPos(posList[index],0.5f);
                SetPageIndex(index);
            }
            else
            {
                _autoIndex = -1;
                Debug.LogWarning("页码不存在");
            }
        }
        private void SetPageIndex(int index)
        {
            if (currentPageIndex != index)
            {
                //归来，yi'wu'feng'yu'yi'wu't
                currentPageIndex = index;
                OnPageChanged?.Invoke(index);
                SetPagination(index);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDrag = true;
            startDragHorizontal = rect.horizontalNormalizedPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            float posX = rect.horizontalNormalizedPosition;
            posX += ((posX - startDragHorizontal) * sensitivity);
            posX = posX < 1 ? posX : 1;
            posX = posX > 0 ? posX : 0;
            int index = 0;
            float offset = Mathf.Abs(posList[index] - posX);
            for (int i = 1; i < posList.Count; i++)
            {
                float temp = Mathf.Abs(posList[i] - posX);
                if (temp < offset)
                {
                    index = i;
                    offset = temp;
                }
            }
            SetPageIndex(index);

            targethorizontal = posList[index]; //设置当前坐标，更新函数进行插值  
            isDrag = false;
            startTime = 0;
            stopMove = false;
        }

        void SetPagination(int curPage)
        {
            if (_pagination)
            {
                for (int i = 0; i < _pagination.transform.childCount; i++)
                {
                    _pagination.transform.GetChild(i).GetComponent<Toggle>().isOn = (curPage == i)
                        ? true
                        : false;
                }
            }
        }
    }
}

