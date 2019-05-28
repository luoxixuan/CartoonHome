using UnityEngine;
using UnityEngine.EventSystems;

namespace GalGameDialog
{
    public class TouchItem : MonoBehaviour, IPointerClickHandler
    {
        public SceneDecorateItem decorateItem;
        public float touchRadiusSquare = 0.01f;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            /*
            #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.touches[0];
                        Vector2 touchPoint = Camera.main.ScreenToWorldPoint(touch.position);
                        float deltaX = gameObject.transform.position.x - touchPoint.x;
                        float deltaY = gameObject.transform.position.y - touchPoint.y;
                        float distance = deltaX*deltaX + deltaY*deltaY;
                        distance = Mathf.Abs(distance);
                        if (touchRadiusSquare > distance) {
                            Debug.Log("TouchItem::Update: touch item. distance: " + distance);
                            onClick();
                        }
                    }
            #endif
            */
        }

        private void onClick()
        {
            if (decorateItem)
            {
                decorateItem.doDecorateItemEvent();
            }
        }

        #region IPointerClickHandler implementation

        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("TouchItem::OnPointerClick: click item");
            onClick();

        }

        #endregion
    }
}