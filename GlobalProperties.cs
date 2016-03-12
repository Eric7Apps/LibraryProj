// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;



namespace DGOLibrary
{
  class GlobalProperties
  {
  private MainForm MForm;
  private ConfigureFile Config;



  internal GlobalProperties( MainForm UseForm )
    {
    MForm = UseForm;

    Config = new ConfigureFile( MForm, Application.StartupPath + "\\Config.txt" );
    ReadAllPropertiesFromConfig();
    }




  internal void ReadAllPropertiesFromConfig()
    {
    // RSAPrime1 = Config.GetString( "RSAPrime1" );
    }


  /*
  internal string GetRSAPrime1()
    {
    return RSAPrime1;
    }
    */


  /*
  internal void SetRSAPrivKInverseExponent( string SetTo )
    {
    RSAPrivKInverseExponent = SetTo;
    Config.SetString( "RSAPrivKInverseExponent", SetTo );
    Config.WriteToTextFile();
    }
    */


  }
}



