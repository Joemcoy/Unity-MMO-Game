using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using TMPro;

using PiMMORPG.Client;
using PiMMORPG.Client.RPG;
using PiMMORPG.Client.BattleRoyale;

namespace Scripts.Local.UI
{
    public class RoomInfoViewer : MonoBehaviour
    {
        public UniStormWeatherSystem_C system;
        public TextMeshProUGUI timeLabel, waterLevelLabel, playerCountLabel, statusLabel;
        
        public int WaterLevel
        {
            get { return int.Parse(waterLevelLabel.text); }
            set { waterLevelLabel.text = value.ToString(); }
        }

        public int PlayerCount
        {
            get { return int.Parse(playerCountLabel.text); }
            set { playerCountLabel.text = value.ToString(); }
        }

        public string Status
        {
            get { return statusLabel.text; }
            set { statusLabel.text = value; }
        }

        private void Update()
        {
            if(system == null || !system.transform.root.gameObject.activeInHierarchy)
                system = FindObjectOfType<UniStormWeatherSystem_C>();
            else
            {
                //timeLabel.transform.parent.parent.gameObject.SetActive(PiBaseClient.Current is PiBRClient);
                waterLevelLabel.transform.parent.gameObject.SetActive(PiBaseClient.Current is PiBRClient);
                //playerCountLabel.transform.parent.gameObject.SetActive(PiBaseClient.Current is PiBRClient);
                statusLabel.transform.parent.gameObject.SetActive(PiBaseClient.Current is PiBRClient);
                timeLabel.text = string.Format("{0}:{1:00}", system.hourCounter, system.minuteCounter);
            }
        }
    }
}