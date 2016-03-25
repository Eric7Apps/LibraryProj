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



  internal bool ContainsURL( string URL )
    {
    return PageDictionary.ContainsKey( URL );
    }



  internal int GetIndex( string URL )
    {
    if( !PageDictionary.ContainsKey( URL ))
      return -1;

    int Index = PageDictionary[URL].GetIndex();
    URLIndexDictionary[Index] = URL;
    return Index;
    }



  internal void UpdatePageFromFile( string Title, string URL, string FileName, bool SetTime, string RelativeURLBase )
    {
    if( !MForm.CheckEvents())
      return;

    if( URL == null )
      {
      MForm.ShowStatus( "URL is null in UpdatePageFromTempFile()." );
      return;
      }

    if( Title == null )
      {
      MForm.ShowStatus( "Title is null in UpdatePageFromTempFile()." );
      return;
      }

    if( URL.Length < 10 )
      {
      MForm.ShowStatus( "URL is too short in UpdatePageFromTempFile()." );
      return;
      }

    // "Main Page"
    if( Title.Length < 5 )
      {
      MForm.ShowStatus( "Title is too short in UpdatePageFromTempFile()." );
      return;
      }

    Page UsePage;
    if( PageDictionary.ContainsKey( URL ))
      {
      UsePage = PageDictionary[URL];
      }
    else
      {
      UsePage = new Page( MForm );
      PageDictionary[URL] = UsePage;
      PageDictionary[URL].SetIndex( NextIndex );
      URLIndexDictionary[NextIndex] = URL;
      NextIndex++;
      }

    // Notice that if the URL aleady exists then
    // the contents of that page are updated but
    // the old contents are not saved.  So for example
    // the main index page at www.durangoherald.com
    // is updated but the old one is not saved.

    // When this page gets parsed it will refer back
    // to this PageList to get the index for the URL
    // by using GetIndex(), which looks in the
    // PageDictionary.
    UsePage.UpdateFromFile( Title, URL, FileName, SetTime, RelativeURLBase );
    }



  internal void AddEmptyPage( string Title, string URL, string RelativeURLBase )
    {
    if( PageDictionary.ContainsKey( URL ))
      return;

    Page UsePage = new Page( MForm );
    UsePage.SetNewTitleAndURL( Title, URL, RelativeURLBase );
    PageDictionary[URL] = UsePage;
    PageDictionary[URL].SetIndex( NextIndex );
    URLIndexDictionary[NextIndex] = URL;
    NextIndex++;
    }



  internal bool ReadFromTextFile()
    {
    PageDictionary.Clear();
    if( !File.Exists( FileName ))
      return false;
      
    try
    {
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

        int PageIndex = Page1.GetIndex();
        URLIndexDictionary[PageIndex] = Page1.GetURL();

        if( PageIndex >= NextIndex )
          NextIndex = PageIndex + 1;

        PageDictionary[Page1.GetURL()] = Page1;
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

     MForm.ShowStatus( "PageList wrote " + PageDictionary.Count.ToString( "N0" ) + " page objects to the file." );
     return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write the pages data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal void ShowTitles()
    {
    SortedDictionary<string, int> TitlesDictionary = new SortedDictionary<string, int>();

    foreach( KeyValuePair<string, Page> Kvp in PageDictionary )
      {
      string Line = Kvp.Value.GetTitle() + " >  " + Kvp.Key;
      TitlesDictionary[Line] = 1;
      }

    foreach( KeyValuePair<string, int> Kvp in TitlesDictionary )
      {
      if( !MForm.CheckEvents())
        return;

      MForm.ShowStatus( Kvp.Key );
      }

    // Validate duplicate links to see if they're all
    // still there later.
    }



  internal void IndexAll()
    {
    try
    {
    ECTime StartTime = new ECTime();
    StartTime.SetToNow();

    MForm.AllWords.ClearAll();

    foreach( KeyValuePair<string, Page> Kvp in PageDictionary )
      {
      if( !MForm.CheckEvents())
        return;

      Page Page1 = Kvp.Value;
      Page1.UpdateFromFile( Page1.GetTitle(), Page1.GetURL(), Page1.GetFileName(), false, Page1.GetRelativeURLBase() );
      }

    MForm.AllWords.WriteToTextFile();
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
    MForm.ShowStatus( "Start of ReadAllFiles()." );
    foreach( KeyValuePair<string, Page> Kvp in PageDictionary )
      {
      if( !MForm.CheckEvents())
        return;

      Page Page1 = Kvp.Value;
      string ReadFileName = Page1.GetFileName();
      if( ReadFileName.Length < 1 )
        continue;

      string FileContents = Page1.ReadFromTextFile( ReadFileName );
      Page1.MoveContentsToUTF8( FileContents );
      }

    MForm.ShowStatus( "Finished ReadAllFiles()." );
    }




  internal byte[] Get24HoursPage()
    {
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
      // if( SendPage.GetContentsUpdatedIndex() < OldIndex )
        // continue;

      // "It is acceptable and approved to link to any of
      // our materials, including deep links to our site.
      // Text from our stories can be quoted when linking
      // to our content, but it must not be more than
      // one-tenth of the total word count of the story or
      // 100 words, whichever is lesser. Quoted content
      // must contain a direct link to the story from which
      // it is taken."

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

    foreach( KeyValuePair<string, string> Kvp in TitlesDictionary )
      {
      SBuilder.Append( Kvp.Value );
      }

    SBuilder.Append( "</body>\r\n</html>\r\n" );

    // This could return null if there was a problem.
    return UTF8Strings.StringToBytes( SBuilder.ToString());
    }




  }
}

