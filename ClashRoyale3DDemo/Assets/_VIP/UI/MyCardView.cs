using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class MyCardView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    //TODO：预览区卡牌看要不要设置为不能动
    public MyCard data;
    public int index;//出牌区索引
    private Transform previewHolder;//小兵预览放置位置
    private Camera mainCamera;
    bool isDragging;//是否已经拖拽过了(是否是预览小兵)
    private void Start()
    {
        mainCamera = Camera.main;
        previewHolder = GameObject.Find("PreviewHolder").transform;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //TODO:把改张卡牌放到所有卡牌所在节点的最后一个，使其绘制时在最上层
        transform.SetAsLastSibling();
        //TODO：把敌方区域渲染为禁放区；
    }
    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = eventData.position; 这样直接给屏幕坐标不知道为什么没问题,可能是因为Ui不像3D物体产生近大远小的效果
        //p屏幕坐标一般 不能直接使用，屏幕受自己设置的影响
        //屏幕转世界
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, eventData.position, null, out Vector3 posWorld);
        transform.position = posWorld;
        //从拖拽的卡牌屏幕位置发射射线
        Ray ray = mainCamera.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        //把玩家阵营的卡牌从出牌区拖到地面
        if (Physics.Raycast(ray, out hit,float.PositiveInfinity, 1 << LayerMask.NameToLayer("PlayingField")))
        {
            previewHolder.transform.position = hit.point;//移动到鼠标射线到地面的位置
            if (isDragging == false)
            {
                print("命中地面，没有变小兵");
                //1隐藏改卡牌
                GetComponent<CanvasGroup>().alpha = 0;
                //2从卡牌数组中找到该卡牌数据
                for (int i = 0; i < data.placeablesIndices.Length; i++)
                {
                    int unitId = data.placeablesIndices[i];
                    MyPlaceable p = null;
                    for (int j = 0; j < MyPlaceableModel.instance.list.Count; j++)
                    {
                        if (MyPlaceableModel.instance.list[j].id == unitId)
                        {
                            p = MyPlaceableModel.instance.list[j];
                            break;
                        }
                    }
                    //3取出小兵之间的相对偏移，不定就会站在一起
                    Vector3 offset = data.relativeOffsets[i];

                    //4生成卡牌对应的小兵数组，并且将其设置为预览用的卡牌
                    GameObject unit = Instantiate(Resources.Load<GameObject>(p.associatedPrefab), previewHolder);
                    unit.transform.localPosition = offset;

                    MyPlaceable p2 = p.Clone();
                    p2.faciton = UnityRoyale.Placeable.Faction.Player;//设置为玩家阵营
                    unit.GetComponent<MyPlaceableView>().data = p2;
                }


                isDragging = true;
            }
            else
            {
                print("命中地面卡牌已经变小兵");
            }
        }
        //把小兵从地面拖回出牌区
        else
        {
            if (isDragging)//卡牌已经激活，曾放到场景
            {
                print("卡牌没有命中地面");
                //1标记卡牌为未激活(未显示预览小兵)
                isDragging = false;
                //2显示卡牌
                GetComponent<CanvasGroup>().alpha = 1;
                //3销毁预览用小兵
                foreach (Transform trUnit in previewHolder)
                {
                    Destroy(trUnit.gameObject);
                }
            }

        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Ray ray = mainCamera.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, float.PositiveInfinity, 1 << LayerMask.NameToLayer("PlayingField")))//碰到地面是生成小兵销毁卡牌
        {
            //鼠标松开后检测碰到什么位置
            //碰到地面是生成小兵放在
            OnCardUse();

            //1销毁打出去的牌
            Destroy(this.gameObject);
            //2从预览区取一张卡牌填补到出牌区
           MyCardMgr1.instacne.StartCoroutine(MyCardMgr1.instacne.从预览区发牌到出牌区(index,0.5f));
            //3生成一张卡牌填补到预览区
            MyCardMgr1.instacne.StartCoroutine(MyCardMgr1.instacne.卡牌创建到预览区(0.5f));

        }
        else //鼠标没有命中出牌区，卡牌自动飞回出牌时的位置
        {
            transform.DOMove(MyCardMgr1.instacne.cards[index].position, 2f);
        }
    }

    private void OnCardUse()
    {
        for (int i = previewHolder.childCount - 1; i >= 0; i--)
        {
            Transform trUnit = previewHolder.GetChild(i);
            trUnit.SetParent(MyPlaceableMgr.instance.transform,true);
            MyPlaceableMgr.instance.mine.Add(trUnit.GetComponent<MyPlaceableView>());//添加到自己放小兵列表里
        }
    }
}
