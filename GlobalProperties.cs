// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com



using System;
using System.Text;
using System.IO;
using System.Windows.Forms; // Application.



namespace DGOLibrary
{
  class GlobalProperties
  {
  private MainForm MForm;
  private ConfigureFile Config;
  private int LastIndexInCompress1 = 0;
  private int LastIndexInWordCompress = 0;


  internal GlobalProperties( MainForm UseForm )
    {
    MForm = UseForm;

    Config = new ConfigureFile( MForm, Application.StartupPath + "\\Config.txt" );
    ReadAllPropertiesFromConfig();
    }




  internal void ReadAllPropertiesFromConfig()
    {
    try
    {
    LastIndexInCompress1 = GetIntegerValue( "LastIndexInCompress1" );
    LastIndexInWordCompress = GetIntegerValue( "LastIndexInWordCompress" );

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in ReadAllPropertiesFromConfig():" );
      MForm.ShowStatus( Except.Message );
      }
    }



  private int GetIntegerValue( string Key )
    {
    try
    {
    return Int32.Parse( Config.GetString( Key ));
    }
    catch( Exception ) // Except )
      {
      // MForm.ShowStatus( "Exception in GetIntegerValue():" );
      // MForm.ShowStatus( Except.Message );
      return -1;
      }
    }



  private void SetIntegerValue( string Key, int ToSet )
    {
    Config.SetString( Key, ToSet.ToString() );
    Config.WriteToTextFile();
    }



  internal int GetLastIndexInCompress1()
    {
    return LastIndexInCompress1;
    }


  internal void SetLastIndexInCompress1( int SetTo )
    {
    SetIntegerValue( "LastIndexInCompress1", SetTo );
    }



  internal int GetLastIndexInWordCompress()
    {
    return LastIndexInWordCompress;
    }


  internal void SetLastIndexInWordCompress( int SetTo )
    {
    SetIntegerValue( "LastIndexInWordCompress", SetTo );
    }



  }
}



