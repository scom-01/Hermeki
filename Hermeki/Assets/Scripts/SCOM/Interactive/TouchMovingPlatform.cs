using System.Collections.Generic;
using UnityEngine;


public class TouchMovingPlatform : TouchObject
{
    public Dictionary<GameObject, Transform> old_transforms = new Dictionary<GameObject, Transform>();

    public override void UnTouch(GameObject obj)
    {
        base.UnTouch(obj);
        //Dictionary에 Object 검색
        if (old_transforms.ContainsKey(obj))
        {
            //Object의 이전 Transform으로 이동
            obj.transform.SetParent(old_transforms[obj]);

            //삭제
            old_transforms.Remove(obj);
        }
    }
    public override void Touch(GameObject obj)
    {
        base.Touch(obj);

        //유닛의 부모 오브젝트 Transform을 저장
        old_transforms.Add(obj, obj.transform.parent);

        //유닛을 MovingObject의 하위로 이동
        obj.transform.SetParent(this.transform);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Touch(collision.collider.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        UnTouch(collision.collider.gameObject);
    }
}
