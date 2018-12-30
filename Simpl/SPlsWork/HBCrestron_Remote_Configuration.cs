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

namespace UserModule_HBCRESTRON_REMOTE_CONFIGURATION
{
    public class UserModuleClass_HBCRESTRON_REMOTE_CONFIGURATION : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        StringParameter HOMEBRIDGEIP;
        UShortParameter HOMEBRIDGEPORT;
        StringParameter HOMEBRIDGECODE;
        public override object FunctionMain (  object __obj__ ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusFunctionMainStartCode();
                
                __context__.SourceCodeLine = 230;
                WaitForInitializationComplete ( ) ; 
                __context__.SourceCodeLine = 231;
                 HBCrestron.hbHttpClient_Connect(  HOMEBRIDGEIP  .ToString() , (ushort)( HOMEBRIDGEPORT  .Value ) ,  HOMEBRIDGECODE  .ToString() )  ;  
 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler(); }
            return __obj__;
            }
            
        
        public override void LogosSplusInitialize()
        {
            _SplusNVRAM = new SplusNVRAM( this );
            
            HOMEBRIDGEPORT = new UShortParameter( HOMEBRIDGEPORT__Parameter__, this );
            m_ParameterList.Add( HOMEBRIDGEPORT__Parameter__, HOMEBRIDGEPORT );
            
            HOMEBRIDGEIP = new StringParameter( HOMEBRIDGEIP__Parameter__, this );
            m_ParameterList.Add( HOMEBRIDGEIP__Parameter__, HOMEBRIDGEIP );
            
            HOMEBRIDGECODE = new StringParameter( HOMEBRIDGECODE__Parameter__, this );
            m_ParameterList.Add( HOMEBRIDGECODE__Parameter__, HOMEBRIDGECODE );
            
            
            
            _SplusNVRAM.PopulateCustomAttributeList( true );
            
            NVRAM = _SplusNVRAM;
            
        }
        
        public override void LogosSimplSharpInitialize()
        {
            
            
        }
        
        public UserModuleClass_HBCRESTRON_REMOTE_CONFIGURATION ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}
        
        
        
        
        const uint HOMEBRIDGEIP__Parameter__ = 10;
        const uint HOMEBRIDGEPORT__Parameter__ = 11;
        const uint HOMEBRIDGECODE__Parameter__ = 12;
        
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
