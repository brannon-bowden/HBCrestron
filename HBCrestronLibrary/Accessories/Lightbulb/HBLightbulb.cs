using System;
using System.Collections.Generic;
using System.Linq;
using HBCrestronLibrary.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HBCrestronLibrary.Accessories.Lightbulb
{
    public delegate void LightbulbHandler(HBCrestronEventArgs e);

    class Lightbulb
    {
        public string Name = string.Empty;
        public string Service = "Lightbulb";
        public string DeviceLocation = "";
        public ushort RemoteDeviceAID = 0;

        public bool On = false;
        public ushort RemoteDeviceOnIID = 0;

        public ushort SupportsRGB = 0;

        public ushort SupportsBrightness = 0;
        public ushort Brightness = 0;
        public ushort RemoteDeviceBrightnessIID = 0;

        public ushort SupportsHue = 0;
        public ushort Hue = 0;
        public ushort RemoteDeviceHueIID = 0;

        public ushort SupportsSaturation = 0;
        public ushort Saturation = 0;
        public ushort RemoteDeviceSaturationIID = 0;

        public ushort SupportsColorTemperature = 0;
        public ushort ColorTemperature = 140;
        public ushort RemoteDeviceColorTemperatureIID = 0;
    }

    class AddLightbulbPayload
    {
        [JsonProperty(PropertyName = "name")]
        public string Name = "";
        [JsonProperty(PropertyName = "service")]
        public string service = "Lightbulb";
        public string Brightness = "default";
        public string Hue = "default";
        public string Saturation = "default";
        public string ColorTemperature = "default";

        [JsonIgnore]
        public ushort SupportsBrightness = 0;
        [JsonIgnore]
        public ushort SupportsHue = 0;
        [JsonIgnore]
        public ushort SupportsSaturation = 0;
        [JsonIgnore]
        public ushort SupportsColorTemperature = 0;

        public bool ShouldSerializeBrightness()
        {
            if (SupportsBrightness == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeHue()
        {
            if (SupportsHue == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeSaturation()
        {
            if (SupportsSaturation == 1)
            {
                return true;
            }
            return false;
        }

        public bool ShouldSerializeColorTemperature()
        {
            if (SupportsColorTemperature == 1)
            {
                return true;
            }
            return false;
        }
    }

    public static class HBLightbulb
    {        
        public static event LightbulbHandler LightbulbEvent;
        private static List<Lightbulb> Lightbulbs = new List<Lightbulb>();
        public static void AddDevice(string name, ushort supportsBrightness, ushort supportsHue, ushort supportsSaturation, ushort supportsColorTemperature, ushort supportsRGB, string deviceLocation)
        {
            Lightbulbs.Add(new Lightbulb {Name = name, SupportsBrightness = supportsBrightness, SupportsHue = supportsHue, SupportsSaturation = supportsSaturation, SupportsColorTemperature = supportsColorTemperature, SupportsRGB = supportsRGB, DeviceLocation = deviceLocation});
            if (deviceLocation == "Local")
            {
                var tempAddLightbulbPayload = new AddLightbulbPayload();
                tempAddLightbulbPayload.Name = name;
                tempAddLightbulbPayload.SupportsBrightness = supportsBrightness;
                tempAddLightbulbPayload.SupportsHue = supportsHue;
                tempAddLightbulbPayload.SupportsSaturation = supportsSaturation;
                tempAddLightbulbPayload.SupportsColorTemperature = supportsColorTemperature;
                var tempAddObject = new AddObject(tempAddLightbulbPayload);
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempAddObject));
            }
        }
        public static void RemoveDevice(string name)
        {
            var tempRemoveObject = new RemoveObject();
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].DeviceLocation == "Local")
            {
                tempRemoveObject.Payload.Name = name;
                HBCrestron.SendWebSocketData(JsonConvert.SerializeObject(tempRemoveObject)); //Serialize the Data above
            }
            Lightbulbs.Remove(Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)]);
        }
        public static ushort CheckIfDeviceExists(string name)
        {
            if (Lightbulbs.Any(item => item.Name == name))
            {
                return 1;
            }
            return 0;
        }

        public static void SetOn(string name, ushort value)
        {
            ControlBoolValueObject tempControlBoolValueObject = new ControlBoolValueObject();
            tempControlBoolValueObject.Characteristics[0] = new ControlBoolValuePayload();
            tempControlBoolValueObject.Characteristics[0].AID = Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceAID;
            tempControlBoolValueObject.Characteristics[0].IID = Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceOnIID;
            if(value == 1)
                tempControlBoolValueObject.Characteristics[0].Value = true;
            var stringToSend = JsonConvert.SerializeObject(tempControlBoolValueObject);
            HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
        }
        public static void SetOnFeedback(string name, ushort value)
        {
            if ((Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].On && value == 0) || (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].On == false && value == 1)) 
            {
                if (value == 1)
                {
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].On = true;
                }
                else
                {
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].On = false;
                }
                UpdateOnFeedback(name);                
            }
        }
        public static void SetOnFeedback(string name, bool value)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].On != value)
            {
                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].On = value;
                UpdateOnFeedback(name);                
            }
        }

        public static void SetBrightness(string name, ushort value)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsBrightness == 1)
            {
                var newvalue = Math.Round(value/655.35);
                // ReSharper disable once CompareOfFloatsByEqualityOperator This works, but could probably be written better
                if (newvalue != Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Brightness)
                {
                    var tempControlIntValueObject = new ControlIntValueObject();
                    tempControlIntValueObject.Characteristics[0] = new ControlIntValuePayload();
                    tempControlIntValueObject.Characteristics[0].AID =
                        Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceAID;
                    tempControlIntValueObject.Characteristics[0].IID =
                        Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceBrightnessIID;
                    tempControlIntValueObject.Characteristics[0].Value =(ushort) newvalue;
                    var stringToSend = JsonConvert.SerializeObject(tempControlIntValueObject);
                    HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
                }
            }
        }

        public static void SetBrightnessFeedback(string name, ushort value)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsBrightness == 1)
            {
                var newvalue = Math.Round(value / 655.35);
                // ReSharper disable once CompareOfFloatsByEqualityOperator This works, but could probably be written better
                if (newvalue != Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Brightness)
                {
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Brightness = (ushort)newvalue;
                    UpdateBrightnessFeedback(name);
                }
            }
        }
        public static void SetHue(string name, ushort value)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsHue == 1 &&
                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue != value)
            {
                ControlIntValueObject tempControlIntValueObject = new ControlIntValueObject();
                tempControlIntValueObject.Characteristics[0] = new ControlIntValuePayload();
                tempControlIntValueObject.Characteristics[0].AID =
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceAID;
                tempControlIntValueObject.Characteristics[0].IID =
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceHueIID;
                tempControlIntValueObject.Characteristics[0].Value = value;
                var stringToSend = JsonConvert.SerializeObject(tempControlIntValueObject);
                HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
            }
        }
        public static void SetHueFeedback(string name, ushort value)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsHue == 1 && Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue != value)
            {
                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue = value;
                UpdateHueFeedback(name);
            }
        }
        public static void SetSaturation(string name, ushort value)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsSaturation == 1 &&
                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Saturation != value)
            {
                ControlIntValueObject tempControlIntValueObject = new ControlIntValueObject();
                tempControlIntValueObject.Characteristics[0] = new ControlIntValuePayload();
                tempControlIntValueObject.Characteristics[0].AID =
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceAID;
                tempControlIntValueObject.Characteristics[0].IID =
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceSaturationIID;
                tempControlIntValueObject.Characteristics[0].Value = value;
                var stringToSend = JsonConvert.SerializeObject(tempControlIntValueObject);
                HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
            }
        }
        public static void SetSaturationFeedback(string name, ushort value)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsSaturation == 1 && Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Saturation != value)
            {
                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Saturation = value;
                UpdateSaturationFeedback(name);
            }
        }
        public static void SetColorTemperature(string name, ushort value)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsColorTemperature == 1 &&
                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].ColorTemperature != value)
            {
                ControlIntValueObject tempControlIntValueObject = new ControlIntValueObject();
                tempControlIntValueObject.Characteristics[0] = new ControlIntValuePayload();
                tempControlIntValueObject.Characteristics[0].AID =
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceAID;
                tempControlIntValueObject.Characteristics[0].IID =
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceColorTemperatureIID;
                tempControlIntValueObject.Characteristics[0].Value = value;
                var stringToSend = JsonConvert.SerializeObject(tempControlIntValueObject);
                HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
            }
        }
        public static void SetColorTemperatureFeedback(string name, ushort value)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsColorTemperature == 1 && Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].ColorTemperature != value)
            {
                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].ColorTemperature = value;
                UpdateColorTemperatureFeedback(name);
            }
        }

        public static void SetRGB(string name, ushort red, ushort green, ushort blue)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsRGB == 1)
            {
                RGB tempRgb = new RGB((byte)red, (byte)green, (byte)blue);
                HSV tempHsv = HBLightbulbConversion.RGBtoHSV(tempRgb);
                if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue != (ushort) tempHsv.H)
                {
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue = (ushort) tempHsv.H;
                    ControlIntValueObject tempControlIntValueObject = new ControlIntValueObject();
                    tempControlIntValueObject.Characteristics[0] = new ControlIntValuePayload();
                    tempControlIntValueObject.Characteristics[0].AID =
                        Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceAID;
                    tempControlIntValueObject.Characteristics[0].IID =
                        Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceHueIID;
                    tempControlIntValueObject.Characteristics[0].Value = Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue;
                    var stringToSend = JsonConvert.SerializeObject(tempControlIntValueObject);
                    HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
                }
                if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Saturation != (ushort)tempHsv.S)
                {
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Saturation = (ushort)tempHsv.S;
                    ControlIntValueObject tempControlIntValueObject = new ControlIntValueObject();
                    tempControlIntValueObject.Characteristics[0] = new ControlIntValuePayload();
                    tempControlIntValueObject.Characteristics[0].AID =
                        Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceAID;
                    tempControlIntValueObject.Characteristics[0].IID =
                        Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceSaturationIID;
                    tempControlIntValueObject.Characteristics[0].Value = Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Saturation;
                    var stringToSend = JsonConvert.SerializeObject(tempControlIntValueObject);
                    HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
                }
                if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue != (ushort)tempHsv.V)
                {
                    Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue = (ushort)tempHsv.V;
                    ControlIntValueObject tempControlIntValueObject = new ControlIntValueObject();
                    tempControlIntValueObject.Characteristics[0] = new ControlIntValuePayload();
                    tempControlIntValueObject.Characteristics[0].AID =
                        Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceAID;
                    tempControlIntValueObject.Characteristics[0].IID =
                        Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceBrightnessIID;
                    tempControlIntValueObject.Characteristics[0].Value = Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Brightness;
                    var stringToSend = JsonConvert.SerializeObject(tempControlIntValueObject);
                    HBCrestron.hbHttpClient_SendRequest("characteristics", "", stringToSend);
                }
            }
        }
        public static void SetRGBFeedback(string name, ushort red, ushort green, ushort blue)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsRGB == 1)
            {
                RGB tempRgb = new RGB((byte) red, (byte) green, (byte) blue);
                HSV tempHsv = HBLightbulbConversion.RGBtoHSV(tempRgb);
                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue = (ushort) tempHsv.H;
                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Saturation = (ushort) tempHsv.S;
                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Brightness = (ushort) tempHsv.V;
                UpdateBrightnessFeedback(name);
                UpdateSaturationFeedback(name);
                UpdateHueFeedback(name);
            }
        }  
        public static void UpdateOnFeedback(string name)
        {
            var tempUpdateValueObject = new SetBoolValueObject();
            tempUpdateValueObject.Payload.Name = name;
            tempUpdateValueObject.Payload.Characteristic = "On";
            tempUpdateValueObject.Payload.Value = Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].On;
            var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
            HBCrestron.SendWebSocketData(stringToSend);
        }
        public static void UpdateBrightnessFeedback(string name)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsBrightness == 1)
            {
                var tempUpdateValueObject = new SetIntValueObject();
                tempUpdateValueObject.Payload.Name = name;
                tempUpdateValueObject.Payload.Characteristic = "Brightness";
                tempUpdateValueObject.Payload.Value = Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Brightness;
                var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
                HBCrestron.SendWebSocketData(stringToSend);
            }
        }
        public static void UpdateHueFeedback(string name)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsHue == 1)
            {
                var tempUpdateValueObject = new SetIntValueObject();
                tempUpdateValueObject.Payload.Name = name;
                tempUpdateValueObject.Payload.Characteristic = "Hue";
                tempUpdateValueObject.Payload.Value = Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue;
                var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
                HBCrestron.SendWebSocketData(stringToSend);
            }
        }
        public static void UpdateSaturationFeedback(string name)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsSaturation == 1)
            {
                var tempUpdateValueObject = new SetIntValueObject();
                tempUpdateValueObject.Payload.Name = name;
                tempUpdateValueObject.Payload.Characteristic = "Saturation";
                tempUpdateValueObject.Payload.Value = Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Saturation;
                var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
                HBCrestron.SendWebSocketData(stringToSend);
            }
        }
        public static void UpdateColorTemperatureFeedback(string name)
        {
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsColorTemperature == 1)
            {
                var tempUpdateValueObject = new SetIntValueObject();
                tempUpdateValueObject.Payload.Name = name;
                tempUpdateValueObject.Payload.Characteristic = "ColorTemperature";
                tempUpdateValueObject.Payload.Value = Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].ColorTemperature;
                var stringToSend = JsonConvert.SerializeObject(tempUpdateValueObject);
                HBCrestron.SendWebSocketData(stringToSend);
            }
        }
        public static void ConvertHSVtoRGB(string name)
        {
            HSV tempHSV = new HSV(RGBTempObject.Hue, RGBTempObject.Saturation, RGBTempObject.Brightness);
            RGB tempRGB = HBLightbulbConversion.HSVtoRGB(tempHSV);
            LightbulbEvent(new HBCrestronEventArgs(name, "Red", tempRGB.R));
            LightbulbEvent(new HBCrestronEventArgs(name, "Green", tempRGB.G));
            LightbulbEvent(new HBCrestronEventArgs(name, "Blue", tempRGB.B));
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
                    case "Brightness":
                        UpdateBrightnessFeedback(name);
                        break;
                    case "Hue":
                        UpdateHueFeedback(name);
                        break;
                    case "Saturation":
                        UpdateSaturationFeedback(name);
                        break;
                    case "ColorTemperature":
                        UpdateColorTemperatureFeedback(name);
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
                        LightbulbEvent(new HBCrestronEventArgs(name, characteristic, value));
                        break;
                    case "Brightness":
                        ushort brightnessValue = (ushort)o["payload"]["value"];                        
                        if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsRGB == 1)
                        {
                            RGBTempObject.Brightness = brightnessValue;
                            ConvertHSVtoRGB(name);
                        }
                        else
                        {
                            LightbulbEvent(new HBCrestronEventArgs(name, characteristic, (ushort)Math.Round(brightnessValue * 655.35)));    
                        }
                        break;
                    case "Hue":
                        ushort hueValue = (ushort)o["payload"]["value"];
                        if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsRGB == 1)
                        {
                            RGBTempObject.Hue = hueValue;
                            ConvertHSVtoRGB(name);
                        }
                        else
                        {
                            LightbulbEvent(new HBCrestronEventArgs(name, characteristic, hueValue));
                        }
                        break;
                    case "Saturation":
                        ushort saturationValue = (ushort)o["payload"]["value"];
                        if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].SupportsRGB == 1)
                        {
                            RGBTempObject.Saturation = saturationValue;
                            ConvertHSVtoRGB(name);
                        }
                        else
                        {
                            LightbulbEvent(new HBCrestronEventArgs(name, characteristic, saturationValue));
                        }
                        break;
                    case "ColorTemperature":
                        ushort colorTemperatureValue = (ushort)o["payload"]["value"];
                        LightbulbEvent(new HBCrestronEventArgs(name, characteristic, colorTemperatureValue));
                        break;
                }
            }
        }
        public static void ProcessHttpClientResponse(JToken accessory)
        {
            var name = (string)accessory["services"][0]["characteristics"][3]["value"];
            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].DeviceLocation != "Remote") return;
            Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceAID = (ushort) accessory["aid"];
            //aid is per device iid is per characteristic
            foreach (var service in accessory["services"])
            {
                foreach (var characteristic in service["characteristics"])
                {
                    switch ((string) characteristic["description"])
                    {
                        case "On":
                            Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceOnIID =
                                (ushort) characteristic["iid"];
                            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].On !=
                                (bool) characteristic["value"])
                            {
                                Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].On =
                                    (bool) characteristic["value"];
                                LightbulbEvent((bool) characteristic["value"]
                                    ? new HBCrestronEventArgs(name, "On", 1)
                                    : new HBCrestronEventArgs(name, "On", 0));
                            }
                            break;
                        case "Brightness":
                            Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceBrightnessIID =
                                (ushort) characteristic["iid"];
                            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Brightness ==
                                (ushort) characteristic["value"]) continue;
                            Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Brightness =
                                (ushort) characteristic["value"];
                            LightbulbEvent(new HBCrestronEventArgs(name, "Brightness", (ushort) characteristic["value"]));
                            break;
                        case "Hue":
                            Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceHueIID =
                                (ushort) characteristic["iid"];
                            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue ==
                                (ushort) characteristic["value"]) continue;
                            Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Hue =
                                (ushort) characteristic["value"];
                            LightbulbEvent(new HBCrestronEventArgs(name, "Hue", (ushort) characteristic["value"]));
                            break;
                        case "Saturation":
                            Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceSaturationIID =
                                (ushort) characteristic["iid"];
                            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Saturation ==
                                (ushort) characteristic["value"]) continue;
                            Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].Saturation =
                                (ushort) characteristic["value"];
                            LightbulbEvent(new HBCrestronEventArgs(name, "Saturation", (ushort) characteristic["value"]));
                            break;
                        case "ColorTemperature":
                            Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].RemoteDeviceColorTemperatureIID =
                                (ushort) characteristic["iid"];
                            if (Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].ColorTemperature ==
                                (ushort) characteristic["value"]) continue;
                            Lightbulbs[Lightbulbs.FindIndex(i => i.Name == name)].ColorTemperature =
                                (ushort) characteristic["value"];
                            LightbulbEvent(new HBCrestronEventArgs(name, "ColorTemperature",
                                (ushort) characteristic["value"]));
                            break;
                    }
                }
            }
        }
    }
}