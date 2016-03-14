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
  private string Title = ""; // The title given in the link tag.
  private ECTime ContentsUpdated;
  private string FileName = "";
  private string FileContents = "";
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


  internal bool UpdateFromTempFile( string UseURL, string TempFileName, string UseTitle )
    {
    if( MForm.PageList1 == null )
      return false;

    Title = UseTitle;
    URL = UseURL;
    MForm.ShowStatus( "Updating: " + URL );
    MForm.ShowStatus( "Title: " + Title );

    if( !ReadFromTextFile( TempFileName ))
      return false;

    ContentsUpdated.SetToNow();
    if( FileName.Length < 1 )
      FileName = MakeNewFileName();

    WriteToTextFile();
    ParseLinks();
    ParseWords();

    return true;
    }



  private void ParseLinks()
    {
    if( FileContents.Length < 1 )
      return;

    if( !FileContents.Contains( "<" ))
      return;


    // Split it at the beginning of each tag.
    string[] Lines = FileContents.Split( new Char[] { '<' } );
    StringBuilder SBuilder = new StringBuilder();
    bool AppendingLink = false;
    for( int Count = 0; Count < Lines.Length; Count++ )
      {
      string Line = Lines[Count];
      if( Line.ToLower().StartsWith( "a href=" ))
        AppendingLink = true;

      // a target="_blank" href="

      // a class="weather-condition" href="

      if( AppendingLink )
        SBuilder.Append( Line );

      if( Line.ToLower().Contains( "/a>" ))
        {
        AppendingLink = false;
        AddOneLink( SBuilder.ToString());
        SBuilder.Clear();
        }
      }
    }



  private void AddOneLink( string InString )
    {
    if( MForm.GetIsClosing())
      return;

    MForm.ShowStatus( " " );
    // MForm.ShowStatus( "AddOneLink: " + InString );
    string[] Parts = InString.Split( new Char[] { '>' } );
    if( Parts.Length < 2 )
      return;

    /*
    if( Parts.Length > 2 )
      {
      // Handle something like this?
      // a href="http://thecloudscout.com/?referrer=durango-herald">
      // img alt="" src="/img/weather_icons/sct.png" height="47" />
      // a class="weather-condition" href="http://thecloudscout.com/?referrer=durango-herald">Partly Cloudy
      // /a>

      return;
      }
      */

    string LinkURL = Parts[0].Trim();
    string LinkTitle = Parts[1].Trim();

    LinkURL = LinkURL.Replace( "a href=", "" );
    LinkURL = LinkURL.Replace( "\"", "" );

    LinkTitle = LinkTitle.Replace( "/a", "" );

    // a href="/section/News01/">Local &amp; Regional
    if( !LinkURL.StartsWith( "http://" ))
      LinkURL = "http://www.durangoherald.com" + LinkURL;

    MForm.ShowStatus( "LinkURL: " + LinkURL );
    MForm.ShowStatus( "LinkTitle: " + LinkTitle );

    if( !MForm.PageList1.ContainsURL( LinkURL ))
      {
      // Get this new page:
      if( MForm.GetURLMgrForm != null )
        MForm.GetURLMgrForm.AddURLForm( LinkTitle, LinkURL );

      }
    }



  private void ParseWords()
    {
    if( FileContents.Length < 1 )
      return;

    if( !FileContents.Contains( "<" ))
      return;

    String AllLines = FileContents;
    AllLines = AllLines.Replace( "<", " " );
    AllLines = AllLines.Replace( ">", " " );
    AllLines = AllLines.Replace( "/", " " );
    AllLines = AllLines.Replace( "=", " " );
    AllLines = AllLines.Replace( ".", " " );
    AllLines = AllLines.Replace( ",", " " );
    AllLines = AllLines.Replace( ";", " " );
    AllLines = AllLines.Replace( ":", " " );
    AllLines = AllLines.Replace( "\r", " " );
    AllLines = AllLines.Replace( "\n", " " );
    AllLines = AllLines.Replace( "\\", " " );
    AllLines = AllLines.Replace( "-", " " );
    AllLines = AllLines.Replace( "_", " " );
    AllLines = AllLines.Replace( "'", " " );
    AllLines = AllLines.Replace( "!", " " );
    AllLines = AllLines.Replace( "(", " " );
    AllLines = AllLines.Replace( ")", " " );
    AllLines = AllLines.Replace( "{", " " );
    AllLines = AllLines.Replace( "}", " " );
    AllLines = AllLines.Replace( "[", " " );
    AllLines = AllLines.Replace( "]", " " );

    // Distinguish a word like 'script' from the
    // script tag.  Somebody could search for a 
    // script for play.
    // Also, type, text, media, window, replace

    // Split on spaces.
    string[] WordsArray = AllLines.Split( new Char[] { ' ' } );
    for( int Count = 0; Count < WordsArray.Length; Count++ )
      {
      string Word = WordsArray[Count].ToLower().Trim();
      if( Word.Length < 2 )
        continue;

      // Word = Word.Replace( "\"", "" );

      if( !WordIsIndexed( Word ))
        continue;

      MForm.ShowStatus( "Word: " + Word );

      }
    }



  private bool WordIsIndexed( string Word )
    {
    // This is hard-coded for now, but it could be from
    // a configuration file and dictionary.
    if( Word == "&amp;" )
      return false;

    if( Word == "the" )
      return false;

    if( Word == "html" )
      return false;

    if( Word == "com" )
      return false;

    if( Word == "org" )
      return false;

    if( Word == "rel" )
      return false;

    if( Word == "href" )
      return false;

    return true;
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

    // Using .txt instead of .htm because it's only
    // used as text data here, and relative links won't
    // work.  And also because of Javascript.
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

    // MForm.ShowStatus( "Saving to file: \r\n" + FileName );

    string[] Lines = FileContents.Split( new Char[] { '\r' } );

    using ( StreamWriter SWriter = new StreamWriter( FileName  )) 
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

