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
  private string FileName = "";
  private int NextIndex = 1;

  private PageList()
    {
    }



  internal PageList( MainForm UseForm )
    {
    MForm = UseForm;
    PageDictionary = new SortedDictionary<string, Page>();
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

    return PageDictionary[URL].GetIndex();
    }



  internal void UpdatePageFromFile( string Title, string URL, string FileName, bool SetTime )
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
    UsePage.UpdateFromFile( Title, URL, FileName, SetTime );
    }



  internal void AddEmptyPage( string Title, string URL )
    {
    if( PageDictionary.ContainsKey( URL ))
      return;

    Page UsePage = new Page( MForm );
    UsePage.SetNewTitleAndURL( Title, URL );
    PageDictionary[URL] = UsePage;
    PageDictionary[URL].SetIndex( NextIndex );
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
    }



  internal void IndexAll()
    {
    MForm.AllWords.ClearAll();

    foreach( KeyValuePair<string, Page> Kvp in PageDictionary )
      {
      if( !MForm.CheckEvents())
        return;

      Page Page1 = Kvp.Value;
      Page1.UpdateFromFile( Page1.GetTitle(), Page1.GetURL(), Page1.GetFileName(), false );
      }

    MForm.AllWords.WriteToTextFile();
    }



  internal void ReadAllFilesToContentStrings()
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

      Page1.ReadFromTextFile( ReadFileName );
      }

    MForm.ShowStatus( "Finished ReadAllFiles()." );
    }



  }
}

