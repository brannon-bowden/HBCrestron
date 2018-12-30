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

namespace UserModule_HBCRESTRON_REMOTE_SWITCH_MODULE
{
    public class UserModuleClass_HBCRESTRON_REMOTE_SWITCH_MODULE : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        Crestron.Logos.SplusObjects.DigitalInput REMOVEDEVICE;
        Crestron.Logos.SplusObjects.DigitalInput RESETDEVICECONFIG;
        Crestron.Logos.SplusObjects.DigitalInput STATE_ON;
        Crestron.Logos.SplusObjects.DigitalInput STATE_OFF;
        Crestron.Logos.SplusObjects.DigitalOutput STATE_ON_FB;
        Crestron.Logos.SplusObjects.DigitalOutput STATE_OFF_FB;
        StringParameter DEVICENAME;
        object STATE_ON_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                 HBSwitch.SetOn(  DEVICENAME  .ToString() , (ushort)( 1 ) )  ;  
 
                
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object STATE_OFF_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
             HBSwitch.SetOn(  DEVICENAME  .ToString() , (ushort)( 0 ) )  ;  
 
            
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object REMOVEDEVICE_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBSwitch.RemoveDevice(  DEVICENAME  .ToString() )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object RESETDEVICECONFIG_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         HBSwitch.RemoveDevice(  DEVICENAME  .ToString() )  ;  
 
         HBSwitch.AddDevice(  DEVICENAME  .ToString() , "Remote" )  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public void _SWITCHEVENT ( object __sender__ /*HBCrestronLibrary.Common.HBCrestronEventArgs E */) 
    { 
    HBCrestronEventArgs  E  = (HBCrestronEventArgs )__sender__;
    try
    {
        SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
        
        __context__.SourceCodeLine = 192;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Name == DEVICENAME ))  ) ) 
            { 
            __context__.SourceCodeLine = 194;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Characteristic == "On"))  ) ) 
                { 
                __context__.SourceCodeLine = 196;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (E.Value == 1))  ) ) 
                    { 
                    __context__.SourceCodeLine = 198;
                    STATE_OFF_FB  .Value = (ushort) ( 0 ) ; 
                    __context__.SourceCodeLine = 199;
                    STATE_ON_FB  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 203;
                    STATE_ON_FB  .Value = (ushort) ( 0 ) ; 
                    __context__.SourceCodeLine = 204;
                    STATE_OFF_FB  .Value = (ushort) ( 1 ) ; 
                    } 
                
                } 
            
            } 
        
        
        
    }
    finally { ObjectFinallyHandler(); }
    }
    
public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 217;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 218;
        // RegisterEvent( HBSwitch , SWITCHEVENT , _SWITCHEVENT ) 
        try { g_criticalSection.Enter(); HBSwitch .SwitchEvent  += _SWITCHEVENT; } finally { g_criticalSection.Leave(); }
        ; 
        __context__.SourceCodeLine = 219;
         HBSwitch.AddDevice(  DEVICENAME  .ToString() , "Remote" )  ;  
 
        __context__.SourceCodeLine = 220;
        STATE_ON_FB  .Value = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 221;
        STATE_OFF_FB  .Value = (ushort) ( 1 ) ; 
        
        
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
    
    STATE_ON = new Crestron.Logos.SplusObjects.DigitalInput( STATE_ON__DigitalInput__, this );
    m_DigitalInputList.Add( STATE_ON__DigitalInput__, STATE_ON );
    
    STATE_OFF = new Crestron.Logos.SplusObjects.DigitalInput( STATE_OFF__DigitalInput__, this );
    m_DigitalInputList.Add( STATE_OFF__DigitalInput__, STATE_OFF );
    
    STATE_ON_FB = new Crestron.Logos.SplusObjects.DigitalOutput( STATE_ON_FB__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATE_ON_FB__DigitalOutput__, STATE_ON_FB );
    
    STATE_OFF_FB = new Crestron.Logos.SplusObjects.DigitalOutput( STATE_OFF_FB__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATE_OFF_FB__DigitalOutput__, STATE_OFF_FB );
    
    DEVICENAME = new StringParameter( DEVICENAME__Parameter__, this );
    m_ParameterList.Add( DEVICENAME__Parameter__, DEVICENAME );
    
    
    STATE_ON.OnDigitalPush.Add( new InputChangeHandlerWrapper( STATE_ON_OnPush_0, false ) );
    STATE_OFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( STATE_OFF_OnPush_1, false ) );
    REMOVEDEVICE.OnDigitalPush.Add( new InputChangeHandlerWrapper( REMOVEDEVICE_OnPush_2, false ) );
    RESETDEVICECONFIG.OnDigitalPush.Add( new InputChangeHandlerWrapper( RESETDEVICECONFIG_OnPush_3, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_HBCRESTRON_REMOTE_SWITCH_MODULE ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint REMOVEDEVICE__DigitalInput__ = 0;
const uint RESETDEVICECONFIG__DigitalInput__ = 1;
const uint STATE_ON__DigitalInput__ = 2;
const uint STATE_OFF__DigitalInput__ = 3;
const uint STATE_ON_FB__DigitalOutput__ = 0;
const uint STATE_OFF_FB__DigitalOutput__ = 1;
const uint DEVICENAME__Parameter__ = 10;

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
