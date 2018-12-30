using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;
using HBCrestronLibrary.Common;
using HBCrestronLibrary.Accessories.Switch;
using HBCrestronLibrary.Accessories.Fan;
using HBCrestronLibrary.Accessories.Outlet;
using HBCrestronLibrary.Accessories.SecuritySystem;
using HBCrestronLibrary.Accessories.Lightbulb;
using HBCrestronLibrary.Accessories.Window;
using HBCrestronLibrary;
using HBCrestronLibrary.Accessories.Thermostat;
using HBCrestronLibrary.Accessories.GarageDoor;
using HBCrestronLibrary.Accessories.Door;
using HBCrestronLibrary.Accessories.OccupancySensor;

namespace UserModule_HBCRESTRON_LOCAL_LIGHTBULB_MODULE
{
    public class UserModuleClass_HBCRESTRON_LOCAL_LIGHTBULB_MODULE : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        Crestron.Logos.SplusObjects.DigitalInput REMOVEDEVICE;
        Crestron.Logos.SplusObjects.DigitalInput RESETDEVICECONFIG;
        Crestron.Logos.SplusObjects.DigitalInput STATE_FB;
        Crestron.Logos.SplusObjects.AnalogInput BRIGHTNESS_FB;
        Crestron.Logos.SplusObjects.AnalogInput HUE_FB;
        Crestron.Logos.SplusObjects.AnalogInput SATURATION_FB;
        Crestron.Logos.SplusObjects.AnalogInput COLORTEMPERATURE_FB;
        Crestron.Logos.SplusObjects.AnalogInput RED_FB;
        Crestron.Logos.SplusObjects.AnalogInput GREEN_FB;
        Crestron.Logos.SplusObjects.AnalogInput BLUE_FB;
        Crestron.Logos.SplusObjects.DigitalOutput STATE_ON;
        Crestron.Logos.SplusObjects.DigitalOutput STATE_OFF;
        Crestron.Logos.SplusObjects.AnalogOutput BRIGHTNESS;
        Crestron.Logos.SplusObjects.AnalogOutput HUE;
        Crestron.Logos.SplusObjects.AnalogOutput SATURATION;
        Crestron.Logos.SplusObjects.AnalogOutput COLORTEMPERATURE;
        Crestron.Logos.SplusObjects.AnalogOutput RED;
        Crestron.Logos.SplusObjects.AnalogOutput GREEN;
        Crestron.Logos.SplusObjects.AnalogOutput BLUE;
        UShortParameter SUPPORTSBRIGHTNESS;
        UShortParameter SUPPORTSHUE;
        UShortParameter SUPPORTSSATURATION;
        UShortParameter SUPPORTSCOLORTEMPERATURE;
        UShortParameter SUPPORTSRGB;
        StringParameter DEVICENAME;
        ushort STARTUPLOCK = 0;
        private void SETBRIGHTNESSFEEDBACK (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 175;
            CreateWait ( "BRIGHTNESSFEEDBACKWAIT" , 50 , BRIGHTNESSFEEDBACKWAIT_Callback ) ;
            
            }
            
        public void BRIGHTNESSFEEDBACKWAIT_CallbackFn( object stateInfo )
        {
        
            try
            {
                Wait __LocalWait__ = (Wait)stateInfo;
                SplusExecutionContext __context__ = SplusThreadStartCode(__LocalWait__);
                __LocalWait__.RemoveFromList();
                
            
             HBLightbulb.SetBrightnessFeedback(  DEVICENAME  .ToString() , (ushort)( BRIGHTNESS_FB  .UshortValue ) )  ;  
 
            
        
        
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler(); }
            
        }
        
    private void SETON (  SplusExecutionContext __context__ ) 
        { 
        
        __context__.SourceCodeLine = 183;
        CreateWait ( "ONWAIT" , 50 , ONWAIT_Callback ) ;
        
        }
        
    public void ONWAIT_CallbackFn( object stateInfo )
    {
    
        try
        {
            Wait __LocalWait__ = (Wait)stateInfo;
            SplusExecutionContext __context__ = SplusThreadStartCode(__LocalWait__);
            __LocalWait__.RemoveFromList();
            
            
            __context__.SourceCodeLine = 185;
            Functions.Pulse ( 10, STATE_ON ) ; 
            
        
        
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler(); }
        
    }
    
object STATE_FB_OnChange_0 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 195;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (STATE_FB  .Value == 1))  ) ) 
            {
            __context__.SourceCodeLine = 196;
             HBLightbulb.SetOnFeedback(  DEVICENAME  .ToString() , (ushort)( 1 ) )  ;  
 
            }
        
        else 
            {
            __context__.SourceCodeLine = 198;
             HBLightbulb.SetOnFeedback(  DEVICENAME  .ToString() , (ushort)( 0 ) )  ;  
 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BRIGHTNESS_FB_OnChange_1 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBLightbulb.SetBrightnessFeedback(  DEVICENAME  .ToString() , (ushort)( BRIGHTNESS_FB  .UshortValue ) )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object HUE_FB_OnChange_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBLightbulb.SetHueFeedback(  DEVICENAME  .ToString() , (ushort)( HUE_FB  .UshortValue ) )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SATURATION_FB_OnChange_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBLightbulb.SetSaturationFeedback(  DEVICENAME  .ToString() , (ushort)( SATURATION_FB  .UshortValue ) )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object COLORTEMPERATURE_FB_OnChange_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBLightbulb.SetColorTemperatureFeedback(  DEVICENAME  .ToString() , (ushort)( COLORTEMPERATURE_FB  .UshortValue ) )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object RED_FB_OnChange_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBLightbulb.SetRGBFeedback(  DEVICENAME  .ToString() , (ushort)( RED_FB  .UshortValue ) , (ushort)( GREEN_FB  .UshortValue ) , (ushort)( BLUE_FB  .UshortValue ) )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object GREEN_FB_OnChange_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBLightbulb.SetRGBFeedback(  DEVICENAME  .ToString() , (ushort)( RED_FB  .UshortValue ) , (ushort)( GREEN_FB  .UshortValue ) , (ushort)( BLUE_FB  .UshortValue ) )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BLUE_FB_OnChange_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBLightbulb.SetRGBFeedback(  DEVICENAME  .ToString() , (ushort)( RED_FB  .UshortValue ) , (ushort)( GREEN_FB  .UshortValue ) , (ushort)( BLUE_FB  .UshortValue ) )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object REMOVEDEVICE_OnPush_8 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBLightbulb.RemoveDevice(  DEVICENAME  .ToString() )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object RESETDEVICECONFIG_OnPush_9 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBLightbulb.RemoveDevice(  DEVICENAME  .ToString() )  ;  
 
         HBLightbulb.AddDevice(  DEVICENAME  .ToString() , (ushort)( SUPPORTSBRIGHTNESS  .Value ) , (ushort)( SUPPORTSHUE  .Value ) , (ushort)( SUPPORTSSATURATION  .Value ) , (ushort)( SUPPORTSCOLORTEMPERATURE  .Value ) , (ushort)( SUPPORTSRGB  .Value ) , "Local" )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public void _LIGHTBULBEVENT ( object __sender__ /*HBCrestronLibrary.Common.HBCrestronEventArgs E */) 
    { 
    HBCrestronEventArgs  E  = (HBCrestronEventArgs )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 248;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Name == DEVICENAME ))  ) ) 
            { 
            __context__.SourceCodeLine = 250;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "On"))  ) ) 
                { 
                __context__.SourceCodeLine = 252;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Value == 1))  ) ) 
                    { 
                    __context__.SourceCodeLine = 254;
                    SETON (  __context__  ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 258;
                    Functions.Pulse ( 10, STATE_OFF ) ; 
                    } 
                
                } 
            
            else 
                {
                __context__.SourceCodeLine = 262;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "Brightness"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 264;
                    CancelWait ( "ONWAIT" ) ; 
                    __context__.SourceCodeLine = 265;
                    BRIGHTNESS  .Value = (ushort) ( E.Value ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 267;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "Hue"))  ) ) 
                        { 
                        __context__.SourceCodeLine = 269;
                        HUE  .Value = (ushort) ( E.Value ) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 271;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "Saturation"))  ) ) 
                            { 
                            __context__.SourceCodeLine = 273;
                            SATURATION  .Value = (ushort) ( E.Value ) ; 
                            } 
                        
                        else 
                            {
                            __context__.SourceCodeLine = 275;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "ColorTemperature"))  ) ) 
                                { 
                                __context__.SourceCodeLine = 277;
                                COLORTEMPERATURE  .Value = (ushort) ( E.Value ) ; 
                                } 
                            
                            else 
                                {
                                __context__.SourceCodeLine = 279;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "Red"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 281;
                                    RED  .Value = (ushort) ( E.Value ) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 283;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "Green"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 285;
                                        GREEN  .Value = (ushort) ( E.Value ) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 287;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "Blue"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 289;
                                            BLUE  .Value = (ushort) ( E.Value ) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                }
                            
                            }
                        
                        }
                    
                    }
                
                }
            
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public void _CONNECTIONEVENT ( object __sender__ /*EventArgs E */) 
    { 
    EventArgs  E  = (EventArgs )__sender__;
     HBLightbulb.AddDevice(  DEVICENAME  .ToString() , (ushort)( SUPPORTSBRIGHTNESS  .Value ) , (ushort)( SUPPORTSHUE  .Value ) , (ushort)( SUPPORTSSATURATION  .Value ) , (ushort)( SUPPORTSCOLORTEMPERATURE  .Value ) , (ushort)( SUPPORTSRGB  .Value ) , "Local" )  ;  
 
     HBLightbulb.SetOnFeedback(  DEVICENAME  .ToString() , (ushort)( STATE_FB  .Value ) )  ;  
 
     HBLightbulb.SetBrightnessFeedback(  DEVICENAME  .ToString() , (ushort)( BRIGHTNESS_FB  .UshortValue ) )  ;  
 
     HBLightbulb.SetRGBFeedback(  DEVICENAME  .ToString() , (ushort)( RED_FB  .UshortValue ) , (ushort)( GREEN_FB  .UshortValue ) , (ushort)( BLUE_FB  .UshortValue ) )  ;  
 
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 309;
        // RegisterEvent( HBCrestron , HBWEBSOCKETCLIENTCONNECTIONEVENT , _CONNECTIONEVENT ) 
        try { g_criticalSection.Enter(); HBCrestron .HBWebSocketClientConnectionEvent  += _CONNECTIONEVENT; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 310;
        // RegisterEvent( HBLightbulb , LIGHTBULBEVENT , _LIGHTBULBEVENT ) 
        try { g_criticalSection.Enter(); HBLightbulb .LightbulbEvent  += _LIGHTBULBEVENT; } finally { g_criticalSection.Leave(); }
        ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    
    REMOVEDEVICE = new Crestron.Logos.SplusObjects.DigitalInput( REMOVEDEVICE__DigitalInput__, this );
    m_DigitalInputList.Add( REMOVEDEVICE__DigitalInput__, REMOVEDEVICE );
    
    RESETDEVICECONFIG = new Crestron.Logos.SplusObjects.DigitalInput( RESETDEVICECONFIG__DigitalInput__, this );
    m_DigitalInputList.Add( RESETDEVICECONFIG__DigitalInput__, RESETDEVICECONFIG );
    
    STATE_FB = new Crestron.Logos.SplusObjects.DigitalInput( STATE_FB__DigitalInput__, this );
    m_DigitalInputList.Add( STATE_FB__DigitalInput__, STATE_FB );
    
    STATE_ON = new Crestron.Logos.SplusObjects.DigitalOutput( STATE_ON__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATE_ON__DigitalOutput__, STATE_ON );
    
    STATE_OFF = new Crestron.Logos.SplusObjects.DigitalOutput( STATE_OFF__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATE_OFF__DigitalOutput__, STATE_OFF );
    
    BRIGHTNESS_FB = new Crestron.Logos.SplusObjects.AnalogInput( BRIGHTNESS_FB__AnalogSerialInput__, this );
    m_AnalogInputList.Add( BRIGHTNESS_FB__AnalogSerialInput__, BRIGHTNESS_FB );
    
    HUE_FB = new Crestron.Logos.SplusObjects.AnalogInput( HUE_FB__AnalogSerialInput__, this );
    m_AnalogInputList.Add( HUE_FB__AnalogSerialInput__, HUE_FB );
    
    SATURATION_FB = new Crestron.Logos.SplusObjects.AnalogInput( SATURATION_FB__AnalogSerialInput__, this );
    m_AnalogInputList.Add( SATURATION_FB__AnalogSerialInput__, SATURATION_FB );
    
    COLORTEMPERATURE_FB = new Crestron.Logos.SplusObjects.AnalogInput( COLORTEMPERATURE_FB__AnalogSerialInput__, this );
    m_AnalogInputList.Add( COLORTEMPERATURE_FB__AnalogSerialInput__, COLORTEMPERATURE_FB );
    
    RED_FB = new Crestron.Logos.SplusObjects.AnalogInput( RED_FB__AnalogSerialInput__, this );
    m_AnalogInputList.Add( RED_FB__AnalogSerialInput__, RED_FB );
    
    GREEN_FB = new Crestron.Logos.SplusObjects.AnalogInput( GREEN_FB__AnalogSerialInput__, this );
    m_AnalogInputList.Add( GREEN_FB__AnalogSerialInput__, GREEN_FB );
    
    BLUE_FB = new Crestron.Logos.SplusObjects.AnalogInput( BLUE_FB__AnalogSerialInput__, this );
    m_AnalogInputList.Add( BLUE_FB__AnalogSerialInput__, BLUE_FB );
    
    BRIGHTNESS = new Crestron.Logos.SplusObjects.AnalogOutput( BRIGHTNESS__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( BRIGHTNESS__AnalogSerialOutput__, BRIGHTNESS );
    
    HUE = new Crestron.Logos.SplusObjects.AnalogOutput( HUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( HUE__AnalogSerialOutput__, HUE );
    
    SATURATION = new Crestron.Logos.SplusObjects.AnalogOutput( SATURATION__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( SATURATION__AnalogSerialOutput__, SATURATION );
    
    COLORTEMPERATURE = new Crestron.Logos.SplusObjects.AnalogOutput( COLORTEMPERATURE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( COLORTEMPERATURE__AnalogSerialOutput__, COLORTEMPERATURE );
    
    RED = new Crestron.Logos.SplusObjects.AnalogOutput( RED__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( RED__AnalogSerialOutput__, RED );
    
    GREEN = new Crestron.Logos.SplusObjects.AnalogOutput( GREEN__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( GREEN__AnalogSerialOutput__, GREEN );
    
    BLUE = new Crestron.Logos.SplusObjects.AnalogOutput( BLUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( BLUE__AnalogSerialOutput__, BLUE );
    
    SUPPORTSBRIGHTNESS = new UShortParameter( SUPPORTSBRIGHTNESS__Parameter__, this );
    m_ParameterList.Add( SUPPORTSBRIGHTNESS__Parameter__, SUPPORTSBRIGHTNESS );
    
    SUPPORTSHUE = new UShortParameter( SUPPORTSHUE__Parameter__, this );
    m_ParameterList.Add( SUPPORTSHUE__Parameter__, SUPPORTSHUE );
    
    SUPPORTSSATURATION = new UShortParameter( SUPPORTSSATURATION__Parameter__, this );
    m_ParameterList.Add( SUPPORTSSATURATION__Parameter__, SUPPORTSSATURATION );
    
    SUPPORTSCOLORTEMPERATURE = new UShortParameter( SUPPORTSCOLORTEMPERATURE__Parameter__, this );
    m_ParameterList.Add( SUPPORTSCOLORTEMPERATURE__Parameter__, SUPPORTSCOLORTEMPERATURE );
    
    SUPPORTSRGB = new UShortParameter( SUPPORTSRGB__Parameter__, this );
    m_ParameterList.Add( SUPPORTSRGB__Parameter__, SUPPORTSRGB );
    
    DEVICENAME = new StringParameter( DEVICENAME__Parameter__, this );
    m_ParameterList.Add( DEVICENAME__Parameter__, DEVICENAME );
    
    BRIGHTNESSFEEDBACKWAIT_Callback = new WaitFunction( BRIGHTNESSFEEDBACKWAIT_CallbackFn );
    ONWAIT_Callback = new WaitFunction( ONWAIT_CallbackFn );
    
    STATE_FB.OnDigitalChange.Add( new InputChangeHandlerWrapper( STATE_FB_OnChange_0, false ) );
    BRIGHTNESS_FB.OnAnalogChange.Add( new InputChangeHandlerWrapper( BRIGHTNESS_FB_OnChange_1, false ) );
    HUE_FB.OnAnalogChange.Add( new InputChangeHandlerWrapper( HUE_FB_OnChange_2, false ) );
    SATURATION_FB.OnAnalogChange.Add( new InputChangeHandlerWrapper( SATURATION_FB_OnChange_3, false ) );
    COLORTEMPERATURE_FB.OnAnalogChange.Add( new InputChangeHandlerWrapper( COLORTEMPERATURE_FB_OnChange_4, false ) );
    RED_FB.OnAnalogChange.Add( new InputChangeHandlerWrapper( RED_FB_OnChange_5, false ) );
    GREEN_FB.OnAnalogChange.Add( new InputChangeHandlerWrapper( GREEN_FB_OnChange_6, false ) );
    BLUE_FB.OnAnalogChange.Add( new InputChangeHandlerWrapper( BLUE_FB_OnChange_7, false ) );
    REMOVEDEVICE.OnDigitalPush.Add( new InputChangeHandlerWrapper( REMOVEDEVICE_OnPush_8, false ) );
    RESETDEVICECONFIG.OnDigitalPush.Add( new InputChangeHandlerWrapper( RESETDEVICECONFIG_OnPush_9, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_HBCRESTRON_LOCAL_LIGHTBULB_MODULE ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}


private WaitFunction BRIGHTNESSFEEDBACKWAIT_Callback;
private WaitFunction ONWAIT_Callback;


const uint REMOVEDEVICE__DigitalInput__ = 0;
const uint RESETDEVICECONFIG__DigitalInput__ = 1;
const uint STATE_FB__DigitalInput__ = 2;
const uint BRIGHTNESS_FB__AnalogSerialInput__ = 0;
const uint HUE_FB__AnalogSerialInput__ = 1;
const uint SATURATION_FB__AnalogSerialInput__ = 2;
const uint COLORTEMPERATURE_FB__AnalogSerialInput__ = 3;
const uint RED_FB__AnalogSerialInput__ = 4;
const uint GREEN_FB__AnalogSerialInput__ = 5;
const uint BLUE_FB__AnalogSerialInput__ = 6;
const uint STATE_ON__DigitalOutput__ = 0;
const uint STATE_OFF__DigitalOutput__ = 1;
const uint BRIGHTNESS__AnalogSerialOutput__ = 0;
const uint HUE__AnalogSerialOutput__ = 1;
const uint SATURATION__AnalogSerialOutput__ = 2;
const uint COLORTEMPERATURE__AnalogSerialOutput__ = 3;
const uint RED__AnalogSerialOutput__ = 4;
const uint GREEN__AnalogSerialOutput__ = 5;
const uint BLUE__AnalogSerialOutput__ = 6;
const uint SUPPORTSBRIGHTNESS__Parameter__ = 10;
const uint SUPPORTSHUE__Parameter__ = 11;
const uint SUPPORTSSATURATION__Parameter__ = 12;
const uint SUPPORTSCOLORTEMPERATURE__Parameter__ = 13;
const uint SUPPORTSRGB__Parameter__ = 14;
const uint DEVICENAME__Parameter__ = 15;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    
}

SplusNVRAM _SplusNVRAM = null;

public class __CEvent__ : CEvent
{
    public __CEvent__() {}
    public void Close() { base.Close(); }
    public int Reset() { return base.Reset() ? 1 : 0; }
    public int Set() { return base.Set() ? 1 : 0; }
    public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
}
public class __CMutex__ : CMutex
{
    public __CMutex__() {}
    public void Close() { base.Close(); }
    public void ReleaseMutex() { base.ReleaseMutex(); }
    public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
}
 public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
