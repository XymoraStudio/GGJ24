using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Microlight.MicroUI {
    // ****************************************************************************************************
    // Window for tooltip system
    // ****************************************************************************************************
    public class MicroTooltipWindow : MonoBehaviour {
        [SerializeField] RectTransform _root;
        public RectTransform Root => _root;
        [SerializeField] GameObject _contentContainer;
        public GameObject ContentContainer => _contentContainer;
        [SerializeField] TMP_Text _textContent;
        public TMP_Text TextContent => _textContent;
        [SerializeField] Image _windowImage;
        public Image WindowImage => _windowImage;
        RectTransform _container;

        [SerializeField] Vector2 _tooltipDownRightOffset;   // When tooltip is down right from mouse, mouse blocks tooltip so it needs to be offset

        // Properties
        string _text;
        public string Text {
            get => _text;
            set {
                _text = value;
                _textContent.text = value;
                _textContent.ForceMeshUpdate();   // This is called so we can update the size of the tooltip
                UpdateWindowSize();
            }
        }
        bool _display;
        public bool Display {
            get => _display;
            set {
                if(value == _display) return;
                _display = value;
                if(value) _container = transform.parent.transform as RectTransform;
                _contentContainer.SetActive(value);
                OnDisplayChange?.Invoke(this);
            }
        }

        // Events
        public event Action<MicroTooltipWindow> OnDisplayChange;

        private void Start() {
            _contentContainer.SetActive(Display);
        }
        // Update enables tooltip to follow mouse
        void Update() {
            if(!_contentContainer.activeInHierarchy) return;

            Root.position = Input.mousePosition + (Vector3)_tooltipDownRightOffset;
            MicroUIUtilities.PositionRectInsideContainer(Root, _container);
        }
        void UpdateWindowSize() {
            if(TextContent == null) return;
            if(Root == null) return;

            Vector2 textSize = TextContent.GetRenderedValues(false);   // Get size of the text content
            Vector2 paddingValue = new Vector2(Mathf.Abs(TextContent.GetComponent<RectTransform>().anchoredPosition.x), Mathf.Abs(TextContent.GetComponent<RectTransform>().anchoredPosition.y));   // Define padding size
            Vector2 paddingSize = new Vector2(paddingValue.x, paddingValue.y) * 2;   // We take the x position which is always the same and appy the same to the y, multiply padding with 2 to pad the other side too
            Vector2 backgroundSize = textSize + paddingSize;

            Root.sizeDelta = backgroundSize;   // Update size of the tooltip
        }
    }
}