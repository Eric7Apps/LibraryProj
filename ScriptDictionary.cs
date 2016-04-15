// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace DGOLibrary
{
  class ScriptDictionary
  {
  private MainForm MForm;
  private SortedDictionary<string, string> MainDictionary;
  private string FileName = "";



  private ScriptDictionary()
    {
    }



  internal ScriptDictionary( MainForm UseForm )
    {
    MForm = UseForm;

    FileName = MForm.GetDataDirectory() + "JavaScript.txt";

    MainDictionary = new SortedDictionary<string, string>();
    }



  internal bool ReadFromTextFile()
    {
    MainDictionary.Clear();

    if( !File.Exists( FileName ))
      return false;

    try
    {
    using( StreamReader SReader = new StreamReader( FileName, Encoding.UTF8 ))
      {
      while( SReader.Peek() >= 0 ) 
        {
        string Line = SReader.ReadLine();
        if( Line == null )
          continue;

        Line = Line.Trim();
        if( Line == "" )
          continue;

        if( ContainsBadStuff( Line ))
          continue;

        string[] SplitS = Line.Split( new Char[] { '\t' } );
        if( SplitS.Length < 2 )
          continue;

        string KeyWord = SplitS[0].Trim().ToLower();
        string URL = SplitS[1].Trim();
        MainDictionary[KeyWord] = URL;
        }
      }

    MForm.ShowStatus( " " );
    MForm.ShowStatus( "Javascript Count: " + MainDictionary.Count.ToString( "N0" ));
    MForm.ShowStatus( " " );

    return true;

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the file: \r\n" + FileName );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal bool WriteToTextFile()
    {
    try
    {
    using( StreamWriter SWriter = new StreamWriter( FileName, false, Encoding.UTF8 ))
      {
      foreach( KeyValuePair<string, string> Kvp in MainDictionary )
        {
        string Line = Kvp.Key + "\t" + Kvp.Value;
        SWriter.WriteLine( Line );
        }

      SWriter.WriteLine( " " );
      }

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write the Javascript dictionary data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }


  internal bool ContainsBadStuff( string InString )
    {
    InString = InString.ToLower();
    
    if( InString.Contains( "telerik" ))
      return true;

    // It makes a unique ad ID.
    if( InString.Contains( "adzoneid:" ))
      return true;

    // It makes a unique ID.
    if( InString.Contains( "https://secure.ballantinecommunications.net/clickshare/" ))
      return true;

    // It makes a unique ID.
    if( InString.Contains( "current_user_data.id" ))
      return true;



    // It makes a unique tag:
    if( InString.Contains( "googletag.display(" ))
      return true;

    // It's making a separate script for every article.
    if( InString.Contains( "/apps/pbcs.dll/article?" ))
      return true;

    return false;
    }



  internal void AddLine( string Line, string URL )
    {
    Line = Line.Trim().ToLower();

    if( ContainsBadStuff( Line ))
      return;

    if( !MainDictionary.ContainsKey( Line ))
      MForm.ShowStatus( "New JavaScript: " + Line );

    if( Line.Length > 0 )
      MainDictionary[Line] = URL;

    }


  }
}

