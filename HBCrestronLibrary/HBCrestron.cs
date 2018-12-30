using System;
using System.Globalization;
using System.Text;
using Crestron.SimplSharp; // For Basic SIMPL# Classes
using Crestron.SimplSharp.CrestronWebSocketClient;
using Crestron.SimplSharp.Net.Http;
using HBCrestronLibrary.Accessories.Door;
using HBCrestronLibrary.Accessories.GarageDoor;
using HBCrestronLibrary.Accessories.Lightbulb;
using HBCrestronLibrary.Accessories.OccupancySensor;
using HBCrestronLibrary.Accessories.SecuritySystem;
using HBCrestronLibrary.Accessories.Switch;
using HBCrestronLibrary.Accessories.Fan;
using HBCrestronLibrary.Accessories.Outlet;
using HBCrestronLibrary.Accessories.Thermostat;
using HBCrestronLibrary.Accessories.Window;
using Newtonsoft.Json.Linq;
using Kvp = System.Collections.Generic.KeyValuePair<string, object>;

namespace HBCrestronLibrary
{

    public delegate void ConnectionEventHandler(EventArgs e);

    public static class HBCrestron
    {
        private static CTimer _hbWebSocketClientConnectionCheck;
        public static event ConnectionEventHandler HBWebSocketClientConnectionEvent;        
        private static WebSocketClient _hbWebSocketClient;               

        private static CTimer _hbHttpClientPollTimer;
        private static HttpClient _hbHttpClient = new HttpClient();
        private static HttpClientRequest _hbHttpClient_Request = new HttpClientRequest();        
        private static string _hbHttpClientIpAddress = "";
        private static string _hbHttpClientCode = "";
        private static ushort _hbHttpClientPort;

        public static void hbWebSocketClient_Connect(string url, ushort port)
        {
            _hbWebSocketClient = new WebSocketClient();
            _hbWebSocketClient.URL = "ws://" + url + ":" + port;
            _hbWebSocketClient.Host = url;
            _hbWebSocketClient.Port = port;
            _hbWebSocketClient.ConnectionCallBack = hbWebSocketClient_OnOpen;
            
            _hbWebSocketClient.ReceiveCallBack = hbWebSocketClient_OnMessage;
            
            _hbWebSocketClient.DisconnectCallBack = hbWebSocketClient_OnClose;
            _hbWebSocketClient.ConnectAsync();
            

        }
        public static void hbWebSocketClient_CheckConnection(object o)
        {
            if (_hbWebSocketClient.Connected == false)
            {
                _hbWebSocketClient.ConnectAsync();
                _hbWebSocketClientConnectionCheck = new CTimer(hbWebSocketClient_CheckConnection, 3000);
            }
        }
        public static void SendWebSocketData(string jsondata)
        {
            //CrestronConsole.PrintLine("DataSent - {0}", jsondata);
            try
            {
                if(_hbWebSocketClient.Connected == true) //The only edge case issue is adding devices, which shouldn't happen unless a connected event fires. HomeBridge will request the data again on it's own.
                _hbWebSocketClient.Send(Encoding.Default.GetBytes(jsondata), (uint)Encoding.Default.GetBytes(jsondata).Length, WebSocketClient.WEBSOCKET_PACKET_TYPES.LWS_WS_OPCODE_07__TEXT_FRAME);
            }
            catch (Exception)
            {
                CrestronConsole.PrintLine("Error Sending Data out the Websocket");
            }
        }
        static int hbWebSocketClient_OnOpen(WebSocketClient.WEBSOCKET_RESULT_CODES result)
        {
            if (result == WebSocketClient.WEBSOCKET_RESULT_CODES.WEBSOCKET_CLIENT_SUCCESS)
            {
                _hbWebSocketClient.ReceiveAsync();
                HBWebSocketClientConnectionEvent(new EventArgs());
            }
            else
            {
                hbWebSocketClient_OnClose(WebSocketClient.WEBSOCKET_RESULT_CODES.WEBSOCKET_CLIENT_ERROR, null);
            }
            return 0;
        }
        static int hbWebSocketClient_OnClose(WebSocketClient.WEBSOCKET_RESULT_CODES result, object o)
        {
            _hbWebSocketClientConnectionCheck = new CTimer(hbWebSocketClient_CheckConnection, 3000);
            return 0;
        }
        static int hbWebSocketClient_OnMessage(byte[] data,uint length, WebSocketClient.WEBSOCKET_PACKET_TYPES type, WebSocketClient.WEBSOCKET_RESULT_CODES result)
        {
            if (type == WebSocketClient.WEBSOCKET_PACKET_TYPES.LWS_WS_OPCODE_07__CLOSE)
            {
                hbWebSocketClient_OnClose(WebSocketClient.WEBSOCKET_RESULT_CODES.WEBSOCKET_CLIENT_ERROR, null);
            }            
            _hbWebSocketClient.ReceiveAsync();
            var e = Encoding.Default.GetString(data, 0, data.Length);
            var o = JObject.Parse(e);
            string name = (string)o["payload"]["name"];
            //determine who the device belongs to....
            if (HBLightbulb.CheckIfDeviceExists(name) > 0)
            {
                HBLightbulb.ProcessWebsocketResponse(o);
            }
            else if (HBSwitch.CheckIfDeviceExists(name) > 0)
            {
                HBSwitch.ProcessWebsocketResponse(o);
            }
            else if (HBFan.CheckIfDeviceExists(name) > 0)
            {
                HBFan.ProcessWebsocketResponse(o);
            }
            else if (HBOutlet.CheckIfDeviceExists(name) > 0)
            {
                HBOutlet.ProcessWebsocketResponse(o);
            }
            else if (HBDoor.CheckIfDeviceExists(name) > 0)
            {
                HBDoor.ProcessWebsocketResponse(o);
            }
            else if (HBGarageDoor.CheckIfDeviceExists(name) > 0)
            {
                HBGarageDoor.ProcessWebsocketResponse(o);
            }
            else if (HBWindow.CheckIfDeviceExists(name) > 0)
            {
                HBWindow.ProcessWebsocketResponse(o);
            }
            else if (HBOccupancySensor.CheckIfDeviceExists(name) > 0)
            {
                HBOccupancySensor.ProcessWebsocketResponse(o);
            }
            else if (HBThermostat.CheckIfDeviceExists(name) > 0)
            {
                HBThermostat.ProcessWebsocketResponse(o);
            }
            else if (HBSecuritySystem.CheckIfDeviceExists(name) > 0)
            {
                HBSecuritySystem.ProcessWebsocketResponse(o);
            }
            return 0;

        }        
       
        public static void hbHttpClient_Connect(string ip, ushort port, string code)
        {
            _hbHttpClientIpAddress = ip;
            _hbHttpClientPort = port;
            _hbHttpClientCode = code;
            _hbHttpClient.AllowAutoRedirect = true;
            _hbHttpClient.KeepAlive = false;
            //hbHttpClient.Req
            _hbHttpClient_Request.Header.SetHeaderValue("Transfer-Encoding","");
            _hbHttpClient_Request.Header.SetHeaderValue("Content-Type", "Application/json");
            _hbHttpClient_Request.Header.SetHeaderValue("authorization", _hbHttpClientCode);
            _hbHttpClient_Request.RequestType = RequestType.Put;            
            _hbHttpClientPollTimer = new CTimer(hbHttpClient_PollInformation, 7000);
        }        
        public static string hbHttpClient_SendRequest(string page, string parameters, string requestContent)
        {
            if (page == "characteristics")  //If we are going to update a Characteristic, then stop the current poll timer.
            {
                _hbHttpClientPollTimer.Stop();
            }
            //CrestronConsole.PrintLine("Sending Http Data - {0}",requestContent);
            if (parameters != "")
            {
                parameters = "?" + parameters;
            }
            String url = "http://" + _hbHttpClientIpAddress + ":" + _hbHttpClientPort + "/" + page + parameters;
            //CrestronConsole.PrintLine(url);
            try
            {
                _hbHttpClient_Request.Url.Parse(url);
                _hbHttpClient_Request.ContentString = requestContent;                
                try
                {
                    var hbHttpClientResponse = _hbHttpClient.Dispatch(_hbHttpClient_Request);
                    if (hbHttpClientResponse.Code >= 200 && hbHttpClientResponse.Code < 300)
                    {
                        //CrestronConsole.PrintLine(hbHttpClient_Response.ContentString);
                        if (page == "characteristics")
                        {
                            _hbHttpClientPollTimer.Stop();
                            hbHttpClient_PollInformation(null);
                        }
                        return hbHttpClientResponse.ContentString;
                    }
                    ErrorLog.Notice(hbHttpClientResponse.Code.ToString(CultureInfo.InvariantCulture));
                    return null;
                }
                catch (Exception)
                {
                    ErrorLog.Error("Error Connecting To HomeBridge Server, Please Ensure it is Running in Insecure Mode and the IP / Port are Correct");
                    return null;
                }
            }
            catch (Exception)
            {
                ErrorLog.Error("Error Parsing URL for the HomeBridge Server");
                return null;
            }


        }
        static void hbHttpClient_PollInformation(object o)
        {
            var response = hbHttpClient_SendRequest("accessories", "", "");
            if (response != null)
            {
                hbHttpClient_OnMessage(response);
            }
            _hbHttpClientPollTimer = new CTimer(hbHttpClient_PollInformation, 7000);
        }
        static void hbHttpClient_OnMessage(string data)
        {
            var o = JObject.Parse(data);
            //CrestronConsole.PrintLine("DataReceived - {0}", data);
            if (o["accessories"] != null)
            {
                foreach (var accessory in o["accessories"])
                {
                    string name = (string)accessory["services"][0]["characteristics"][3]["value"];
                    //determine who the device belongs to....
                    if (HBLightbulb.CheckIfDeviceExists(name) > 0)
                    {
                        HBLightbulb.ProcessHttpClientResponse(accessory);
                    }
                    else if (HBSwitch.CheckIfDeviceExists(name) > 0)
                    {
                        HBSwitch.ProcessHttpClientResponse(accessory);
                    }
                    else if (HBFan.CheckIfDeviceExists(name) > 0)
                    {
                        HBFan.ProcessHttpClientResponse(accessory);
                    }
                    else if (HBOutlet.CheckIfDeviceExists(name) > 0)
                    {
                        HBOutlet.ProcessHttpClientResponse(accessory);
                    }
                    else if (HBDoor.CheckIfDeviceExists(name) > 0)
                    {
                        HBDoor.ProcessHttpClientResponse(accessory);
                    }
                    else if (HBGarageDoor.CheckIfDeviceExists(name) > 0)
                    {
                        HBGarageDoor.ProcessHttpClientResponse(accessory);
                    }
                    else if (HBWindow.CheckIfDeviceExists(name) > 0)
                    {
                        HBWindow.ProcessHttpClientResponse(accessory);
                    }
                    else if (HBOccupancySensor.CheckIfDeviceExists(name) > 0)
                    {
                        HBOccupancySensor.ProcessHttpClientResponse(accessory);
                    }
                    else if (HBThermostat.CheckIfDeviceExists(name) > 0)
                    {
                        HBThermostat.ProcessHttpClientResponse(accessory);
                    }
                    else if (HBSecuritySystem.CheckIfDeviceExists(name) > 0)
                    {
                        HBSecuritySystem.ProcessHttpClientResponse(accessory);
                    }
                }
            }
        }                       
    }
}