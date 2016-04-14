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
  // private string CompressedFileContents = "";
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



  private void GetScriptAndComments( string FileContents )
    {
    // MForm.ShowStatus( "Code Comments:" );
    FileContents = FileContents.ToLower();
    string CommentLine = FileContents;

    CommentLine = CommentLine.Replace( '\r', ' ' );
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
    string ScriptLine = FileContents;
    ScriptLine = ScriptLine.Replace( '\r', ' ' );
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



  internal void ReindexFromFile( bool ReadFromCompressed )
    {
    try
    {
    SearchableContents = "";

    if( FileName == "" )
      return;

    string FileContents = "";
    if( ReadFromCompressed )
      {
      string CompressedFileName = FileName.Replace( "\\PageFiles\\", "\\PageFilesCompressed\\" );
      FileContents = ReadFromCompressedFile( CompressedFileName );
      }
    else
      {
      FileContents = ReadFromTextFile( FileName );
      }

    if( FileContents == "" )
      return;

    if( HasBadFileContents( FileContents, FileName ))
      {
      File.Delete( FileName );
      return;
      }

    if( !HasGoodBaseURLContents( FileContents ))
      {
      File.Delete( FileName );
      return;
      }

    /*
    if( URL != ExistingURL )
      {
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "Title: " + Title );
      MForm.ShowStatus( "URL is being fixed from: " + URL );
      MForm.ShowStatus( "To: " + URL );
      URL = ExistingURL;
      }
      */

    // MForm.ShowStatus( " " );
    // MForm.ShowStatus( " " );
    // MForm.ShowStatus( "Title: " + Title );
    // MForm.ShowStatus( "Updating: " + URL );
    // MForm.ShowStatus( "File name: " + FileName );

    // GetCleanUnicodeString() was already done
    // in ReadFromTextFile() for each line.

    // Update the file's date/time stamp for testing
    // so I know if the file was used.
    //  WriteToTextFile( FileContents );

    string CleanContents = FileContents;
    FileContents = "";

    if( !ReadFromCompressed )
      {
      CleanContents = Utility.RemovePatternFromStartToEnd( "<!--", "-->", CleanContents );
      // This will match <ScRipT.  It's not case sensitive.
      CleanContents = Utility.RemovePatternFromStartToEnd( "<script", "/script>", CleanContents );
      // SplitForFrequencyData( CleanAndSimplify.SimplifyCharacterCodes( CleanContents ));
      SplitForFrequencyData( CleanContents );
      // ==========
      }

    // ContentsUpdateTime.SetToNow();

    // Parse what's in the tags recursively.
    BasicTag BTag = new BasicTag( this, CleanContents, RelativeURLBase );
    BTag.MakeContainedTags();
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Page.ReindexFromFile()." );
      MForm.ShowStatus( Except.Message );
      }
    }



  private bool HasGoodBaseURLContents( string FileContents )
    {
    // This is an oversimplification for fixing a problem
    // that needs more work.

    FileContents = FileContents.ToLower();

    if( FileContents.Contains( "href=\"http://e-herald.net\"" ))
      {
      if( RelativeURLBase != "http://www.durangoherald.com" )
        return false;

      }


    ////////
    if( RelativeURLBase == "http://www.durangoherald.com" )
      {
      if( FileContents.Contains( "http://www.durangotelegraph.com" ))
        {
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "A page from the Herald contains a full link to the Telegraph." );
        MForm.ShowStatus( "Title: " + Title );
        MForm.ShowStatus( "URL: " + URL );
        return false;
        }

      if( FileContents.Contains( "http://www.durangogov.org" ))
        {
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "A page from the Herald contains a full link to the Durango Gov." );
        MForm.ShowStatus( "Title: " + Title );
        MForm.ShowStatus( "URL: " + URL );
        return false;
        }
      }


    //////////
    if( RelativeURLBase == "http://www.durangotelegraph.com" )
      {
      if( FileContents.Contains( "http://www.durangoherald.com" ))
        {
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "A page from the Telegraph contains a full link to the Herald." );
        MForm.ShowStatus( "Title: " + Title );
        MForm.ShowStatus( "URL: " + URL );
        return false;
        }

      if( FileContents.Contains( "http://www.durangogov.org" ))
        {
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "A page from the Telegraph contains a full link to Durango Gov." );
        MForm.ShowStatus( "Title: " + Title );
        MForm.ShowStatus( "URL: " + URL );
        return false;
        }
      }


    /////////
    if( RelativeURLBase == "http://www.durangogov.org" )
      {
      if( FileContents.Contains( "http://www.durangoherald.com" ))
        {
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "A page from Durango Gov contains a full link to the Herald." );
        MForm.ShowStatus( "Title: " + Title );
        MForm.ShowStatus( "URL: " + URL );
        return false;
        }

      if( FileContents.Contains( "http://www.durangotelegraph.com" ))
        {
        MForm.ShowStatus( " " );
        MForm.ShowStatus( "A page from Durango Gov contains a full link to the Telegraph." );
        MForm.ShowStatus( "Title: " + Title );
        MForm.ShowStatus( "URL: " + URL );
        return false;
        }
      }

    return true;
    }



  internal void MakeNewFromFile( string UseTitle, string UseURL, string InFileName, string RelativeURL )
    {
    try
    {
    SearchableContents = "";

    // MForm.ShowStatus( " " );
    // MForm.ShowStatus( "UseTitle: " + UseTitle );
    // MForm.ShowStatus( "Checking InFileName: " + InFileName );

    if( InFileName.Length < 3 )
      {
      // MForm.ShowStatus( " " );
      // MForm.ShowStatus( "No file name specified for: " + Title );
      return;
      }

    // This might not be a new page.  It might be like
    // one of the main pages that keeps getting
    // downloaded to be updated.  So leave it with
    // the original title and things if that's true.

    if( URL.Length < 3 )
      URL = UseURL;

    if( RelativeURLBase.Length < 3 )
      RelativeURLBase = RelativeURL.ToLower();

    // Truncate the title if it's too long.
    if( Title.Length < 3 )
      Title = Utility.TruncateString( UseTitle, 500 );

    // GetCleanUnicodeString() is done in
    // ReadFromTextFile() for each line.
    string FileContents = ReadFromTextFile( InFileName );
    if( FileContents == "" )
      return;

    if( HasBadFileContents( FileContents, InFileName ))
      return;

    if( !HasGoodBaseURLContents( FileContents ))
      return;  // So it never gets written from the temp file.

    if( FileName.Length < 3 )
      FileName = MakeNewFileName(); // UniqueNumber );

    WriteToTextFile( FileContents );

    // MForm.ShowStatus( " " );
    // MForm.ShowStatus( " " );
    // MForm.ShowStatus( "Title: " + Title );
    // MForm.ShowStatus( "Updating: " + URL );
    // MForm.ShowStatus( "File name: " + FileName );

    string CleanContents = FileContents;
    FileContents = "";

    ContentsUpdateTime.SetToNow();

    GetScriptAndComments( CleanContents );

    CleanContents = Utility.RemovePatternFromStartToEnd( "<!--", "-->", CleanContents );

    // This will match <ScRipT.  It's not case sensitive.
    CleanContents = Utility.RemovePatternFromStartToEnd( "<script", "/script>", CleanContents );

    // Parse what's in the tags recursively.
    BasicTag BTag = new BasicTag( this, CleanContents, RelativeURLBase );
    BTag.MakeContainedTags();
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Page.MakeNewFromFile()." );
      MForm.ShowStatus( Except.Message );
      }
    }



  private bool HasBadFileContents( string FileContents, string InFileName )
    {
    if( FileContents.Contains( "The requested page could not be found." ))
      {
      MForm.ShowStatus( "This link is bad. (404)" );
      MForm.ShowStatus( "URL is: " + URL );
      // MForm.ShowStatus( "Deleting File: " + InFileName );
      File.Delete( InFileName );
      return true;
      }

    if( FileContents.Contains( "sorry, but there is not a web page matching your entry." ))
      {
      MForm.ShowStatus( "This link is bad. (404)" );
      MForm.ShowStatus( "URL is: " + URL );
      // MForm.ShowStatus( "Deleting File: " + InFileName );
      File.Delete( InFileName );
      return true;
      }

    if( FileContents.StartsWith( "%PDF-" ))
      {
      MForm.ShowStatus( "This is a PDF file." );
      MForm.ShowStatus( "URL is: " + URL );
      // MForm.ShowStatus( "Deleting File: " + InFileName );
      File.Delete( InFileName );
      return true;
      }

    if( FileContents.Contains( "https://n26.network-auth.com/splash/" ))
      {
      // Obviously this is not running on a real server
      // when this happens.
      MForm.ShowStatus( "Cancelled because WiFi access needs OK." );
      // MForm.ShowStatus( "Deleting File: " + InFileName );
      File.Delete( InFileName );
      MForm.SetCancelled();
      return true;
      }

    if( !FileContents.Contains( "<html" ))
      {
      if( !FileContents.Contains( "<HTML" ))
        {
        MForm.ShowStatus( "This is not an HTML file." );
        // MForm.ShowStatus( "Deleting File: " + InFileName );
        File.Delete( InFileName );
        return true;
        }
      }

    return false;
    }



  private void SplitForFrequencyData( string InString )
    {
    // It can do without the space or CR after the
    // last </html> tag.
    InString = InString.Trim();

    InString = PageCompress.ReplaceCharacters( InString );

    string[] SplitS = InString.Split( new Char[] { ' ' } );
    for( int Count = 0; Count < SplitS.Length; Count++ )
      {
      string Line = SplitS[Count];

      if( Line.Length >= PageCompress.MinimumStringLength )
        MForm.AddPageFrequencyCount( Line );

      }
    }




  /*
  internal void ReadToFullFileContentsString( string InFileName )
    {
    FullFileContents = ReadFromTextFile( InFileName );
    }
    */


  internal void ReadFullAndWriteToCompressed( string InFileName )
    {
    string FileContents = ReadFromTextFile( InFileName );

    int DiffAt = 0;
    string CleanContents = "";
    byte[] CharBuffer = MForm.CharIndex.StringToBytes( FileContents );
    if( CharBuffer == null )
      {
      MForm.ShowStatus( "CharBuffer == null." );
      }
    else
      {
      string TestChars = MForm.CharIndex.BytesToString( CharBuffer );
      if( TestChars != FileContents )
        {
        DiffAt = Utility.FirstDifferentCharacter( TestChars, FileContents );
        CleanContents = FileContents.Replace( '\r', MarkersDelimiters.CRReplace );
        TestChars = TestChars.Replace( '\r', MarkersDelimiters.CRReplace );
        string ShowS = "TestChars.Length: " + TestChars.Length.ToString() + "\r\n" +
           GetFileName() + "\r\n" +
           "DiffAt: " + DiffAt.ToString() +
           "\r\nCleanContents:\r\n" + CleanContents +
           "\r\n\r\nTestChars:\r\n" + TestChars;

        throw( new Exception( "TestChars != FileContents.\r\n" + ShowS ));
        }
      }

    CleanContents = FileContents;

    // Remove comments.
    CleanContents = Utility.RemovePatternFromStartToEnd( "<!--", "-->", CleanContents );

    // Remove JavaScript.
    CleanContents = Utility.RemovePatternFromStartToEnd( "<script", "/script>", CleanContents );

    // ========= Fix this because of links.
    // CleanContents = CleanAndSimplify.SimplifyCharacterCodes( CleanContents );

    CleanContents = CleanContents.Replace( "\r\r", "\r" );
    CleanContents = CleanContents.Replace( "\r\r", "\r" );
    // It can do without the space or CR after the
    // last </html> tag.
    CleanContents = CleanContents.Trim();
    if( CleanContents.Length < 20 ) // <html>nada</html>
      {
      MForm.ShowStatus( "Nothing to compress in: " + InFileName );
      return;
      }

    string CompressedFileContents = MForm.PageCompress1.GetCompressedPage( CleanContents );
    WriteToCompressedTextFile( CompressedFileContents );
    string Test = MForm.PageCompress1.GetDecompressedPage( CompressedFileContents );

    DiffAt = Utility.FirstDifferentCharacter( Test, CleanContents );

    if( Test != CleanContents )
      {
      CleanContents = CleanContents.Replace( '\r', MarkersDelimiters.CRReplace );
      // CleanContents = CleanContents.Replace( '\n', MarkersDelimiters.NewLineReplace );
      Test = Test.Replace( '\r', MarkersDelimiters.CRReplace );
      // Test = Test.Replace( '\n', MarkersDelimiters.NewLineReplace );

      string ShowS = "DiffAt: " + DiffAt.ToString() +
        "\r\nCleanContents:\r\n" + CleanContents +
        "\r\n\r\nTest:\r\n" + Test +
        "\r\n\r\nCompressedFileContents:\r\n" + CompressedFileContents;

      throw( new Exception( "Test != CleanContents.\r\n" + ShowS ));
      }
    }



  internal void AddLink( string Title, string LinkURL )
    {
    // if( "" != MForm.PageList1.GetExistingURL( LinkURL ))
      // return;

    if( "" != MForm.MainURLIndex.GetExistingURL( LinkURL ))
      return;

    // Get this new page:
    if( MForm.GetURLMgrForm != null )
      MForm.GetURLMgrForm.AddURLForm( Title, LinkURL, false, true, RelativeURLBase );

    }


  private string MakeNewFileName() //  int UniqueNumber )
    {
    if( MForm.GetIsClosing())
      return "";

    ECTime RightNow = new ECTime();
    RightNow.SetToNow();

    string Path = MForm.GetPageFilesDirectory() +
      "Y" + RightNow.GetLocalYear().ToString() + "\\";

    MakeNewDirectory( Path ); // If it needs to create a new one.
    string CompressedPath = Path.Replace( "\\PageFiles\\", "\\PageFilesCompressed\\" );
    MakeNewDirectory( CompressedPath );

    Path += "M" + RightNow.GetLocalMonth().ToString() + "\\";
    MakeNewDirectory( Path );
    CompressedPath = Path.Replace( "\\PageFiles\\", "\\PageFilesCompressed\\" );
    MakeNewDirectory( CompressedPath );

    Path += "D" + RightNow.GetLocalDay().ToString() + "\\";
    MakeNewDirectory( Path );
    CompressedPath = Path.Replace( "\\PageFiles\\", "\\PageFilesCompressed\\" );
    MakeNewDirectory( CompressedPath );

    string FileS = "H" + RightNow.GetLocalHour().ToString();
    FileS += "M" + RightNow.GetLocalMinute().ToString();

    FileS += "S" + RightNow.GetLocalSecond().ToString();

    // Make it unique.  But this would normally happen
    // only once every 2 or 3 seconds or so, based on the
    // timer event.

    // "There are 10,000 ticks in a millisecond."

    // It is improbable, but not guaranteed, that one of
    // these 64K values will be duplicated during a
    // particular second.  Duplicate file names are
    // checked for though.
    // UniqueNumber
    ulong Ticks = ECTime.GetRandomishTickCount() & 0xFFFF;
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

    using( StreamReader SReader = new StreamReader( ReadFileName, Encoding.UTF8 ))
      {
      while( SReader.Peek() >= 0 )
        {
        string Line = SReader.ReadLine();
        if( Line == null )
          break;

        // <meta http-equiv="content-type" 
        // content="text/html; charset=utf-8" />

        Line = Utility.GetCleanUnicodeString( Line, 10000000, false );
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



  internal string ReadFromCompressedFile( string ReadFileName )
    {
    if( MForm.GetIsClosing())
      return "";

    try
    {
    if( !File.Exists( ReadFileName ))
      {
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "Reading compressed text file. The file does not exist for:" );
      MForm.ShowStatus( Title );
      MForm.ShowStatus( ReadFileName );
      MForm.ShowStatus( URL );
      MForm.ShowStatus( " " );
      return "";
      }

    StringBuilder SBuilder = new StringBuilder();

    using( StreamReader SReader = new StreamReader( ReadFileName, Encoding.UTF8 ))
      {
      while( SReader.Peek() >= 0 )
        {
        string Line = SReader.ReadLine();
        if( Line == null )
          break;

        // Line = Utility.GetCleanUnicodeString( Line, 10000000, false );
        if( Line.Length > 0 )
          SBuilder.Append( Line );

        }
      }

    string Result = MForm.PageCompress1.GetDecompressedPage( SBuilder.ToString() );
    return Result;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the file: \r\n" + ReadFileName );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  internal bool WriteToCompressedTextFile( string FileContents )
    {
    try
    {
    if( FileContents.Length < 1 )
      return false;

    string CompressedFileName = FileName.Replace( "\\PageFiles\\", "\\PageFilesCompressed\\" );
    // This would be one big line.
    string[] Lines = FileContents.Split( new Char[] { '\r' } );

    using ( StreamWriter SWriter = new StreamWriter( CompressedFileName, false, Encoding.UTF8 ))
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
      MForm.ShowStatus( "Could not write to the compressed file." );
      // MForm.ShowStatus( CompressedFileName );
      MForm.ShowStatus( Except.Message );
      return false;
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

    using ( StreamWriter SWriter = new StreamWriter( FileName, false, Encoding.UTF8 ))
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
           // Index.ToString() + "\t" +
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

    if( FileName.Length > 0 )
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
    else
      {
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "There is no file for:" );
      MForm.ShowStatus( Title );
      MForm.ShowStatus( URL );
      return false;
      }

    ///////////
    // Index = Int32.Parse( SplitS[4] );
    // RelativeURLBase = Utility.GetCleanUnicodeString( SplitS[5], 1000, true );
    //////////

    RelativeURLBase = Utility.GetCleanUnicodeString( SplitS[4], 1000, true );
    RelativeURLBase = RelativeURLBase.ToLower();

    if( !URL.ToLower().StartsWith( RelativeURLBase ))
      {
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "URL base doesn't match." );
      MForm.ShowStatus( Title );
      MForm.ShowStatus( URL );
      MForm.ShowStatus( RelativeURLBase );
      return false;
      }

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


  internal void AddWords( SortedDictionary<string, int> WordsDictionary, string InFile )
    {
    if( WordsDictionary == null )
      return;

    string URL = GetURL();
    foreach( KeyValuePair<string, int> Kvp in WordsDictionary )
      {
      string FixedWord = WordFix.FixWord( Kvp.Key );

      MForm.MainWordsData.UpdateWord( FixedWord, URL, InFile );
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

