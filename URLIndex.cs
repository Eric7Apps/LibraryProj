// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
using System.Text;
using System.Collections.Generic;
using System.IO;


namespace DGOLibrary
{
  class URLIndex
  {
  private MainForm MForm;
  private URLsRec[] URLsRecArray;
  private const int URLsRecArrayLength = 0xFFFF + 1;
  private string[] URLStringsArray;
  private int URLStringsArrayLast = 0;
  private string FileName = "";


  // Test this against PageList.


  public struct URLsRec
    {
    public URLIndexRec[] URLIndexArray;
    public int URLIndexArrayLast;
    }


  public struct URLIndexRec
    {
    public string URL;
    public int Index;
    public Page PageForURL;
    }


  private URLIndex()
    {
    }


  internal URLIndex( MainForm UseForm )
    {
    MForm = UseForm;
    URLsRecArray = new URLsRec[URLsRecArrayLength];
    URLStringsArray = new string[1024];
    FileName = MForm.GetDataDirectory() + "URLIndex.txt";
    }




  private int GetRandomishIndex( string URL )
    {
    string URLKey = URL.ToLower();

    // Check: URLsRecArrayLength for the size of
    // this index.

    // Get a basic randomish index.
    // A quick and easy (for now) hash function.
    int Sum = 0;
    int ExclusiveOR = 0;
    for( int Count = 0; Count < URLKey.Length; Count++ )
      {
      // A sum would be the same as Exclusive OR
      // except that it has a carry bit.
      Sum += (int)URLKey[Count];

      // No carry bit.
      ExclusiveOR = ExclusiveOR ^ (int)URLKey[Count];
      } 

    int Index = Sum & 0xFF;
    Index <<= 8;
    Index |= ExclusiveOR & 0xFF;
    return Index; // A 16 bit number.
    }



  internal Page GetPage( string URL )
    {
    try
    {
    if( URL == null )
      return null;

    if( URL.Length < 5 )
      return null;

    string ExistingURL = GetExistingURL( URL );
    if( ExistingURL.Length == 0 )
      {
      MForm.ShowStatus( "There is no existing URL for GetPage()." );
      MForm.ShowStatus( ExistingURL );
      return null;
      }

    int Index = GetRandomishIndex( ExistingURL );

    if( URLsRecArray[Index].URLIndexArray ==  null )
      {
      MForm.ShowStatus( "The URLIndexArray was null for:" );
      MForm.ShowStatus( ExistingURL );
      return null;
      }

    string URLKey = ExistingURL.ToLower();
    int Last = URLsRecArray[Index].URLIndexArrayLast;
    for( int Count = 0; Count < Last; Count++ )
      {
      if( URLKey == URLsRecArray[Index].URLIndexArray[Count].URL.ToLower() )
        {
        Page Result =  URLsRecArray[Index].URLIndexArray[Count].PageForURL;
        if( Result == null )
          {
          Result = new Page( MForm );
          URLsRecArray[Index].URLIndexArray[Count].PageForURL = Result;
          }

        return Result;
        }
      }

    MForm.ShowStatus( "Didn't find a matching URL for:" );
    MForm.ShowStatus( ExistingURL );
    return null;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetExistingPage():" );
      MForm.ShowStatus( Except.Message );
      return null;
      }
    }



  internal string GetExistingURL( string URL )
    {
    try
    {
    if( URL == null )
      return "";

    if( URL.Length < 5 )
      return "";

    // This checks with ToLower().
    if( ExactURLExists2( URL ))
      return URL;

    if( ExactURLExists2( URL + "/" ))
      return URL + "/";

    string TestURL = URL;
    if( URL.EndsWith( "/" ))
      {
      TestURL = Utility.TruncateString( URL, URL.Length - 1 );
      if( ExactURLExists2( TestURL ))
        return TestURL;

      }

    TestURL = URL;
    if( URL.EndsWith( "?clear=True" ))
      {
      TestURL = Utility.TruncateString( URL, URL.Length - 11 );
      if( ExactURLExists2( TestURL ))
        return TestURL;

      }


    // There are duplicate links because they fall
    // under multiple categories or because they are
    // just messed up and duplicated all over the place.

    // TitlesDictionary

    // Durango track and field team strong in Farmington
    // /20160320/SPORTS03/160329972/0/Sports/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/0/Sports01/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/0/Sports02/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/0/Sports03/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/0/Sports04/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/0/Sports05/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/-1/Sports
    // /20160320/SPORTS03/160329972/-1/Sports03/Durango-track-and-field-team-strong-in-Farmington
    // /20160320/SPORTS03/160329972/Durango-track-and-field-team-strong-in-Farmington

    // http://www.durangoherald.com/article/

    // 4-H garden is gateway to lessons about health
    // /20160316/COLUMNISTS05/160319649/-1/Columnists
    // /20160316/COLUMNISTS05/160319649/-1/Lifestyle
    // /20160316/COLUMNISTS05/160319649/-1/Lifestyle01
    // /20160316/COLUMNISTS05/160319649/-1/Lifestyle04
    // /20160316/COLUMNISTS05/160319649/-1/News06

    return "";
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetExistingURL():" );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  private bool ExactURLExists2( string URL )
    {
    try
    {
    // This does ToLower().
    int Index = GetRandomishIndex( URL );

    if( URLsRecArray[Index].URLIndexArray ==  null )
      return false;

    string URLKey = URL.ToLower();
    int Last = URLsRecArray[Index].URLIndexArrayLast;
    for( int Count = 0; Count < Last; Count++ )
      {
      if( URLKey == URLsRecArray[Index].URLIndexArray[Count].URL.ToLower() )
        return true;

      }

    return false;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in ExactURLExists():" );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal void AddEmptyPage( string Title, string URL, string RelativeURLBase )
    {
    try
    {
    if( "" != GetExistingURL( URL ))
      return;

    if( LinkTag.LinkIsBad( URL, Title, RelativeURLBase ))
      return;

    Page PageToAdd = new Page( MForm );
    PageToAdd.SetNewTitleAndURL( Title, URL, RelativeURLBase );
    AddURL( URL, PageToAdd );

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddEmptyPage():" );
      MForm.ShowStatus( Except.Message );
      }
    }


  internal void AddURL( string URL, Page PageToAdd )
    {
    try
    {
    string Test = GetExistingURL( URL );
    if( Test.Length > 0 )
      {
      // MForm.ShowStatus( "The URL is already there in AddURL()." );
      return;
      }

    URLIndexRec Rec = new URLIndexRec();
    Rec.URL = URL;
    Rec.PageForURL = PageToAdd; // Might be null.

    URLStringsArray[URLStringsArrayLast] = URL;
    Rec.Index = URLStringsArrayLast;
    URLStringsArrayLast++;
    if( URLStringsArrayLast >= URLStringsArray.Length )
      Array.Resize( ref URLStringsArray, URLStringsArray.Length + 1024 );

    int Index = GetRandomishIndex( Rec.URL );

    if( URLsRecArray[Index].URLIndexArray ==  null )
      URLsRecArray[Index].URLIndexArray = new URLIndexRec[64];

    URLsRecArray[Index].URLIndexArray[URLsRecArray[Index].URLIndexArrayLast] = Rec;
    URLsRecArray[Index].URLIndexArrayLast++;
    if( URLsRecArray[Index].URLIndexArrayLast >= URLsRecArray[Index].URLIndexArray.Length )
      Array.Resize( ref URLsRecArray[Index].URLIndexArray, URLsRecArray[Index].URLIndexArray.Length + 64 );

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddURL():" );
      MForm.ShowStatus( Except.Message );
      }
    }



  private string GetURLFromIndex( int Where )
    {
    if( Where < 0 )
      {
      MForm.ShowStatus( "Where < 0 in GetURLFromIndex()." );
      return "";
      }

    if( Where >= URLStringsArrayLast )
      {
      MForm.ShowStatus( "Where >= URLStringsArrayLast in GetURLFromIndex()." );
      return "";
      }

    string Result = URLStringsArray[Where];
    if( Result == null )
      return "";

    return Result;
    }



  internal int GetIndex( string URL )
    {
    try
    {
    string FixedURL = GetExistingURL( URL );
    if( FixedURL.Length == 0 )
      return -1;

    int Index = GetRandomishIndex( FixedURL );

    if( URLsRecArray[Index].URLIndexArray ==  null )
      return -1;

    int Last = URLsRecArray[Index].URLIndexArrayLast;
    string URLKey = FixedURL.ToLower();
    for( int Count = 0; Count < Last; Count++ )
      {
      if( URLKey == URLsRecArray[Index].URLIndexArray[Count].URL.ToLower() )
        return URLsRecArray[Index].URLIndexArray[Count].Index;

      }

    return -1;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetIndex():" );
      MForm.ShowStatus( Except.Message );
      return -1;
      }
    }



  internal void UpdatePageFromFile( string Title, string URL, string FileName, bool SetTime, string RelativeURLBase, bool ReadFromFile )
    {
    try
    {
    if( URL == null )
      return;

    if( Title == null )
      return;

    if( LinkTag.LinkIsBad( URL, Title, RelativeURLBase ))
      return;

    if( URL.Length < 10 )
      {
      MForm.ShowStatus( "URL is too short in UpdatePageFromTempFile()." );
      return;
      }

    if( Title.Length < 3 )
      {
      // MForm.ShowStatus( "Title is too short in UpdatePageFromTempFile()." );
      // MForm.ShowStatus( "Title: " + Title );
      return;
      }

    string ExistingURL = GetExistingURL( URL );
    if( ExistingURL.Length == 0 )
      {
      AddURL( URL, null );
      ExistingURL = URL;
      }

    // It will create an empty one if it doesn't have it.
    Page UsePage = GetPage( ExistingURL );
    if( UsePage == null )
      {
      MForm.ShowStatus( "There was an error getting the page for:" );
      MForm.ShowStatus( ExistingURL );
      return;
      }

    // Notice that if the URL aleady exists then
    // the contents of that page are updated but
    // the old contents are not saved.  So for example
    // the main index page at www.durangoherald.com
    // is updated but the old one is not saved.

    // When this page gets parsed it will refer back
    // to this URLIndex to get the indexes for every
    // URL in the links it is adding.  So while this
    // is being called, new URLs are being added.
    UsePage.UpdateFromFile( Title, ExistingURL, FileName, SetTime, RelativeURLBase, ReadFromFile );
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in URLIndex.UpdatePageFromFile():" );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal bool WriteToTextFile()
    {
    try
    {
    int Written = 0;
    using( StreamWriter SWriter = new StreamWriter( FileName, false, Encoding.UTF8 ))
      {
      for( int Index = 0; Index < URLsRecArrayLength; Index++ )
        {
        if( URLsRecArray[Index].URLIndexArray ==  null )
          continue;

        int Last = URLsRecArray[Index].URLIndexArrayLast;
        for( int Count = 0; Count < Last; Count++ )
          {
          Page OnePage =  URLsRecArray[Index].URLIndexArray[Count].PageForURL;
          if( OnePage == null )
            continue;

          Written++;
          // string Line = Index.ToString() + ") " + OnePage.ObjectToString();
          string Line = OnePage.ObjectToString();
          SWriter.WriteLine( Line );
          }
        }
      }

    MForm.ShowStatus( "URLIndex wrote " + Written.ToString( "N0" ) + " page objects to the file." );
    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in URLIndex.WriteToTextFile():" );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  private void ClearAll()
    {
    try
    {
    for( int Index = 0; Index < URLsRecArrayLength; Index++ )
      {
      URLsRecArray[Index].URLIndexArray =  null;
      URLsRecArray[Index].URLIndexArrayLast = 0;
      }
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in URLIndex.ClearAll():" );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal bool ReadFromTextFile()
    {
    try
    {
    ClearAll();
    if( !File.Exists( FileName ))
      return false;

    int HowMany = 0;
    using( StreamReader SReader = new StreamReader( FileName, Encoding.UTF8 ))
      {
      while( SReader.Peek() >= 0 ) 
        {
        string Line = SReader.ReadLine();
        if( Line == null )
          continue;

        if( !Line.Contains( "\t" ))
          continue;

        Page Page1 = new Page( MForm );
        if( !Page1.StringToObject( Line ))
          continue;

        string CheckURL = Page1.GetURL();
        if( LinkTag.LinkIsBad( CheckURL, Page1.GetTitle(), Page1.GetRelativeURLBase() ))
          continue;

        if( "" != GetExistingURL( CheckURL ))
          continue;

        HowMany++;
        AddURL( CheckURL, Page1 );
        }
      }

    MForm.ShowStatus( "Number of pages: " + HowMany.ToString( "N0" ));
    int Total = GetTotalPages();
    MForm.ShowStatus( "Total pages: " + Total.ToString( "N0" ));

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the URLIndex.txt file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal int GetTotalPages()
    {
    try
    {
    int Total = 0;
    for( int Index = 0; Index < URLsRecArrayLength; Index++ )
      {
      if( URLsRecArray[Index].URLIndexArray ==  null )
        continue;

      Total += URLsRecArray[Index].URLIndexArrayLast;
      }

    return Total;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetTotalPages():" );
      MForm.ShowStatus( Except.Message );
      return -1;
      }
    }



  internal void ReadAllFilesToContent()
    {
    MForm.ShowStatus( "Start of ReadAllFilesToContent()." );
    int Loops = 0;

    for( int Index = 0; Index < URLsRecArrayLength; Index++ )
      {
      if( URLsRecArray[Index].URLIndexArray ==  null )
        continue;

      int Last = URLsRecArray[Index].URLIndexArrayLast;
      for( int Count = 0; Count < Last; Count++ )
        {
        Loops++;
        if( (Loops & 0x3F) == 0 )
          {
          MForm.ShowStatus( "Files: " + Loops.ToString( "N0" ));
          if( !MForm.CheckEvents())
            return;

          }

        Page Page1 =  URLsRecArray[Index].URLIndexArray[Count].PageForURL;
        if( Page1 == null )
          continue;

        string ReadFileName = Page1.GetFileName();
        if( ReadFileName.Length < 1 )
          continue;

        Page1.ReadToFullFileContentsString( ReadFileName );
        // Page1.MoveContentsToUTF8( FileContents );
        }
      }

    MForm.ShowStatus( "Finished ReadAllFilesToContent()." );
    }



  internal void IndexAll()
    {
    try
    {
    ECTime StartTime = new ECTime();
    MForm.MainWordsData.ClearAllIntCollections();
    ReadAllFilesToContent();

    StartTime.SetToNow();

    int Loops = 0;
    for( int Index = 0; Index < URLsRecArrayLength; Index++ )
      {
      if( URLsRecArray[Index].URLIndexArray ==  null )
        continue;

      int Last = URLsRecArray[Index].URLIndexArrayLast;
      for( int Count = 0; Count < Last; Count++ )
        {
        Loops++;
        if( (Loops & 0x1F) == 0 )
          {
          // MForm.ShowStatus( "Files: " + Loops.ToString( "N0" ));
          if( !MForm.CheckEvents())
            return;

          if( MForm.GetIsClosing() )
            return;

          }

        Page Page1 =  URLsRecArray[Index].URLIndexArray[Count].PageForURL;
        if( Page1 == null )
          continue;

        Page1.UpdateFromFile( Page1.GetTitle(), Page1.GetURL(), Page1.GetFileName(), false, Page1.GetRelativeURLBase(), false );
        }
      }

    MForm.MainWordsData.WriteToTextFile();
    MForm.ShowStatus( " " );
    int Seconds = (int)StartTime.GetSecondsToNow();
    int Minutes = Seconds / 60;
    Seconds = Seconds % 60;
    MForm.ShowStatus( "Finished indexing in: " + Minutes.ToString( "N0" ) + ":" + Seconds.ToString());
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in URLIndex.IndexAll()." );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal void ShowDuplicateFileNames()
    {
    // Running this more than once would clear triplicate
    // file names, and quadruplicate, etc.
    try
    {
    ECTime StartTime = new ECTime();
    StartTime.SetToNow();

    int HowMany = 0;
    int Loops = 0;
    for( int Index = 0; Index < URLsRecArrayLength; Index++ )
      {
      if( URLsRecArray[Index].URLIndexArray ==  null )
        continue;

      Loops++;
      if( (Loops & 0x7F) == 0 )
        {
        MForm.ShowStatus( "Duplicate files check: " + Loops.ToString( "N0" ));
        if( !MForm.CheckEvents())
          return;

        if( MForm.GetIsClosing() )
          return;

        }

      for( int Index2 = Index; Index2 < URLsRecArrayLength; Index2++ )
        {
        if( URLsRecArray[Index2].URLIndexArray ==  null )
          continue;

        int Last = URLsRecArray[Index].URLIndexArrayLast;
        int Last2 = URLsRecArray[Index2].URLIndexArrayLast;
        for( int Count = 0; Count < Last; Count++ )
          {
          Page Page1 =  URLsRecArray[Index].URLIndexArray[Count].PageForURL;
          if( Page1 == null )
            {
            MForm.ShowStatus( "Page was null." );
            continue;
            }

          // If it's one of the empty pages that are 
          // added in GetURLManagerForm.cs.
          if( Page1.GetFileName() == "" )
            continue;

          for( int Count2 = 0; Count2 < Last2; Count2++ )
            {
            if( (Index == Index2) && (Count == Count2) )
              continue;

            Page Page2 =  URLsRecArray[Index2].URLIndexArray[Count2].PageForURL;
            if( Page2 == null )
              {
              MForm.ShowStatus( "Page was null." );
              continue;
              }

            if( Page2.GetFileName() == "" )
              continue;

            if( Page1.GetFileName() == Page2.GetFileName())
              {
              // Page1.ClearFileName();
              // Page2.ClearFileName();
              HowMany++;
              MForm.ShowStatus( " " );
              MForm.ShowStatus( "Title 1: " + Page1.GetTitle() );
              MForm.ShowStatus( "Duplicate Title 2: " + Page2.GetTitle() );
              MForm.ShowStatus( "File name: " + Page1.GetFileName() );
              }
            }
          }
        }
      }

    MForm.ShowStatus( "Finished checking duplicates.");
    MForm.ShowStatus( "HowMany: " + HowMany.ToString( "N0" ));

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in URLIndex.ShowDuplicateFileNames()." );
      MForm.ShowStatus( Except.Message );
      }
    }



  private string GetTitlePart( string Title, string BaseURL )
    {
    if( BaseURL.Contains( "durangoherald.com" ))
      return "Herald) " + Title;

    if( BaseURL.Contains( "durangotelegraph.com" ))
      return "Telegraph) " + Title;

    if( BaseURL.Contains( "durangogov.org" ))
      return "Durango Gov) " + Title;

    if( BaseURL.Contains( "colorado.gov" ))
      return "Colorado Gov) " + Title;

    return Title;
    }



  internal byte[] Get24HoursPage()
    {
    try
    {
    // http://127.0.0.1/get24hours.htm

    StringBuilder SBuilder = new StringBuilder();

    // Obviously you could get parts of pages like
    // this from files or something, but this is just
    // a very basic example with no style sheets or
    // anything like that in it.
    SBuilder.Append( "<!DOCTYPE html>\r\n" );
    SBuilder.Append( "<html>\r\n<head>\r\n" );

    // charset=ISO-8859-1 is obsolete old 8 bit encoding.
    // See below where it does UTF8Strings.StringToBytes().
    SBuilder.Append( "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n" );

    SBuilder.Append( "<title>The Library Project</title>\r\n" );
    SBuilder.Append( "<p><b>The Library Project.</b></p><br>\r\n" );

    SortedDictionary<string, string> TitlesDictionary = new SortedDictionary<string, string>();

    ECTime OldTime = new ECTime();
    OldTime.SetToNow();
    OldTime.AddSeconds( -(60 * 60 * 24) ); // 24 hours back.
    ulong OldDateIndex = OldTime.GetIndex();

    for( int Index = 0; Index < URLsRecArrayLength; Index++ )
      {
      if( URLsRecArray[Index].URLIndexArray ==  null )
        continue;

      int Last = URLsRecArray[Index].URLIndexArrayLast;
      for( int Count = 0; Count < Last; Count++ )
        {
        Page SendPage =  URLsRecArray[Index].URLIndexArray[Count].PageForURL;
        if( SendPage == null )
          continue;

        if( SendPage.GetContentsUpdateTimeIndex() < OldDateIndex )
          continue;

        // This title is the original title from the original
        // link.  It's not from the title tag within the page.
        // It's not from the content of the page.
        string Title = SendPage.GetTitle();
        string URL = SendPage.GetURL();
        string BaseURL = SendPage.GetRelativeURLBase().ToLower();
        Title = GetTitlePart( Title, BaseURL );

        // To sort it by title.
        // Sort it by time index instead?
        string TagPart = "<p><a href=\"" + URL +
          "\">" + Title +
          "</a></p>\r\n";

        // There are duplicated articles with the same title
        // but different URLs.  They put the same article
        // in several different sections.  This will only
        // get the last one.  (As the iteratation is done
        // here by the URL index.
        TitlesDictionary[Title] = TagPart;
        }
      }

    if( TitlesDictionary.Count > 0 )
      {
      foreach( KeyValuePair<string, string> Kvp in TitlesDictionary )
        {
        SBuilder.Append( Kvp.Value );
        }
      }
    else
      {
      SBuilder.Append( "<p>Nothing was found.</p>\r\n" );
      }

    SBuilder.Append( "</body>\r\n</html>\r\n" );

    // This could return null if there was a problem.
    return UTF8Strings.StringToBytes( SBuilder.ToString());
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in URLIndex.Get24HoursPage()." );
      MForm.ShowStatus( Except.Message );
      return null;
      }
    }




  internal byte[] GetCrudeSearchPage()
    {
    try
    {
    // http://127.0.0.1/CrudeSearch.htm

    StringBuilder SBuilder = new StringBuilder();

    SBuilder.Append( "<!DOCTYPE html>\r\n" );
    SBuilder.Append( "<html>\r\n<head>\r\n" );
    // See below where it does UTF8Strings.StringToBytes().
    SBuilder.Append( "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n" );
    SBuilder.Append( "<title>The Library Project</title>\r\n" );
    SBuilder.Append( "<p><b>The Library Project.</b></p><br>\r\n" );

    SortedDictionary<string, string> TitlesDictionary = new SortedDictionary<string, string>();

    for( int Index = 0; Index < URLsRecArrayLength; Index++ )
      {
      if( URLsRecArray[Index].URLIndexArray ==  null )
        continue;

      int Last = URLsRecArray[Index].URLIndexArrayLast;
      for( int Count = 0; Count < Last; Count++ )
        {
        Page SendPage =  URLsRecArray[Index].URLIndexArray[Count].PageForURL;
        if( SendPage == null )
          continue;

        string Contents = SendPage.GetSearchableContents();

        // Obviously you would pass in this word as a
        // parameter for a search, but this is just a
        // basic example.
        if( !Contents.Contains( "library" ))
          continue;

        if( !Contents.Contains( "carnegie" ))
          continue;

        MForm.ShowStatus( " " );
        MForm.ShowStatus( " " );
        MForm.ShowStatus( SendPage.GetFileName());
        MForm.ShowStatus( Contents );

        // This title is the original title from the original
        // link.  It's not from the title tag within the page.
        // It's not from the content of the page.
        string URL = SendPage.GetURL();
        string BaseURL = SendPage.GetRelativeURLBase().ToLower();
        string Title = SendPage.GetTitle();
        Title = GetTitlePart( Title, BaseURL );

        // To sort it by title.
        // Sort it by time index instead?
        string TagPart = "<p><a href=\"" + URL +
          "\">" + Title +
          "</a></p>\r\n";

        TitlesDictionary[Title] = TagPart;
        }
      }

    if( TitlesDictionary.Count > 0 )
      {
      foreach( KeyValuePair<string, string> Kvp in TitlesDictionary )
        {
        SBuilder.Append( Kvp.Value );
        }
      }
    else
      {
      SBuilder.Append( "<p>Nothing was found.</p>\r\n" );
      }

    SBuilder.Append( "</body>\r\n</html>\r\n" );

    // This could return null if there was a problem.
    return UTF8Strings.StringToBytes( SBuilder.ToString());

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in PageList.GetCrudeSearchPage()." );
      MForm.ShowStatus( Except.Message );
      return null;
      }
    }




  internal byte[] GetIndexedSearchPage()
    {
    try
    {
    // http://127.0.0.1/IndexedSearch.htm

    StringBuilder SBuilder = new StringBuilder();

    SBuilder.Append( "<!DOCTYPE html>\r\n" );
    SBuilder.Append( "<html>\r\n<head>\r\n" );
    // See below where it does UTF8Strings.StringToBytes().
    SBuilder.Append( "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n" );
    SBuilder.Append( "<title>The Library Project</title>\r\n" );
    SBuilder.Append( "<p><b>The Library Project.</b></p><br>\r\n" );

    SortedDictionary<string, string> TitlesDictionary = new SortedDictionary<string, string>();

    // Obviously you would pass in these words as
    // parameters for a search, but this is just a
    // basic example.
    string Word1 = "library";
    string Word2 = "carnegie";
    // The words should be checked for valid form before
    // calling this:
    IntegerCollection IntCol1 = MForm.MainWordsData.GetIntegerLinks( Word1 );
    IntegerCollection IntCol2 = MForm.MainWordsData.GetIntegerLinks( Word2 );

    IntegerCollection IntColBoth = null;

    // This makes it search for one word if only
    // one was found.
    if( IntCol1 == null )
      IntColBoth = IntCol2;

    if( IntCol2 == null )
      IntColBoth = IntCol1;

    // If neither or both were null.
    if( IntColBoth == null )
      {
      MForm.ShowStatus( "IntColBoth is null." );
      // If either one is null then this will return an
      // empty object.
      IntColBoth = new IntegerCollection();
      IntColBoth.LogicANDFromCollections( IntCol1, IntCol2 );
      }

    int[] IndexArray = IntColBoth.GetIndexArray();
    if( IndexArray != null )
      {
      MForm.ShowStatus( "IndexArray.Length: " + IndexArray.Length.ToString());
      for( int Count = 0; Count < IndexArray.Length; Count++ )
        {
        string URL = GetURLFromIndex( IndexArray[Count] );
        if( URL.Length == 0 )
          {
          MForm.ShowStatus( "No URL at index: " + IndexArray[Count].ToString() );
          continue;
          }

        Page SendPage = GetPage( URL );
        if( SendPage == null )
          {
          MForm.ShowStatus( "The page was null for: " + URL );
          continue;
          }

        // string URL = SendPage.GetURL();
        string BaseURL = SendPage.GetRelativeURLBase().ToLower();
        string Title = SendPage.GetTitle();
        Title = GetTitlePart( Title, BaseURL );

        // To sort it by title.
        string TagPart = "<p><a href=\"" + URL +
          "\">" + Title +
          "</a></p>\r\n";

        TitlesDictionary[Title] = TagPart;
        }
      }
    else
      {
      MForm.ShowStatus( "IndexArray was null." );
      }

    if( TitlesDictionary.Count > 0 )
      {
      foreach( KeyValuePair<string, string> Kvp in TitlesDictionary )
        {
        SBuilder.Append( Kvp.Value );
        }
      }
    else
      {
      MForm.ShowStatus( "TitlesDictionary was empty." );
      SBuilder.Append( "<p>Nothing was found.</p>\r\n" );
      }

    SBuilder.Append( "</body>\r\n</html>\r\n" );
    MForm.ShowStatus( "Done with indexed search." );

    // This could return null if there was a problem.
    return UTF8Strings.StringToBytes( SBuilder.ToString());
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in PageList.GetIndexedSearchPage()." );
      MForm.ShowStatus( Except.Message );
      return null;
      }
    }


  }
}


