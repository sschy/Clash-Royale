using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRoyale;

public class MyCardMgr1 : MonoBehaviour
{
    public static MyCardMgr1 instacne;
    public GameObject cardPrefab; // 卡牌预制体

    public Transform[] cards = new Transform[4]; // 四张活动卡牌

    public Transform startPos, endPos; // 预览卡牌的起始和终止位置

    private Transform previewCard; // 预览卡牌对象

    public Transform canvas; // 动态创建的卡牌要放在canvas下

    private void Awake()
    {
        instacne = this;

    }
    void Start()
    {
        canvas.gameObject.SetActive(true);

        //开始发牌前创建一张
        StartCoroutine(卡牌创建到预览区(.1f)); // 0.1

        for (int i = 0; i < cards.Length; i++)
        {
            StartCoroutine(从预览区发牌到出牌区(i, .4f + i)); // 0.4, 1.4, 2.4, 3.4
            StartCoroutine(卡牌创建到预览区(.8f + i)); // 0.8, 1.8, 2.8, 3.8
        }
    }

    public IEnumerator 卡牌创建到预览区(float delay)
    {
        // TODO:
        yield return new WaitForSeconds(delay);
        print($"AddToDeck");

        int iCard = Random.Range(0, MyCardModel.instance.list.Count);//随机得到卡牌索引
        MyCard card = MyCardModel.instance.list[iCard];//获取卡牌
        GameObject cardPrefab = Resources.Load<GameObject>(card.cardPrefab);//加载卡牌

        previewCard = Instantiate(cardPrefab, canvas).transform;//实例卡牌

        previewCard.GetComponent<MyCardView>().data = card;

        //移动到预览区
        previewCard.position = startPos.position;
        previewCard.localScale = Vector3.one * 0.5f;
        previewCard.DOMove(endPos.position, .2f);
    }

    public IEnumerator 从预览区发牌到出牌区(int i, float delay)
    {
        // TODO:
        yield return new WaitForSeconds(delay);
        print($"PromoteFromDeck");
        //这里有个小BUG预览区区牌可以动,因为索引index是int不给值默认为0
        previewCard.GetComponent<MyCardView>().index = i;

        previewCard.localScale = Vector3.one;
        previewCard.transform.DOMove(cards[i].position, .2f + 0.05f * i); // .2, .25, .3, .35
    }
}
