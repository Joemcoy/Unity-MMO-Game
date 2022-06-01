using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Scripts.Local.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        public Image Fill;
        public TextMeshProUGUI ProgressLabel, InfoLabel;
        public float Maximum = 1f;
        private float progress = 0f;
        public static LoadingScreen Instance { get; private set; }

        public float Progress
        {
            get { return Fill == null ? progress : Fill.fillAmount * Maximum; }
            set
            {
                float p = 0;
                if (Fill == null)
                    p = progress = value / Maximum;
                else
                    p = Fill.fillAmount = value / Maximum;
                ProgressText = string.Format("{0}%", Mathf.Round(p * 100f));
            }
        }

        public string ProgressText
        {
            get { return ProgressLabel == null ? string.Empty : ProgressLabel.text; }
            set { if (ProgressLabel != null) ProgressLabel.text = value; }
        }

        public string InfoText
        {
            get { return InfoLabel == null ? string.Empty : InfoLabel.text; }
            set { if (InfoLabel != null) InfoLabel.text = value; }
        }

		void Start()
		{
			//DontDestroyOnLoad(gameObject);
            Instance = this;
		}

		public void Dimiss()
		{
            Debug.Log("Dimiss loading screen!");
            Instance = null;
		}
    }
}