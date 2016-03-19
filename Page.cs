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
  private int Index = 0;



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


  internal int GetIndex()
    {
    return Index;
    }


  internal void SetIndex( int ToSet )
    {
    Index = ToSet;
    }



  internal void SetNewTitleAndURL( string UseTitle, string UseURL )
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
    }



  internal bool UpdateFromTempFile( string UseTitle, string UseURL, string TempFileName )
    {
    if( MForm.PageList1 == null )
      return false;

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

    if( Title.Length > 0 )
      {
      if( Title != UseTitle )
        {
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "The title is not being changed." );
        MForm.ShowStatus( "UseTitle: " + UseTitle );
        MForm.ShowStatus( "Title: " + Title );
        MForm.ShowStatus( "URL: " + URL );
        }
      }
    else
      {
      Title = UseTitle;
      }

    MForm.ShowStatus( " " );
    MForm.ShowStatus( "Title: " + Title );
    MForm.ShowStatus( "Updating: " + URL );

    if( !ReadFromTextFile( TempFileName ))
      return false;

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
    
    // GetCleanUnicodeString() was done in ReadFromTextFile().
    FileContents = Utility.RemoveFromStartToEnd( "<!--", "-->", FileContents );
    FileContents = Utility.RemoveFromStartToEnd( "<script", "/script>", FileContents );
    FileContents = Utility.RemoveFromStartToEnd( "<link", ">", FileContents );
    FileContents = Utility.RemoveFromStartToEnd( "<meta", ">", FileContents );
    // FileContents = Utility.RemoveFromStartToEnd( "<form", "/form>", FileContents );

    // <div> tags are nested in all kinds of weird ways
    // and they're not helpful.
    FileContents = Utility.RemoveFromStartToEnd( "<div", ">", FileContents );
    FileContents = FileContents.Replace( "</div>", " " );

    ContentsUpdated.SetToNow();
    if( FileName.Length < 1 )
      FileName = MakeNewFileName();

    WriteToTextFile();

    Tag OuterTag = new Tag( MForm, this, FileContents );
    OuterTag.MakeContainedTags();

    MForm.ShowStatus( " " );
    MForm.ShowStatus( " " );
    MForm.ShowStatus( " " );
    MForm.ShowStatus( "End of OuterTag parsing." );
    MForm.ShowStatus( " " );
    MForm.ShowStatus( " " );
    MForm.ShowStatus( " " );

    ParseWords();

    return true;
    }



  private void ParseWords()
    {
    if( FileContents.Length < 1 )
      return;

    if( MForm.GetIsClosing())
      return;

    String AllLines = FileContents.ToLower();
    if( !AllLines.Contains( "<html" ))
      return;

    // This will be a much more complex form of parsing.

    //    AllLines = RemoveFromStartToEnd( "<!--", "-->", AllLines );
    //    AllLines = RemoveFromStartToEnd( "<script", "/script>", AllLines );
    AllLines = Utility.RemoveFromStartToEnd( "<!doctype", "\">", AllLines );
    // AllLines = Utility.RemoveFromStartToEnd( "<link rel=\"stylesheet\"", "/>", AllLines );
    // AllLines = Utility.RemoveFromStartToEnd( "<meta", "/>", AllLines );

    AllLines = AllLines.Replace( "\r", " " );
    AllLines = AllLines.Replace( "\n", " " );
    AllLines = AllLines.Replace( "<", " " );
    AllLines = AllLines.Replace( ">", " " );
    AllLines = AllLines.Replace( ".", " " );
    AllLines = AllLines.Replace( ",", " " );
    AllLines = AllLines.Replace( ":", " " );
    AllLines = AllLines.Replace( "\"", " " );
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
    AllLines = AllLines.Replace( "&", " " );
    AllLines = AllLines.Replace( "/", " " );
    AllLines = AllLines.Replace( "=", " " );
    AllLines = AllLines.Replace( "\\", " " );
    AllLines = AllLines.Replace( ";", " " );

    SortedDictionary<string, int> WordsDictionary = new SortedDictionary<string, int>();

    // Split on spaces.
    string[] WordsArray = AllLines.Split( new Char[] { ' ' } );
    for( int Count = 0; Count < WordsArray.Length; Count++ )
      {
      string Word = WordsArray[Count].ToLower().Trim();
      if( Word.Length < 2 )
        continue;

      if( CrudeWayOfDealingWithTags( Word ))
        continue;

      WordsDictionary[Word] = 1;
      }

    foreach( KeyValuePair<string, int> Kvp in WordsDictionary )
      MForm.AllWords.UpdateWord( Kvp.Key, URL );

    // MForm.AllWords.ShowAllWords();
    }


  // This is way too crude.
  private bool CrudeWayOfDealingWithTags( string Word )
    {
    if( Word.StartsWith( "href=" ))
      return true; // Yes it's part of a tag.

    if( Word.StartsWith( "\\com" ))
      return true;

    if( Word.StartsWith( "style=" ))
      return true;

    if( Word.StartsWith( "width=" ))
      return true;

    if( Word.StartsWith( "height=" ))
      return true;

    if( Word.StartsWith( "class=" ))
      return true;

    return false;
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
           Index.ToString();

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

    ulong TimeIndex = (ulong)Int64.Parse( SplitS[2] );
    ContentsUpdated.SetFromIndex( TimeIndex );
    int Year = ContentsUpdated.GetLocalYear();
    if( Year < 2000 )
      return false;

    if( Year > 2099 )
      return false;

    FileName = Utility.GetCleanUnicodeString( SplitS[3], 1000 );

    Index = Int32.Parse( SplitS[4] );

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

