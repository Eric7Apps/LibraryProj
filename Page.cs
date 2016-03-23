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
  private int Index = 0;
  private byte[] UTF8Contents;
  private string RelativeURLBase = "";

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



  internal string GetRelativeURLBase()
    {
    return RelativeURLBase;
    }


  internal string GetTitle()
    {
    return Title;
    }


  internal string GetFileName()
    {
    return FileName;
    }


  internal int GetIndex()
    {
    return Index;
    }


  internal void SetIndex( int ToSet )
    {
    Index = ToSet;
    }



  internal void SetNewTitleAndURL( string UseTitle, string UseURL, string RelativeURL )
    {
    // Make sure this isn't being used from somewhere it
    // shouldn't be used.
    if( Title.Length > 0 )
      {
      MForm.ShowStatus( "This already has a title: " + Title );
      return;
      }

    if( URL.Length > 0 )
      {
      MForm.ShowStatus( "This already has a URL: " + URL );
      return;
      }

    Title = UseTitle;
    URL = UseURL;
    RelativeURLBase = RelativeURL;
    }



  internal bool UpdateFromFile( string UseTitle, string UseURL, string InFileName, bool SetTime, string RelativeURL )
    {
    if( MForm.PageList1 == null )
      return false;


    if( InFileName.Length < 3 )
      {
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "No file name specified for: " + Title );
      return false;
      }

    if( URL.Length > 0 )
      {
      if( URL != UseURL )
        {
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "Title: " + Title );
        MForm.ShowStatus( "UseTitle: " + UseTitle );
        MForm.ShowStatus( "The URL being updated is different from the original one: " + URL );
        return false;
        }
      }

    URL = UseURL;
    RelativeURLBase = RelativeURL;
    if( Title.Length > 0 )
      {
      if( Title != UseTitle )
        {
        // MForm.ShowStatus( "The title is not being changed." );
        }
      }
    else
      {
      Title = UseTitle;
      }

    string FileContents = ReadFromTextFile( InFileName );
    if( FileContents == "" )
      return false;

    if( FileName.Length < 1 )
      FileName = MakeNewFileName();

    // MForm.ShowStatus( " " );
    MForm.ShowStatus( " " );
    // MForm.ShowStatus( "Title: " + Title );
    // MForm.ShowStatus( "Updating: " + URL );
    // MForm.ShowStatus( "File name: " + FileName );

    // This is meant to keep a historical index of articles
    // so the advertising and other things in Javascript
    // from years ago would be sent from a different
    // server and it would be invalid.  A reference to
    // a Javascript file on the server would be to a
    // file that doesn't exist.  There are also 
    // references to Facebook and Twitter that would
    // not be valid.  Most of what's  between the
    // comment tags is about the Javascript code.  It wouldn't
    // make sense to index words for a search when
    // they are in between comment tags.  It also
    // makes it safer because there's no telling what
    // an advertiser will include in their Javascript.
    // It also makes the files smaller because there's
    // a lot of Javascript and comments about the
    // Javascript in every page.

    // GetCleanUnicodeString() was done in
    // ReadFromTextFile() for each line.
    FileContents = Utility.SimplifyAndCleanCharacters( FileContents );

    // RemoveFromStartToEnd doesn't account for nested 
    // tags, but it should work for commented areas.
    // The same comments and script are pretty much
    // used in every file.
    FileContents = Utility.RemoveFromStartToEnd( "<!--", "-->", FileContents );
    FileContents = Utility.RemoveFromStartToEnd( "<script", "/script>", FileContents );

    // FileContents = Utility.RemoveFromStartToEnd( "<link", ">", FileContents );
    // FileContents = Utility.RemoveFromStartToEnd( "<meta", ">", FileContents );
    // FileContents = Utility.RemoveFromStartToEnd( "<style", "/style>", FileContents );
    // FileContents = Utility.RemoveFromStartToEnd( "<form", "/form>", FileContents );

    // <div> tags are nested in all kinds of weird ways
    // and they're not helpful.
    // FileContents = Utility.RemoveFromStartToEnd( "<div", ">", FileContents );
    // FileContents = FileContents.Replace( "</div>", " " );

    if( SetTime )
      ContentsUpdated.SetToNow();

    WriteToTextFile( FileContents );
    MoveContentsToUTF8( FileContents );

    string CleanContents = FileContents;
    FileContents = "";

    // Until I come up with something better:
    CleanContents = CleanContents.Replace( "<h1 style=\"text-align: center;margin: 10px 0 20px 0; font-size: 46px;\" >", "" );

    CleanContents = CleanContents.Replace( "<br/>", " " );
    CleanContents = CleanContents.Replace( "<br>", " " );
    CleanContents = CleanContents.Replace( "<strong>", " " );
    CleanContents = CleanContents.Replace( "</strong>", " " );
    CleanContents = CleanContents.Replace( "<h1>", " " );
    CleanContents = CleanContents.Replace( "<h2>", " " );
    CleanContents = CleanContents.Replace( "<h3>", " " );
    CleanContents = CleanContents.Replace( "<h4>", " " );
    CleanContents = CleanContents.Replace( "<h5>", " " );
    CleanContents = CleanContents.Replace( "<h6>", " " );
    CleanContents = CleanContents.Replace( "</h1>", " " );
    CleanContents = CleanContents.Replace( "</h2>", " " );
    CleanContents = CleanContents.Replace( "</h3>", " " );
    CleanContents = CleanContents.Replace( "</h4>", " " );
    CleanContents = CleanContents.Replace( "</h5>", " " );
    CleanContents = CleanContents.Replace( "</h6>", " " );
    CleanContents = CleanContents.Replace( "<b>", " " );
    CleanContents = CleanContents.Replace( "<i>", " " );
    CleanContents = CleanContents.Replace( "</b>", " " );
    CleanContents = CleanContents.Replace( "</i>", " " );
    CleanContents = CleanContents.Replace( "<em>", " " );
    CleanContents = CleanContents.Replace( "</em>", " " );
    CleanContents = CleanContents.Replace( "&quot;", "\"" );
    CleanContents = CleanContents.Replace( "&apos;", "'" );

    // &eacute;   As in Resum&eacute;

    // <font>
    // <center>

    // Parse what's in the tags recursively.
    Tag OuterTag = new Tag( MForm, this, CleanContents, RelativeURLBase );
    OuterTag.MakeContainedTags();
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



  internal string ReadFromTextFile( string ReadFileName )
    {
    try
    {
    if( !File.Exists( ReadFileName ))
      {
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "The file does not exist for:" );
      MForm.ShowStatus( Title );
      MForm.ShowStatus( ReadFileName );
      MForm.ShowStatus( URL );
      MForm.ShowStatus( " " );
      return "";
      }

    StringBuilder SBuilder = new StringBuilder();
    using( StreamReader SReader = new StreamReader( ReadFileName  ))
      {
      while( SReader.Peek() >= 0 )
        {
        string Line = SReader.ReadLine();
        if( Line == null )
          break;

        Line = Utility.GetCleanUnicodeString( Line, 1000000 );
        if( Line.Length > 0 )
          SBuilder.Append( Line + "\r" );

        }
      }

    return SBuilder.ToString();
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the file: \r\n" + ReadFileName );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  internal bool WriteToTextFile( string FileContents )
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
        if( Line.Trim().Length > 0 )
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
    string Result = Title + "\t" +
           URL + "\t" +
           ContentsUpdated.GetIndex().ToString() + "\t" +
           FileName + "\t" +
           Index.ToString() + "\t" +
           RelativeURLBase;

    return Result;
    }



  internal bool StringToObject( string InString )
    {
    try
    {
    string[] SplitS = InString.Split( new Char[] { '\t' } );

    if( SplitS.Length < 5 )
      return false;

    Title = Utility.GetCleanUnicodeString( SplitS[0], 1000 );
    URL = Utility.GetCleanUnicodeString( SplitS[1], 2000 );

    if( URL.Contains( ".aspx?" ))
      return false;

    ulong TimeIndex = (ulong)Int64.Parse( SplitS[2] );
    ContentsUpdated.SetFromIndex( TimeIndex );
    int Year = ContentsUpdated.GetLocalYear();
    if( Year < 2000 )
      return false;

    if( Year > 2099 )
      return false;

    FileName = Utility.GetCleanUnicodeString( SplitS[3], 1000 );

    Index = Int32.Parse( SplitS[4] );

    if( SplitS.Length >= 6 )
      RelativeURLBase = Utility.GetCleanUnicodeString( SplitS[5], 1000 );
    else
      RelativeURLBase = "http://www.durangoherald.com/";

    if( RelativeURLBase.Length < 10 )
      RelativeURLBase = "http://www.durangoherald.com/";

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Page.StringToObject()." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal void MoveContentsToUTF8( string FileContents )
    {
    if( FileContents == null )
      {
      UTF8Contents = null;
      return;
      }

    if( FileContents == "" )
      {
      UTF8Contents = null;
      return;
      }

    UTF8Contents = UTF8Strings.StringToBytes( FileContents );
    if( UTF8Contents == null )
      {
      MForm.ShowStatus( "FileContents couldn't be converted to UTF8 for:" );
      MForm.ShowStatus( FileName );
      return;
      }

    string TestString = UTF8Strings.BytesToString( UTF8Contents, 2000000000 );
    if( TestString != FileContents )
      throw( new Exception( "This should never happen in MoveContentsToUTF8()." ));

    }



  }
}

