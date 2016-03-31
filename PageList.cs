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
  class PageList
  {
  private MainForm MForm;
  private SortedDictionary<string, Page> PageDictionary;
  private SortedDictionary<int, string> URLIndexDictionary;
  private string FileName = "";
  private int NextIndex = 1;

  private PageList()
    {
    }



  internal PageList( MainForm UseForm )
    {
    MForm = UseForm;
    PageDictionary = new SortedDictionary<string, Page>();
    URLIndexDictionary = new SortedDictionary<int, string>();

    FileName = MForm.GetDataDirectory() + "Pages.txt";
    }




  internal string GetExistingURL( string URL )
    {
    try
    {
    // This key has to be lower case, but get the 
    // actual URL from the page object.  It can't
    // be case-sensitive for being unique.
    URL = URL.ToLower();

    if( URL.Length < 3 )
      return "";

    if( PageDictionary.ContainsKey( URL ))
      return URL;

    if( PageDictionary.ContainsKey( URL + "/" ))
      return URL + "/";

    string TestURL = URL;
    if( URL.EndsWith( "/" ))
      {
      TestURL = Utility.TruncateString( URL, URL.Length - 1 );
      if( PageDictionary.ContainsKey( TestURL ))
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


   /*
  // Keep this method private to this object.
  // LinkTag has something that does this too.
  private bool IsBadLinkToAdd( string URL )
    {
    URL = URL.ToLower();
    if( URL.Contains( "/frontpage/" ))
      return true;

    return false;
    }
    */


  internal int GetIndex( string URL )
    {
    try
    {
    string CheckURL = URL.ToLower();
    if( !PageDictionary.ContainsKey( CheckURL ))
      return -1;

    int Index = PageDictionary[CheckURL].GetIndex();
    URLIndexDictionary[Index] = CheckURL;
    return Index;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetIndex():" );
      MForm.ShowStatus( Except.Message );
      return -1;
      }
    }



  internal string GetURLFromIndex( int Index )
    {
    try
    {
    if( Index < 0 )
      return "";

    if( !URLIndexDictionary.ContainsKey( Index ))
      return "";

    // It's already ToLower().
    string CheckURL = URLIndexDictionary[Index];
    if( !PageDictionary.ContainsKey( CheckURL ))
      return "";

    // Get the actual URL which might be case-sensitive.
    return PageDictionary[CheckURL].GetURL();
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetURLFromIndex():" );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  internal void UpdatePageFromFile( string Title, string URL, string FileName, bool SetTime, string RelativeURLBase )
    {
    try
    {
    if( URL == null )
      return;

    // if( IsBadLinkToAdd( URL ))
      // return;

    if( Title == null )
      return;

    if( URL.Length < 10 )
      {
      MForm.ShowStatus( "URL is too short in UpdatePageFromTempFile()." );
      return;
      }

    // "Main Page"
    if( Title.Length < 3 )
      {
      MForm.ShowStatus( "Title is too short in UpdatePageFromTempFile()." );
      return;
      }

    Page UsePage;
    string CheckURL = URL.ToLower();

    // Not this:
    // if( PageDictionary.ContainsKey( CheckURL ))
    string ExistingURL = GetExistingURL( CheckURL );
    if( ExistingURL.Length > 0 )
      {
      CheckURL = ExistingURL;
      UsePage = PageDictionary[CheckURL];
      }
    else
      {
      UsePage = new Page( MForm );
      PageDictionary[CheckURL] = UsePage;
      PageDictionary[CheckURL].SetIndex( NextIndex );
      URLIndexDictionary[NextIndex] = CheckURL;
      // See notes below on the indexes.
      NextIndex++;
      }

    // Notice that if the URL aleady exists then
    // the contents of that page are updated but
    // the old contents are not saved.  So for example
    // the main index page at www.durangoherald.com
    // is updated but the old one is not saved.

    // When this page gets parsed it will refer back
    // to this PageList to get the indexes for every
    // URL in the links it is adding to the
    // PageDictionary.  So while this is being called,
    // NextIndex is being incremented for every new link.
    UsePage.UpdateFromFile( Title, URL, FileName, SetTime, RelativeURLBase, true );
    // So at this point NextIndex has changed.

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in UpdatePageFromFile():" );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal void AddEmptyPage( string Title, string URL, string RelativeURLBase )
    {
    try
    {
    if( "" != GetExistingURL( URL ))
      return;

    // if( IsBadLinkToAdd( URL ))
      // return;

    string CheckURL = URL.ToLower();
    Page UsePage = new Page( MForm );
    UsePage.SetNewTitleAndURL( Title, URL, RelativeURLBase );
    PageDictionary[CheckURL] = UsePage;
    PageDictionary[CheckURL].SetIndex( NextIndex );
    URLIndexDictionary[NextIndex] = CheckURL;
    NextIndex++;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddEmptyPage():" );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal bool ReadFromTextFile()
    {
    try
    {
    PageDictionary.Clear();
    if( !File.Exists( FileName ))
      return false;

    using( StreamReader SReader = new StreamReader( FileName  )) 
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

        // if( IsBadLinkToAdd( Page1.GetURL() ))
          // continue;

        string CheckURL = Page1.GetURL().ToLower();
        if( "" != GetExistingURL( CheckURL ))
          continue;

        int PageIndex = Page1.GetIndex();
        URLIndexDictionary[PageIndex] = CheckURL;

        if( PageIndex >= NextIndex )
          NextIndex = PageIndex + 1;

        PageDictionary[CheckURL] = Page1;
        }
      }

    MForm.ShowStatus( "Number of pages: " + PageDictionary.Count.ToString( "N0" ));

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the pages file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal bool WriteToTextFile()
    {
    try
    {
    using( StreamWriter SWriter = new StreamWriter( FileName  )) 
      {
      foreach( KeyValuePair<string, Page> Kvp in PageDictionary )
        {
        string Line = Kvp.Value.ObjectToString();
        SWriter.WriteLine( Line );
        }
      }

     // MForm.ShowStatus( "PageList wrote " + PageDictionary.Count.ToString( "N0" ) + " page objects to the file." );
     return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write the pages data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }


  /*
  internal void ShowTitles()
    {

    foreach( KeyValuePair<string, string> Kvp in TitlesDictionary )
      {
      if( !MForm.CheckEvents())
        return;

      MForm.ShowStatus( Kvp.Key );
      }

    // There are duplicate links because they fall
    // under multiple categories.
    }
    */


  internal void IndexAll()
    {
    try
    {
    ECTime StartTime = new ECTime();
    StartTime.SetToNow();

    MForm.MainWordsData.ClearAllIntCollections();

    ReadAllFilesToContent();

    int Loops = 0;
    foreach( KeyValuePair<string, Page> Kvp in PageDictionary )
      {
      Loops++;
      if( (Loops & 0x7F) == 0 )
        {
        if( !MForm.CheckEvents())
          return;

      if( MForm.GetIsClosing() )
        return;

        }

      Page Page1 = Kvp.Value;
      Page1.UpdateFromFile( Page1.GetTitle(), Page1.GetURL(), Page1.GetFileName(), false, Page1.GetRelativeURLBase(), false );
      }

    MForm.MainWordsData.WriteToTextFile();
    MForm.ShowStatus( " " );
    MForm.ShowStatus( "Finished indexing in: " + StartTime.GetSecondsToNow().ToString( "N0" ));
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in PageList.IndexAll()." );
      MForm.ShowStatus( Except.Message );
      }
    }


  internal void ReadAllFilesToContent()
    {
    MForm.ShowStatus( "Start of ReadAllFilesToContent()." );
    foreach( KeyValuePair<string, Page> Kvp in PageDictionary )
      {
      if( !MForm.CheckEvents())
        return;

      Page Page1 = Kvp.Value;
      string ReadFileName = Page1.GetFileName();
      if( ReadFileName.Length < 1 )
        continue;

      Page1.ReadToFullFileContentsString( ReadFileName );
      // Page1.MoveContentsToUTF8( FileContents );
      }

    MForm.ShowStatus( "Finished ReadAllFilesToContent()." );
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
    SBuilder.Append( "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />\r\n" );
    SBuilder.Append( "<title>The Library Project</title>\r\n" );
    SBuilder.Append( "<p><b>The Library Project.</b></p><br>\r\n" );

    SortedDictionary<string, string> TitlesDictionary = new SortedDictionary<string, string>();

    ECTime OldTime = new ECTime();
    OldTime.SetToNow();
    OldTime.AddSeconds( -(60 * 60 * 24) ); // 24 hours back.
    ulong OldIndex = OldTime.GetIndex();
    foreach( KeyValuePair<string, Page> Kvp in PageDictionary )
      {
      Page SendPage = Kvp.Value;
      if( SendPage.GetContentsUpdateTimeIndex() < OldIndex )
        continue;

      // This title is the original title from the original
      // link.  It's not from the title tag within the page.
      // It's not from the content of the page.
      string TitlePart = SendPage.GetTitle();
      string BaseURL = SendPage.GetRelativeURLBase().ToLower();
      if( BaseURL.Contains( "durangoherald.com" ))
        TitlePart = "Herald) " + TitlePart;

      if( BaseURL.Contains( "durangotelegraph.com" ))
        TitlePart = "Telegraph) " + TitlePart;

      if( BaseURL.Contains( "durangogov.org" ))
        TitlePart = "Durango Gov) " + TitlePart;

      if( BaseURL.Contains( "colorado.gov" ))
        TitlePart = "Colorado Gov) " + TitlePart;


      // To sort it by title.
      // Sort it by time index instead?
      string TagPart = "<p><a href=\"" + Kvp.Key +
        "\">" + TitlePart +
        "</a></p>\r\n";

      // There are duplicated articles with the same title
      // but different URLs.  They put the same article
      // in several different sections.  This will only
      // get the last one.  (As iterated with foreach 
      //from PageDictionary.)
      TitlesDictionary[TitlePart] = TagPart;
      }

    foreach( KeyValuePair<string, string> Kvp in TitlesDictionary )
      {
      SBuilder.Append( Kvp.Value );
      }

    SBuilder.Append( "</body>\r\n</html>\r\n" );

    // This could return null if there was a problem.
    return UTF8Strings.StringToBytes( SBuilder.ToString());
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in PageList.Get24HoursPage()." );
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
    SBuilder.Append( "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />\r\n" );
    SBuilder.Append( "<title>The Library Project</title>\r\n" );
    SBuilder.Append( "<p><b>The Library Project.</b></p><br>\r\n" );

    SortedDictionary<string, string> TitlesDictionary = new SortedDictionary<string, string>();

    foreach( KeyValuePair<string, Page> Kvp in PageDictionary )
      {
      Page SendPage = Kvp.Value;
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
      string TitlePart = SendPage.GetTitle();

      // To sort it by title.
      // Sort it by time index instead?
      string TagPart = "<p><a href=\"" + Kvp.Key +
        "\">" + TitlePart +
        "</a></p>\r\n";

      // There are duplicated articles with the same title
      // but different URLs.  They put the same article
      // in several different sections.  This will only
      // get the last one.  (As iterated with foreach 
      //from PageDictionary.)
      TitlesDictionary[TitlePart] = TagPart;
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
    SBuilder.Append( "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />\r\n" );
    SBuilder.Append( "<title>The Library Project</title>\r\n" );
    SBuilder.Append( "<p><b>The Library Project.</b></p><br>\r\n" );

    SortedDictionary<string, string> TitlesDictionary = new SortedDictionary<string, string>();

    // Obviously you would pass in these words as
    // parameters for a search, but this is just a
    // basic example.
    string Word1 = "library";
    string Word2 = "carnegie";
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
          continue;

        string CheckURL = URL.ToLower();

        // GetExistingURL()
        if( !PageDictionary.ContainsKey( CheckURL ))
          {
          MForm.ShowStatus( "Missing URL: " + URL );
          continue;
          }

        Page SendPage = PageDictionary[CheckURL];
        string TitlePart = SendPage.GetTitle();
        MForm.ShowStatus( "TitlePart: " + TitlePart );

        // To sort it by title.
        string TagPart = "<p><a href=\"" + URL +
          "\">" + TitlePart +
          "</a></p>\r\n";

      // There are duplicated articles with the same title
      // but different URLs.  They put the same article
      // in several different sections.  This will only
      // get the last one.
        TitlesDictionary[TitlePart] = TagPart;
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

