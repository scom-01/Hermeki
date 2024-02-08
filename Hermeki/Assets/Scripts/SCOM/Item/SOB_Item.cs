using SCOM.CoreSystem;
using UnityEngine;
using SCOM;
namespace SCOM.Item
{
    public class SOB_Item : MonoBehaviour
    {
        /// <summary>
        /// 아이템 Data
        /// </summary>
        [field: SerializeField] public StatsItemSO Item;
        public Core ItemCore { get; private set; }
        [HideInInspector] public Unit unit;

        /// <summary>
        /// 스폰 Effect
        /// </summary>
        public EffectPrefab SpawnEffectObject;

        /// <summary>
        /// 삭제 Effect
        /// </summary>
        public EffectPrefab DestroyEffectObject;

        /// <summary>
        /// Player의 Item 감지 범위
        /// </summary>
        [SerializeField] private float DetectedRadius;

        /// <summary>
        /// Item SpriteRenderer
        /// </summary>
        [SerializeField] private SpriteRenderer SR;

        /// <summary>
        /// Unit과 접촉하는 Collider2D
        /// </summary>
        private CircleCollider2D CC2D;

        /// <summary>
        /// Ground와 접촉하는 Collider2D
        /// </summary>
        private BoxCollider2D BC2D;
        private Transform effectContainer;

        /// <summary>
        /// 바닥 포지션
        /// </summary>
        private Vector3 GroundPos;

        private void Awake()
        {
            BC2D = this.GetComponent<BoxCollider2D>();
            effectContainer = GameObject.FindGameObjectWithTag("EffectContainer").transform;
            GroundPos = new Vector2(transform.position.x + BC2D.offset.x, transform.position.y + BC2D.offset.y - (BC2D.size.y / 2));
            if (SR == null)
            {
                SR = this.GetComponent<SpriteRenderer>();
            }
        }
        private void OnEnable()
        {
            if (Init())
            {
                Debug.Log("Item Init");
            }
        }

        private void OnDisable()
        {
            DestroyEffectObject.SpawnObject(GroundPos);
            Destroy(this.gameObject, 5f);
        }

        public bool Init()
        {
            if (Item == null)
                return false;

            SR.sprite = Item.itemData.ItemSprite;
            CC2D = GetComponentInChildren<CircleCollider2D>();
            if (CC2D != null)
            {
                CC2D.isTrigger = true;
                CC2D.radius = DetectedRadius;
            }

            SpawnEffectObject.SpawnObject(GroundPos);

            return true;
        }


        private void InitializeItem(Core core)
        {
            this.ItemCore = core;
        }
        /// <summary>
        /// 감지된 아이템
        /// </summary>
        /// <param name="isright">Detector의 오른쪽인지(Default true)</param>
        public void Detected(bool isright = true)
        {
            if (GameManager.Inst == null)
            {
                Debug.LogWarning("GameMamanger.Inst is Null");
                return;
            }

            if (Item == null)
                return;

            //GameManager.Inst.SubUI.isRight(isright);
            //GameManager.Inst.SubUI.DetailSubUI.SetInit(Item, this.gameObject);

            //if (GameManager.Inst.SubUI.DetailSubUI.Canvas.enabled)
            //{
            //    //대충 SubUI 내용 바꾸는 코드                
            //    Debug.Log($"Change {GameManager.Inst.SubUI.DetailSubUI.gameObject.name} Text");
            //    GameManager.Inst.SubUI.SetSubUI(true);
            //}
            //else
            //{
            //    Debug.Log($"{GameManager.Inst.SubUI.DetailSubUI.gameObject.name} SetActive(true)");
            //    GameManager.Inst.SubUI.SetSubUI(true);
            //}
        }

        public void UnDetected()
        {
            //GameManager.Inst.SubUI.SetSubUI(false);
            //GameManager.Inst.SubUI.DetailSubUI.SetInit(null, null);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            if (DetectedRadius != 0)
            {
                Gizmos.DrawWireSphere(transform.position, DetectedRadius / 2);
            }
        }
    }
}
