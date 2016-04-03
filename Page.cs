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
  private ECTime ContentsUpdateTime;
  private string FileName = "";
  private int Index = 0;
  private string SearchableContents = "";
  private string FullFileContents = "";
  // private byte[] UTF8Contents;
  private string RelativeURLBase = "";


  private Page()
    {
    }



  internal Page( MainForm UseForm )
    {
    MForm = UseForm;

    ContentsUpdateTime = new ECTime();
    }


  internal void AddStatusString( string InString, int MaxLength )
    {
    try
    {
    if( InString.Length > MaxLength )
      InString = InString.Remove( MaxLength );

    // In the future this will not run in the UI thread
    // so it won't have direct access to MForm.
    MForm.ShowStatus( InString );
    }
    catch( Exception ) // Except )
      {
      // MForm.ShowStatus( "Exception in " );
      // MForm.ShowStatus( Except.Message );
      }
    }


  internal bool GetIsCancelled()
    {
    if( MForm.GetIsClosing())
      return true;

    if( MForm.GetCancelled())
      return true;

    return false;
    }


  internal ulong GetContentsUpdateTimeIndex()
    {
    return ContentsUpdateTime.GetIndex();
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



  internal void UpdateFromFile( string UseTitle, string UseURL, string InFileName, bool SetTime, string RelativeURL, bool ReadFromFile )
    {
    try
    {
    SearchableContents = "";

    if( InFileName.Length < 3 )
      {
      // MForm.ShowStatus( " " );
      // MForm.ShowStatus( "No file name specified for: " + Title );
      return;
      }

    // if( URL.Length > 0 )
      // if( URL != UseURL )

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
      // Truncate the title if it's too long.
      Title = Utility.TruncateString( UseTitle, 500 );
      }

    string FileContents = FullFileContents;
    if( ReadFromFile )
      FileContents = ReadFromTextFile( InFileName );

    if( FileContents == "" )
      return;

    if( FileContents.StartsWith( "%PDF-" ))
      {
      MForm.ShowStatus( "This is a PDF file." );
      MForm.ShowStatus( "Deleting File: " + InFileName );
      File.Delete( InFileName );
      FileContents = "";
      return;
      }

    if( FileContents.Contains( "https://n26.network-auth.com/splash/" ))
      {
      // Obviously this is not running on a real server
      // when this happens.
      MForm.ShowStatus( "Cancelled because WiFi access needs OK." );
      MForm.ShowStatus( "Deleting File: " + InFileName );
      File.Delete( InFileName );
      MForm.SetCancelled();
      return;
      }

    if( !FileContents.Contains( "<html" ))
      {
      if( !FileContents.Contains( "<HTML" ))
        {
        MForm.ShowStatus( "This is not an HTML file." );
        MForm.ShowStatus( "Deleting File: " + InFileName );
        File.Delete( InFileName );
        FileContents = "";
        return;
        }
      }

    // Notice that a main page like the ones added in
    // GetURLManagerForm.cs will have a file name that has
    // the date when it was first created.
    if( FileName.Length < 1 )
      FileName = MakeNewFileName();

    // MForm.ShowStatus( " " );
    // MForm.ShowStatus( " " );
    // MForm.ShowStatus( "Title: " + Title );
    // MForm.ShowStatus( "Updating: " + URL );
    // MForm.ShowStatus( "File name: " + FileName );

    // GetCleanUnicodeString() was done in
    // ReadFromTextFile() for each line.

    if( ReadFromFile )
      WriteToTextFile( FileContents );

    // MoveContentsToUTF8( FileContents );

    string CleanContents = FileContents;
    FileContents = "";

    if( SetTime )
      ContentsUpdateTime.SetToNow();

    if( ReadFromFile )
      {
      // MForm.ShowStatus( "Code Comments:" );
      string CommentLine = CleanContents.ToLower();
      CommentLine = CommentLine.Replace( "\r", " " );
      SortedDictionary<string, int> Lines = Utility.GetPatternsFromStartToEnd( "<!--", "-->", CommentLine );
      if( Lines != null )
        {
        foreach( KeyValuePair<string, int> Kvp in Lines )
          {
          MForm.CodeCommentDictionary1.AddLine( Kvp.Key, RelativeURLBase );
          // MForm.ShowStatus( Kvp.Key );
          }
        }

      // MForm.ShowStatus( "Script:" );
      string ScriptLine = CleanContents.ToLower();
      ScriptLine = ScriptLine.Replace( "\r", " " );
      Lines = Utility.GetPatternsFromStartToEnd( "<script", "/script>", ScriptLine );
      if( Lines != null )
        {
        foreach( KeyValuePair<string, int> Kvp in Lines )
          {
          MForm.ScriptDictionary1.AddLine( Kvp.Key, RelativeURLBase );
          // MForm.   AllWords.UpdateWord( Kvp.Key, URL );
          // MForm.ShowStatus( Kvp.Key );
          }
        }
      }

    CleanContents = Utility.RemovePatternFromStartToEnd( "<!--", "-->", CleanContents );

    // This will match <ScRipT.  It's not case sensitive.
    CleanContents = Utility.RemovePatternFromStartToEnd( "<script", "/script>", CleanContents );

    // Parse what's in the tags recursively.
    BasicTag BTag = new BasicTag( this, CleanContents, RelativeURLBase );
    BTag.MakeContainedTags();
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Page.UpdateFromFile()." );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal void ReadToFullFileContentsString( string InFileName )
    {
    FullFileContents = ReadFromTextFile( InFileName );
    }



  internal void AddLink( string Title, string LinkURL )
    {
    if( "" != MForm.PageList1.GetExistingURL( LinkURL ))
      return;

    // Get this new page:
    if( MForm.GetURLMgrForm != null )
      MForm.GetURLMgrForm.AddURLForm( Title, LinkURL, false, true, RelativeURLBase );

    }


  private string MakeNewFileName()
    {
    if( MForm.GetIsClosing())
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
    // used as text data here.
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
    if( MForm.GetIsClosing())
      return "";

    try
    {
    if( !File.Exists( ReadFileName ))
      {
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "Reading text file. The file does not exist for:" );
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

        Line = Utility.GetCleanUnicodeString( Line, 1000000, true );
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
           ContentsUpdateTime.GetIndex().ToString() + "\t" +
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

    if( SplitS.Length < 6 )
      return false;

    Title = Utility.GetCleanUnicodeString( SplitS[0], 1000, true );
    Title = CleanAndSimplify.SimplifyCharacterCodes( Title );
    URL = Utility.GetCleanUnicodeString( SplitS[1], 2000, true );

    if( URL.Contains( "durangoherald.com/#tab" ))
       return false;

    ulong TimeIndex = (ulong)Int64.Parse( SplitS[2] );
    ContentsUpdateTime.SetFromIndex( TimeIndex );
    int Year = ContentsUpdateTime.GetLocalYear();
    if( Year < 2000 )
      return false;

    if( Year > 2099 )
      return false;

    FileName = Utility.GetCleanUnicodeString( SplitS[3], 2000, true );

    if( FileName.Length > 10 )
      {
      if( !File.Exists( FileName ))
        {
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "The file does not exist for:" );
        MForm.ShowStatus( Title );
        MForm.ShowStatus( FileName );
        MForm.ShowStatus( URL );
        MForm.ShowStatus( " " );
        return false;
        }
      }

    Index = Int32.Parse( SplitS[4] );

    RelativeURLBase = Utility.GetCleanUnicodeString( SplitS[5], 1000, true );

    //   if( RelativeURLBase[RelativeURLBase.Length - 1] == '/' )
    //     RelativeURLBase = Utility.TruncateString( RelativeURLBase, RelativeURLBase.Length - 1 );

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Page.StringToObject()." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }


  /*
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
    */


  internal void AddWords( SortedDictionary<string, int> WordsDictionary )
    {
    if( WordsDictionary == null )
      return;

    string URL = GetURL();
    foreach( KeyValuePair<string, int> Kvp in WordsDictionary )
      {
      MForm.MainWordsData.UpdateWord( Kvp.Key, URL );
      // MForm.AllWords.UpdateWord( Kvp.Key, URL );
      // MForm.ShowStatus( Kvp.Key );
      }
    }



  internal void AddToSearchableContents( string InString )
    {
    SearchableContents = SearchableContents + " " + InString;
    SearchableContents = SearchableContents.Trim();
    }


  internal string GetSearchableContents()
    {
    return SearchableContents;
    }




  }
}

