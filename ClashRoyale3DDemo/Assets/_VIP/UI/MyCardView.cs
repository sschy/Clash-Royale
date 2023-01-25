using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MyCardView : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    //TODO：预览区卡牌看要不要设置为不能动
	public MyCard data;
	public int index;
    bool isDragging;//是否已经变小兵
    public void OnPointerDown(PointerEventData eventData)
    {
        //TODO:把改张卡牌放到所有卡牌所在节点的最后一个，使其绘制时在最上层
        transform.SetAsLastSibling();
        //TODO：把敌方区域渲染为禁放区；
    }
    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = eventData.position; 这样直接给屏幕坐标不知道为什么没问题,可能是因为Ui不像3D物体产生近大远小的效果
        //屏幕转世界
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, eventData.position, null, out Vector3 posWorld);
        transform.position = posWorld;
        //从拖拽的卡牌屏幕位置发射射线
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, float.PositiveInfinity, 1 << LayerMask.NameToLayer("PlayingField")))
        {
            if (isDragging==false)
            {
                print("命中地面，没有变小兵");
                //1隐藏改卡牌
                //2从卡牌数组中找到该卡牌数据
                //3取出小兵间相对偏移
                //4生成卡牌对应的小兵数组，并且将其设置为预览用的卡牌
                isDragging = true;
            }
            else
            {
                print("命中地面卡牌已经变小兵");
            }        
        }
        else
        {
            if (isDragging)//卡牌已经激活，曾放到场景
            {
                print("卡牌没有命中地面");
                //1标记卡牌为未激活(未显示预览小兵)
                //2显示卡牌
                //3销毁预览用小兵
            }
           
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, float.PositiveInfinity, 1 << LayerMask.NameToLayer("PlayingField")))//碰到地面是生成小兵销毁卡牌
        {
            //鼠标松开后检测碰到什么位置
            //碰到地面是生成小兵


            //1销毁打出去的牌
            //2从预览区取一张卡牌填补到出牌区
            //3生成一张卡牌填补到预览区
        }      
        else //鼠标没有命中出牌区，卡牌自动飞回出牌时的位置
        {
            transform.DOMove(MyCardMgr1.instacne.cards[index].position,2f);
        }
    }

    private void Awake()
	{
		
	}

}
