using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityOSC;
using System;


namespace Greg
{
    public class OSCManagerGreg : MonoBehaviour
    {

        public static OSCManagerGreg instance = null;
        public bool SendToMax = true;
        public GameObject player;

        //private Dictionary<string, ServerLog> servers;
        private OSCReciever reciever;

        public int port = 5001;

        //public GameObject player;

        //Initialize our singleton
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            OSCHandler.Instance.Init();
        }

        private void Start()
        {
            reciever = new OSCReciever();
            reciever.Open(port);
        }

        void Update()
        {
            OSCHandler.Instance.UpdateLogs();


            if (reciever.hasWaitingMessages())
            {
                OSCMessage msg = reciever.getNextMessage();
                //Debug.Log(DataToString(msg.Data));
                Debug.Log(msg.Address);


                if (msg.Address == "/D1Radius")
                {
                    ParameterManagerGreg.instance.StopCoroutine("D1RadiusLerp");
                    object[] d1Params = new object[3] { ParameterManagerGreg.instance.getD1Radius(), float.Parse(DataToString(msg.Data)), UnityEngine.Random.Range(.1f, .2f) };
                    ParameterManagerGreg.instance.StartCoroutine("D1RadiusLerp", d1Params);
                    //ParameterManagerGreg.instance.setD1Radius(float.Parse(DataToString(msg.Data)));
                }
                if (msg.Address == "/D2Radius")
                {
                    ParameterManagerGreg.instance.StopCoroutine("D2RadiusLerp");
                    object[] d2Params = new object[3] { ParameterManagerGreg.instance.getD2Radius(), float.Parse(DataToString(msg.Data)), UnityEngine.Random.Range(.1f, .2f) };
                    ParameterManagerGreg.instance.StartCoroutine("D2RadiusLerp", d2Params);
                    //ParameterManagerGreg.instance.setD2Radius(float.Parse(DataToString(msg.Data)));
                }
                if (msg.Address == "/D3Radius")
                {
                    ParameterManagerGreg.instance.StopCoroutine("D3RadiusLerp");
                    object[] d3Params = new object[3] { ParameterManagerGreg.instance.getD3Radius(), float.Parse(DataToString(msg.Data)), UnityEngine.Random.Range(.1f, .2f) };
                    ParameterManagerGreg.instance.StartCoroutine("D3RadiusLerp", d3Params);
                    //ParameterManagerGreg.instance.setD3Radius(float.Parse(DataToString(msg.Data)));
                }

                //msd

                //grab info from max about radius of systems
                //Debug.Log(string.Format("message received: {0} {1}", msg.Address, DataToString(msg.Data)));
            }

            SendMess("/D1Harmonicity", ParameterManagerGreg.instance.D1Harmonicity().ToString());
            SendMess("/D2Harmonicity", ParameterManagerGreg.instance.D2Harmonicity().ToString());
            SendMess("/D2Harmonicity", ParameterManagerGreg.instance.D3Harmonicity().ToString());

            SendMess("/D1Speed", ParameterManagerGreg.instance.D1BaseSpeedMultiplyer().ToString());
            SendMess("/D2Speed", ParameterManagerGreg.instance.D2BaseSpeedMultiplyer().ToString());
            SendMess("/D3Speed", ParameterManagerGreg.instance.D3BaseSpeedMultiplyer().ToString());

            SendMess("/playerPosition", FormatPlayerPosition(player.transform.position));

        }

        public string FormatPlayerPosition(Vector3 pos)
        {
            string xPos = (Mathf.Round(pos.x * 100) / 100).ToString();
            string yPos = (Mathf.Round(pos.y * 100) / 100).ToString();
            string zPos = (Mathf.Round(pos.z * 100) / 100).ToString();
            return xPos + ',' + yPos + ',' + zPos;
        }

        public void SendMess(string route, string mess)
        {
            string maxStuff = route + ' ' + mess;
            OSCHandler.Instance.SendMessageToClient<string>("MaxInfo", "127.0.0.1", maxStuff);
        }

        public void ReceiveMess(string received)
        {

        }

        private string DataToString(List<object> data)
        {
            string buffer = "";

            for (int i = 0; i < data.Count; i++)
            {
                buffer += data[i].ToString() + " ";
            }

            buffer += "\n";

            return buffer;
        }
    }
}

