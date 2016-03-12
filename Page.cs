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
  // One page normally corresponds to one HTML file.
  class Page
  {
  private MainForm MForm;
  private string URL = "";
  private ECTime ContentsUpdated;
  private string FileName = "";
  private string FileContents = "";
  private string[] Links;
  internal int IndexNumber = 0;



  private Page()
    {
    }


  internal Page( MainForm UseForm )
    {
    MForm = UseForm;

    ContentsUpdated = new ECTime();
    }


  internal string GetURL()
    {
    return URL;
    }


  internal bool UpdateFromTempFile( string UseURL, string TempFileName )
    {
    if( MForm.PageList1 == null )
      return false;

    URL = UseURL;
    MForm.ShowStatus( "Updating: " + URL );

    if( !ReadFromTextFile( TempFileName ))
      return false;

    ContentsUpdated.SetToNow();
    if( FileName.Length < 1 )
      FileName = MakeNewFileName();

    WriteToTextFile();
    return true;
    }



  private void ParseLinks()
    {
    // Split it at the beginning of each tag.
    string[] Lines = FileContents.Split( new Char[] { '<' } );
    for( int Count = 0; Count < Lines.Length; Count++ )
      {
      MForm.ShowStatus( Lines[Count] );

      }

    // Links = new string[];
    }



  private string MakeNewFileName()
    {
    if( MForm.PageList1 == null )
      return "";

    ECTime RightNow = new ECTime();
    RightNow.SetToNow();

    string Path = MForm.GetPagesDirectory() +
      "Y" + RightNow.GetLocalYear().ToString() + "\\";

    MakeNewDirectory( Path ); // If it needs to create a new one.

    Path += "M" + RightNow.GetLocalMonth().ToString() + "\\";
    MakeNewDirectory( Path );

    Path += "D" + RightNow.GetLocalDay().ToString() + "\\";
    MakeNewDirectory( Path );

    string FileS = "H" + RightNow.GetLocalHour().ToString();
    FileS += "M" + RightNow.GetLocalMinute().ToString();

    FileS += "S" + RightNow.GetLocalSecond().ToString();

    // Make it unique.  (But this would normally happen
    // only once every 3 seconds or so, based on the
    // timer event.)
    ulong Ticks = ECTime.GetRandomishTickCount() & 0xFF;
    FileS += "T" + Ticks.ToString();

    FileS += ".txt";
    return Path + FileS;
    }



  private bool MakeNewDirectory( string NewDir )
    {
    if( Directory.Exists( NewDir ))
      return true;

    try
    {
    Directory.CreateDirectory( NewDir );
    }
    catch( Exception )
      {
      MForm.ShowStatus( "Could not create the directory:" );
      MForm.ShowStatus( NewDir );
      return false;
      }

    return true;
    }



  internal bool ReadFromTextFile( string ReadFileName )
    {
    if( !File.Exists( ReadFileName ))
      return false;

    try
    {
    StringBuilder SBuilder = new StringBuilder();
    string Line;
    
    using( StreamReader SReader = new StreamReader( ReadFileName  ))
      {
      while( SReader.Peek() >= 0 )
        {
        Line = SReader.ReadLine();
        if( Line == null )
          break;

        Line = Utility.GetCleanUnicodeString( Line, 100000 );
        SBuilder.Append( Line + "\r" );
        }
      }

    FileContents = SBuilder.ToString();
    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the file: \r\n" + ReadFileName );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal bool WriteToTextFile()
    {
    try
    {
    if( FileContents.Length < 1 )
      return false;

    MForm.ShowStatus( "Saving to file: \r\n" + FileName );

    string[] Lines = FileContents.Split( new Char[] { '\r' } );

    using( StreamWriter SWriter = new StreamWriter( FileName  )) 
      {
      for( int Count = 0; Count < Lines.Length; Count++ )
        {
        string Line = Lines[Count];
        SWriter.WriteLine( Line );
        }
      }

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write to the file." );
      MForm.ShowStatus( FileName );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal string ObjectToString()
    {
    string Result = URL + "\t" +
           ContentsUpdated.GetIndex().ToString() + "\t" +
           FileName + "\t" +
           IndexNumber.ToString();

    return Result;
    }



  internal bool StringToObject( string InString )
    {
    try
    {
    string[] SplitS = InString.Split( new Char[] { '\t' } );

    if( SplitS.Length < 4 )
      return false;

    URL = Utility.GetCleanUnicodeString( SplitS[0], 2000 );

    ulong TimeIndex = (ulong)Int64.Parse( SplitS[1] );
    ContentsUpdated.SetFromIndex( TimeIndex );
    int Year = ContentsUpdated.GetLocalYear();
    if( Year < 2000 )
      return false;

    if( Year > 2099 )
      return false;

    FileName = Utility.GetCleanUnicodeString( SplitS[2], 1000 );

    IndexNumber = Int32.Parse( SplitS[3] );

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Page.StringToObject()." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  }
}

