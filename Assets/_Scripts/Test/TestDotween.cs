using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDotween : MonoBehaviour
{
    public RectTransform target;
    // Start is called before the first frame update
    void Start()
    {
        MoveSpriteInVote();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void MoveSpriteInVote()
    {
        GetComponent<Transform>().DOMove(target.position, 5);
        GetComponent<Transform>().DORotate(new Vector3(0, 0, 180), 5);
    }
}
