using System.Collections.Generic;
using System.Linq;
using HBCrestronLibrary.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HBCrestronLibrary.Accessories.Outlet
{
    public delegate void OutletHandler(HBCrestronEventArgs e);

    class Outlet
    {
        public string Name = string.Empty;
        public string Service = "Outlet";
        public string DeviceLocation = "";
        public ushort RemoteDeviceAID = 0;
        

        public bool On = false;
        public ushort RemoteDeviceOnIID = 0;

        public bool OutletInUse = false;
        public ushort RemoteDeviceOutletInUseIID = 0;
    }

    class AddOutletPayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "service")]
        public string Service = "Outlet";
    }

    public static class HBOutlet
    {
        public static event OutletHandler OutletEvent;
        private static List<Outlet> Outlets = new List<Outlet>();
        public static void AddDevice(string name, string deviceLocation)
        {

            Outlets.Add(new Outlet { Name = name, DeviceLocation = deviceLocation});
            if (deviceLocation == "Local")
            {
                var tempAddOutletPayload = new AddOutletPayload();
                tempAddOutletPayload.Name = name;
                var tempAddObject = new AddObject(tempAddOutletPayload);
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempAddObject)); //Serialize the Data above
            }
        }
        public static void RemoveDevice(string name)
        {
            var tempRemoveObject = new RemoveObject();
            if (Outlets[Outlets.FindIndex(i => i.Name == name)].DeviceLocation == "Local")
            {
                tempRemoveObject.Payload.Name = name;
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempRemoveObject)); //Serialize the Data above
            }
            Outlets.Remove(Outlets[Outlets.FindIndex(i => i.Name == name)]);
        }
        public static ushort CheckIfDeviceExists(string name)
        {
            if (Outlets.Any(item => item.Name == name))
            {
                return 1;
            }
            return 0;
        }
        public static void SetOn(string name, ushort value)
        {
            ControlBoolValueObject tempControlBoolValueObject = new ControlBoolValueObject();
            tempControlBoolValueObject.Characteristics[0] = new ControlBoolValuePayload();
            tempControlBoolValueObject.Characteristics[0].AID = Outlets[Outlets.FindIndex(i => i.Name == name)].RemoteDeviceAID;
            tempControlBoolValueObject.Characteristics[0].IID = Outlets[Outlets.FindIndex(i => i.Name == name)].RemoteDeviceOnIID;
            if (value == 1)
                tempControlBoolValueObject.Characteristics[0].Value = true;
            var stringToSend = JsonConvert.SerializeObject(tempControlBoolValueObject);
            HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
        }
        public static void SetOnFeedback(string name, ushort value)
        {
            if ((Outlets[Outlets.FindIndex(i => i.Name == name)].On && value == 0) || (Outlets[Outlets.FindIndex(i => i.Name == name)].On == false && value == 1))
            {
                if (value == 1)
                {
                    Outlets[Outlets.FindIndex(i => i.Name == name)].On = true;
                }
                else
                {
                    Outlets[Outlets.FindIndex(i => i.Name == name)].On = false;
                }
                UpdateOnFeedback(name);
            }
        }
        public static void SetOutletInUseFeedback(string name, ushort value)
        {
            if ((Outlets[Outlets.FindIndex(i => i.Name == name)].OutletInUse && value == 0) || (Outlets[Outlets.FindIndex(i => i.Name == name)].OutletInUse == false && value == 1))
            {
                if (value == 1)
                {
                    Outlets[Outlets.FindIndex(i => i.Name == name)].OutletInUse = true;
                }
                else
                {
                    Outlets[Outlets.FindIndex(i => i.Name == name)].OutletInUse = false;
                }
                UpdateOutletInUseFeedback(name);
            }
        }
        public static void SetOnFeedback(string name, bool value)
        {
            if (Outlets[Outlets.FindIndex(i => i.Name == name)].On != value)
            {
                Outlets[Outlets.FindIndex(i => i.Name == name)].On = value;
                UpdateOnFeedback(name);
            }
        }        
        public static void UpdateOnFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "On";
            tempUpdateValueObject.Payload.Value = Outlets[Outlets.FindIndex(i => i.Name == name)].On;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateOutletInUseFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "OutletInUse";
            tempUpdateValueObject.Payload.Value = Outlets[Outlets.FindIndex(i => i.Name == name)].OutletInUse;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }   
        public static void ProcessWebsocketResponse(JObject o)
        {
            string topic = (string)o["topic"];
            string name = (string)o["payload"]["name"];
            if (topic == "get")
            {
                string characteristic = (string)o["payload"]["characteristic"];
                switch (characteristic)
                {
                    case "On":
                        UpdateOnFeedback(name);
                        break;
                    case "OutletInUse":
                        UpdateOutletInUseFeedback(name);
                        break;
                }
            }
            else if (topic == "set")
            {
                string characteristic = (string)o["payload"]["characteristic"];
                switch (characteristic)
                {
                    case "On":
                        bool onValue = (bool)o["payload"]["value"];
                        ushort value;
                        if (onValue)
                        {
                            value = 1;
                        }
                        else
                        {
                            value = 0;
                        }
                        OutletEvent(new HBCrestronEventArgs(name, characteristic, value));
                        break;                    
                }
            }
        }
        public static void ProcessHttpClientResponse(JToken accessory)
        {
            var name = (string)accessory["services"][0]["characteristics"][3]["value"];
            if (Outlets[Outlets.FindIndex(i => i.Name == name)].DeviceLocation != "Remote") return;
            Outlets[Outlets.FindIndex(i => i.Name == name)].RemoteDeviceAID = (ushort)accessory["aid"];
            //aid is per device iid is per characteristic
            foreach (var service in accessory["services"])
            {
                foreach (var characteristic in service["characteristics"])
                {
                    switch ((string)characteristic["description"])
                    {
                        case "On":
                            Outlets[Outlets.FindIndex(i => i.Name == name)].RemoteDeviceOnIID =
                                (ushort)characteristic["iid"];

                            if (Outlets[Outlets.FindIndex(i => i.Name == name)].On != (bool)characteristic["value"])
                            {
                                Outlets[Outlets.FindIndex(i => i.Name == name)].On = (bool)characteristic["value"];
                                OutletEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "On", 1)
                                    : new HBCrestronEventArgs(name, "On", 0));
                            }
                            break;
                        case "OutletInUse":
                            Outlets[Outlets.FindIndex(i => i.Name == name)].RemoteDeviceOutletInUseIID =
                                (ushort)characteristic["iid"];

                            if (Outlets[Outlets.FindIndex(i => i.Name == name)].OutletInUse != (bool)characteristic["value"])
                            {
                                Outlets[Outlets.FindIndex(i => i.Name == name)].OutletInUse = (bool)characteristic["value"];
                                OutletEvent((bool)characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "OutletInUse", 1)
                                    : new HBCrestronEventArgs(name, "OutletInUse", 0));
                            }
                            break;
                    }
                }
            }
        }
    }
}