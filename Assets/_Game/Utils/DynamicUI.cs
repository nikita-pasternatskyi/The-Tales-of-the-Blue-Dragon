using MP.Game.Objects.Tutorial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MP.Game.Utils
{

    public class DynamicUI : MonoBehaviour
    {
        [SerializeField] private Canvas _canvasPrefab;
        protected static Canvas s_CanvasInstance;

        private void Awake()
        {
            CreateCanvas();
        }

        protected void CreateCanvas()
        {
            if (s_CanvasInstance == null)
            {
                s_CanvasInstance = Instantiate(_canvasPrefab);
            }
        }
    }
}
