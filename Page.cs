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
    FileContents = RemoveFromStartToEnd( "<!--", "-->", FileContents );
    FileContents = RemoveFromStartToEnd( "<script", "/script>", FileContents );

    // <div> tags are nested in all kinds of weird ways
    // and they're not helpful.
    FileContents = RemoveFromStartToEnd( "<div", ">", FileContents );
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

    if( !FileContents.Contains( ">" ))
      return;

    string AllLines = FileContents;

    // a class="weather-condition" href="

    // a target="_blank" href="
    // AllLines = AllLines.Replace( " target=\"_blank\"", "" );

    // Split it at the beginning of each tag.
    string[] Lines = AllLines.Split( new Char[] { '<' } );

    StringBuilder SBuilder = new StringBuilder();
    bool AppendingLink = false;
    for( int Count = 0; Count < Lines.Length; Count++ )
      {
      string Line = Lines[Count];
      if( Line.ToLower().StartsWith( "a href=" ))
        AppendingLink = true;

      if( AppendingLink )
        SBuilder.Append( Line );

      if( Line.ToLower().Contains( "/a>" ))
        {
        AppendingLink = false;
        AddOneLink( SBuilder.ToString());
        SBuilder.Clear();
        }
      }

    MForm.ShowStatus( " " );
    MForm.ShowStatus( "Finished parsing links." );
    MForm.ShowStatus( " " );
    }



  private void AddOneLink( string InString )
    {
    if( MForm.GetIsClosing())
      return;

    string[] Parts = InString.Split( new Char[] { '>' } );
    if( Parts.Length < 2 )
      return;

    string LinkURL = Utility.GetCleanUnicodeString( Parts[0], 2000 );
    string LinkTitle = Utility.GetCleanUnicodeString( Parts[1], 1000 );

    LinkURL = LinkURL.Replace( "a href=", "" );
    LinkURL = LinkURL.Replace( "\"", "" );
    LinkURL = LinkURL.Replace( " class=read-more", "" );

    // If it was https:// it would start with that.
    if( !( LinkURL.StartsWith( "http://" ) ||
           LinkURL.StartsWith( "https://" )))
      LinkURL = "http://www.durangoherald.com" + LinkURL;

    if( !LinkURLIsGood( LinkURL ))
      return;

    if( LinkTitle.StartsWith( "img src=" ))
      {
      // Ignoring image links for now.
      // ParseMoreComplexLink( Parts );
      return;
      }

    if( LinkTitle.StartsWith( "img alt=" ))
      {
      // Ignoring image links for now.
      // ParseMoreComplexLink( Parts );
      return;
      }

    if( !LinkTitle.EndsWith( "/a" ))
      {
      ParseMoreComplexLink( Parts );
      return;
      }

    LinkTitle = LinkTitle.Replace( "/a", "" );

    // a href="/section/News01/"
    if( !LinkURL.StartsWith( "http://" ))
      LinkURL = "http://www.durangoherald.com" + LinkURL;

    // CleanUnicodeString() trims it.
    // LinkURL = LinkURL.Trim();
    // LinkTitle = LinkTitle.Trim();

    MForm.ShowStatus( " " );
    MForm.ShowStatus( "LinkURL: " + LinkURL );
    MForm.ShowStatus( "LinkTitle: " + LinkTitle );

    if( !MForm.PageList1.ContainsURL( LinkURL ))
      {
      // Get this new page:
      if( MForm.GetURLMgrForm != null )
        MForm.GetURLMgrForm.AddURLForm( LinkTitle, LinkURL );

      }
    }



  private void ParseMoreComplexLink( string[] Parts )
    {
    MForm.ShowStatus( " " );
    MForm.ShowStatus( "More complex link parts:" );

    for( int Count = 0; Count < Parts.Length; Count++ )
      MForm.ShowStatus( Count.ToString() + ") " + Parts[Count].Trim() );

/*
Link Parts:
0) a href="/section/goldking/" style=""
1) h1 style="text-align: center;margin: 10px 0 20px 0; font-size: 46px;"
2) Gold King Mine spill: Six months later/h1
3) /a
4) 
*/
    }



  private bool LinkURLIsGood( string LinkURL )
    {
    // This is hard-coded for now but it might 
    // come from a configuration file and
    // dictionary.

    LinkURL = LinkURL.ToLower();

    // Limit the scope of this project to this for now:
    if( !LinkURL.StartsWith( "http://www.durangoherald.com/" ))
      return false;

    if( LinkURL.Contains( "fb_comment_link" ))
      return false;

    if( LinkURL.Contains( "fb:comments-count" ))
      return false;

    if( LinkURL.Contains( "class=popups" ))
      return false;

    if( LinkURL.Contains( "apps/pbcs.dll" ))
      return false;

    /*
    if( LinkURL.Contains( "ch.tbe.taleo.net" ))
      return false;

    if( LinkURL.Contains( "mycapture.com" ))
      return false;

    if( LinkURL.Contains( "secondstreetapp.com" ))
      return false;

    if( LinkURL.Contains( "issuu.com" ))
      return false;

    if( LinkURL.Contains( "marketplace.durangoherald.com" ))
      return false;

    if( LinkURL.Contains( "fourcornersmarketplace.com" ))
      return false;

    if( LinkURL.Contains( "fourcornersexpos.com" ))
      return false;

    if( LinkURL.Contains( "thecloudscout.com" ))
      return false;

    if( LinkURL.Contains( "facebook.com" ))
      return false;

    if( LinkURL.Contains( "twitter.com" ))
      return false;

    if( LinkURL.Contains( "fourcornersschoolpubs.com" ))
      return false;

    if( LinkURL.Contains( "thedurangoheraldsmallpress.com" ))
      return false;

    if( LinkURL.Contains( "directoryplus.com" ))
      return false;

    if( LinkURL.Contains( "bcimedia.com" ))
      return false;

    if( LinkURL.Contains( "doradomagazine.com" ))
      return false;

    if( LinkURL.Contains( "adventurepro.us" ))
      return false;

    if( LinkURL.Contains( "swscene.com" ))
      return false;

    if( LinkURL.Contains( "4flagtv.com" ))
      return false;

    if( LinkURL.Contains( "4cornerstv.com" ))
      return false;

    if( LinkURL.Contains( "pinerivertimes.com" ))
      return false;

    if( LinkURL.Contains( "the-journal.com" ))
      return false;

    */

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
    AllLines = RemoveFromStartToEnd( "<!doctype", "\">", AllLines );
    AllLines = RemoveFromStartToEnd( "<link rel=\"stylesheet\"", "/>", AllLines );
    AllLines = RemoveFromStartToEnd( "<meta http-equiv=", "/>", AllLines );

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

    // AllLines = AllLines.Replace( "/", " " );
    // AllLines = AllLines.Replace( "=", " " );
    // AllLines = AllLines.Replace( "\\", " " );
    // AllLines = AllLines.Replace( ";", " " );

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

    MForm.AllWords.ShowAllWords();
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



  private string RemoveFromStartToEnd( string Start, string End, string InString )
    {
    // Regular expressions?

    if( Start.Length > 100 )
      return "Start length can't be more than 100.";

    if( End.Length > 100 )
      return "End length can't be more than 100.";

    char[] StartBuf = new char[100];
    char[] EndBuf = new char[100];

    int StartIndex = 0;
    int EndIndex = 0;
    StringBuilder SBuilder = new StringBuilder();
    bool IsInside = false;
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      if( !IsInside )
        SBuilder.Append( InString[Count] );

      if( !IsInside )
        {
        // ToLower() so it matches something like <ScRipT
        StartBuf[StartIndex] = Char.ToLower( InString[Count] );
        if( StartBuf[StartIndex] == Start[StartIndex] )
          {
          StartIndex++;
          }
        else
          {
          // No match, so start at zero again.
          StartIndex = 0;
          StartBuf[StartIndex] = Char.ToLower( InString[Count] );
          // Is it already at the beginning of a match here?
          // pppppattern
          if( StartBuf[StartIndex] == Start[StartIndex] )
            StartIndex++;

          }

        if( StartIndex == Start.Length )
          {
          // Remove the start pattern.
          int TruncateToLength = SBuilder.Length - Start.Length;
          // if( TruncateToLength >= 0 )
          SBuilder.Length = TruncateToLength;

          IsInside = true;
          EndIndex = 0;
          }
        }

      // This is not the "else" from above because
      // it might have changed above.
      if( IsInside )
        {
        EndBuf[EndIndex] = Char.ToLower( InString[Count] );
        if( EndBuf[EndIndex] == End[EndIndex] )
          {
          EndIndex++;
          }
        else
          {
          EndIndex = 0;
          EndBuf[EndIndex] = Char.ToLower( InString[Count] );
          if( EndBuf[EndIndex] == End[EndIndex] )
            EndIndex++;

          }

        if( EndIndex == End.Length )
          {
          IsInside = false;
          StartIndex = 0;
          }
        }
      }

    return SBuilder.ToString();
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

