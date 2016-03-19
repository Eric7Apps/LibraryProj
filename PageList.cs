// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;


/*
		130571434983424		8
Main Page	http://www.durangoherald.com	138553911491207	C:\Users\Eric\AEric\Eric\MyStuff\DGOLibrary\bin\Release\Pages\Y2016\M3\D18\H18M42S2T45.txt	1
		130571434983424		5
		130571434983424		16
		130571434983424		17
		130571434983424		18
*/


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



  internal void UpdatePageFromTempFile( string Title, string URL, string FileName )
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
    UsePage.UpdateFromTempFile( Title, URL, FileName );
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

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write the pages data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  }
}

