using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SlideTriggerVisual
{
    public class SliderVisual : MonoBehaviour
    {

        public int kidMin = 10;
        public int kidMax = 25;

        public GUIStyle progress_empty;
        public GUIStyle progress_full;

        //Current Progress
        public float barDisplay;

        Vector2 pos = new Vector2(10, 50);
        Vector2 size = new Vector2(250, 50);

        public Texture2D empty;
        public Texture2D full;
        
        void OnGUI()
        {
            GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y), empty, progress_empty);

            GUI.Box(new Rect(pos.x, pos.y, size.x, size.y), full, progress_full);

            GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
            GUI.Box(new Rect(0, 0, size.x, size.y), full, progress_full);

            GUI.EndGroup();
            GUI.EndGroup();
        }

        void Update()
        {

        }
    }
}