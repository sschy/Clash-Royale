using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU : MonoBehaviour
{
    public float interval = 5;//出牌时间
    void Start()
    {
        StartCoroutine(CardOut());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CardOut()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            var cardList = MyCardModel.instance.list;
            var caraData = cardList[Random.Range(0, cardList.Count)];
            MyCardView.CreatePlaceable(
                caraData,
                new Vector3(Random.Range(-9f, 9f),0,
                Random.Range(7, 2)), 
                MyPlaceableMgr.instance.transform,UnityRoyale.Placeable.Faction.Opponent);
        }
    }

}
