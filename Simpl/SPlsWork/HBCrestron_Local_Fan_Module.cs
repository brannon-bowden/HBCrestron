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

namespace UserModule_HBCRESTRON_LOCAL_FAN_MODULE
{
    public class UserModuleClass_HBCRESTRON_LOCAL_FAN_MODULE : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        Crestron.Logos.SplusObjects.DigitalInput REMOVEDEVICE;
        Crestron.Logos.SplusObjects.DigitalInput RESETDEVICECONFIG;
        Crestron.Logos.SplusObjects.DigitalInput STATE_FB;
        Crestron.Logos.SplusObjects.DigitalInput ROTATION_CLOCKWISE_FB;
        Crestron.Logos.SplusObjects.DigitalInput ROTATION_COUNTERCLOCKWISE_FB;
        Crestron.Logos.SplusObjects.AnalogInput ROTATION_SPEED_FB;
        Crestron.Logos.SplusObjects.DigitalOutput STATE_ON;
        Crestron.Logos.SplusObjects.DigitalOutput STATE_OFF;
        Crestron.Logos.SplusObjects.DigitalOutput ROTATION_CLOCKWISE;
        Crestron.Logos.SplusObjects.DigitalOutput ROTATION_COUNTERCLOCKWISE;
        Crestron.Logos.SplusObjects.AnalogOutput ROTATION_SPEED;
        UShortParameter SUPPORTSROTATIONDIRECTION;
        UShortParameter SUPPORTSROTATIONSPEED;
        StringParameter DEVICENAME;
        object STATE_FB_OnChange_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 178;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (STATE_FB  .Value == 1))  ) ) 
                    {
                    __context__.SourceCodeLine = 179;
                     HBFan.SetOnFeedback(  DEVICENAME  .ToString() , (ushort)( 1 ) )  ;  
 
                    }
                
                else 
                    {
                    __context__.SourceCodeLine = 181;
                     HBFan.SetOnFeedback(  DEVICENAME  .ToString() , (ushort)( 0 ) )  ;  
 
                    }
                
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object ROTATION_CLOCKWISE_FB_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
             HBFan.SetRotationDirectionFeedback(  DEVICENAME  .ToString() , (ushort)( 0 ) )  ;  
 
            
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object ROTATION_COUNTERCLOCKWISE_FB_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBFan.SetRotationDirectionFeedback(  DEVICENAME  .ToString() , (ushort)( 1 ) )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ROTATION_SPEED_FB_OnChange_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBFan.SetRotationSpeedFeedback(  DEVICENAME  .ToString() , (ushort)( ROTATION_SPEED_FB  .UshortValue ) )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object REMOVEDEVICE_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBFan.RemoveDevice(  DEVICENAME  .ToString() )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object RESETDEVICECONFIG_OnPush_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBFan.RemoveDevice(  DEVICENAME  .ToString() )  ;  
 
         HBFan.AddDevice(  DEVICENAME  .ToString() , (ushort)( SUPPORTSROTATIONSPEED  .Value ) , (ushort)( SUPPORTSROTATIONDIRECTION  .Value ) , "Local" )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public void _FANEVENT ( object __sender__ /*HBCrestronLibrary.Common.HBCrestronEventArgs E */) 
    { 
    HBCrestronEventArgs  E  = (HBCrestronEventArgs )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 215;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Name == DEVICENAME ))  ) ) 
            { 
            __context__.SourceCodeLine = 217;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "On"))  ) ) 
                { 
                __context__.SourceCodeLine = 219;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Value == 1))  ) ) 
                    { 
                    __context__.SourceCodeLine = 221;
                    Functions.Pulse ( 10, STATE_ON ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 225;
                    Functions.Pulse ( 10, STATE_OFF ) ; 
                    } 
                
                } 
            
            else 
                {
                __context__.SourceCodeLine = 228;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "RotationDirection"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 230;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Value == 1))  ) ) 
                        { 
                        __context__.SourceCodeLine = 232;
                        Functions.Pulse ( 10, ROTATION_COUNTERCLOCKWISE ) ; 
                        } 
                    
                    else 
                        { 
                        __context__.SourceCodeLine = 236;
                        Functions.Pulse ( 10, ROTATION_CLOCKWISE ) ; 
                        } 
                    
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 239;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "RotationSpeed"))  ) ) 
                        { 
                        __context__.SourceCodeLine = 241;
                        ROTATION_SPEED  .Value = (ushort) ( E.Value ) ; 
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
     HBFan.AddDevice(  DEVICENAME  .ToString() , (ushort)( SUPPORTSROTATIONSPEED  .Value ) , (ushort)( SUPPORTSROTATIONDIRECTION  .Value ) , "Local" )  ;  
 
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
        
        __context__.SourceCodeLine = 258;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 259;
        // RegisterEvent( HBCrestron , HBWEBSOCKETCLIENTCONNECTIONEVENT , _CONNECTIONEVENT ) 
        try { g_criticalSection.Enter(); HBCrestron .HBWebSocketClientConnectionEvent  += _CONNECTIONEVENT; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 260;
        // RegisterEvent( HBFan , FANEVENT , _FANEVENT ) 
        try { g_criticalSection.Enter(); HBFan .FanEvent  += _FANEVENT; } finally { g_criticalSection.Leave(); }
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
    
    ROTATION_CLOCKWISE_FB = new Crestron.Logos.SplusObjects.DigitalInput( ROTATION_CLOCKWISE_FB__DigitalInput__, this );
    m_DigitalInputList.Add( ROTATION_CLOCKWISE_FB__DigitalInput__, ROTATION_CLOCKWISE_FB );
    
    ROTATION_COUNTERCLOCKWISE_FB = new Crestron.Logos.SplusObjects.DigitalInput( ROTATION_COUNTERCLOCKWISE_FB__DigitalInput__, this );
    m_DigitalInputList.Add( ROTATION_COUNTERCLOCKWISE_FB__DigitalInput__, ROTATION_COUNTERCLOCKWISE_FB );
    
    STATE_ON = new Crestron.Logos.SplusObjects.DigitalOutput( STATE_ON__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATE_ON__DigitalOutput__, STATE_ON );
    
    STATE_OFF = new Crestron.Logos.SplusObjects.DigitalOutput( STATE_OFF__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATE_OFF__DigitalOutput__, STATE_OFF );
    
    ROTATION_CLOCKWISE = new Crestron.Logos.SplusObjects.DigitalOutput( ROTATION_CLOCKWISE__DigitalOutput__, this );
    m_DigitalOutputList.Add( ROTATION_CLOCKWISE__DigitalOutput__, ROTATION_CLOCKWISE );
    
    ROTATION_COUNTERCLOCKWISE = new Crestron.Logos.SplusObjects.DigitalOutput( ROTATION_COUNTERCLOCKWISE__DigitalOutput__, this );
    m_DigitalOutputList.Add( ROTATION_COUNTERCLOCKWISE__DigitalOutput__, ROTATION_COUNTERCLOCKWISE );
    
    ROTATION_SPEED_FB = new Crestron.Logos.SplusObjects.AnalogInput( ROTATION_SPEED_FB__AnalogSerialInput__, this );
    m_AnalogInputList.Add( ROTATION_SPEED_FB__AnalogSerialInput__, ROTATION_SPEED_FB );
    
    ROTATION_SPEED = new Crestron.Logos.SplusObjects.AnalogOutput( ROTATION_SPEED__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( ROTATION_SPEED__AnalogSerialOutput__, ROTATION_SPEED );
    
    SUPPORTSROTATIONDIRECTION = new UShortParameter( SUPPORTSROTATIONDIRECTION__Parameter__, this );
    m_ParameterList.Add( SUPPORTSROTATIONDIRECTION__Parameter__, SUPPORTSROTATIONDIRECTION );
    
    SUPPORTSROTATIONSPEED = new UShortParameter( SUPPORTSROTATIONSPEED__Parameter__, this );
    m_ParameterList.Add( SUPPORTSROTATIONSPEED__Parameter__, SUPPORTSROTATIONSPEED );
    
    DEVICENAME = new StringParameter( DEVICENAME__Parameter__, this );
    m_ParameterList.Add( DEVICENAME__Parameter__, DEVICENAME );
    
    
    STATE_FB.OnDigitalChange.Add( new InputChangeHandlerWrapper( STATE_FB_OnChange_0, false ) );
    ROTATION_CLOCKWISE_FB.OnDigitalPush.Add( new InputChangeHandlerWrapper( ROTATION_CLOCKWISE_FB_OnPush_1, false ) );
    ROTATION_COUNTERCLOCKWISE_FB.OnDigitalPush.Add( new InputChangeHandlerWrapper( ROTATION_COUNTERCLOCKWISE_FB_OnPush_2, false ) );
    ROTATION_SPEED_FB.OnAnalogChange.Add( new InputChangeHandlerWrapper( ROTATION_SPEED_FB_OnChange_3, false ) );
    REMOVEDEVICE.OnDigitalPush.Add( new InputChangeHandlerWrapper( REMOVEDEVICE_OnPush_4, false ) );
    RESETDEVICECONFIG.OnDigitalPush.Add( new InputChangeHandlerWrapper( RESETDEVICECONFIG_OnPush_5, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_HBCRESTRON_LOCAL_FAN_MODULE ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint REMOVEDEVICE__DigitalInput__ = 0;
const uint RESETDEVICECONFIG__DigitalInput__ = 1;
const uint STATE_FB__DigitalInput__ = 2;
const uint ROTATION_CLOCKWISE_FB__DigitalInput__ = 3;
const uint ROTATION_COUNTERCLOCKWISE_FB__DigitalInput__ = 4;
const uint ROTATION_SPEED_FB__AnalogSerialInput__ = 0;
const uint STATE_ON__DigitalOutput__ = 0;
const uint STATE_OFF__DigitalOutput__ = 1;
const uint ROTATION_CLOCKWISE__DigitalOutput__ = 2;
const uint ROTATION_COUNTERCLOCKWISE__DigitalOutput__ = 3;
const uint ROTATION_SPEED__AnalogSerialOutput__ = 0;
const uint SUPPORTSROTATIONDIRECTION__Parameter__ = 10;
const uint SUPPORTSROTATIONSPEED__Parameter__ = 11;
const uint DEVICENAME__Parameter__ = 12;

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
