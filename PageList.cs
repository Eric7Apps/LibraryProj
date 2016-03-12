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


  private PageList()
    {
    }



  internal PageList( MainForm UseForm )
    {
    MForm = UseForm;
    PageDictionary = new SortedDictionary<string, Page>();
    FileName = MForm.GetDataDirectory() + "Pages.txt";
    }



  /*
  internal bool ContainsURL( string URL )
    {
    return PageDictionary.ContainsKey( URL );
    }
    */


  internal void UpdatePageFromTempFile( string URL, string FileName )
    {
    Page UsePage;
    if( PageDictionary.ContainsKey( URL ))
      {
      UsePage = PageDictionary[URL];
      }
    else
      {
      UsePage = new Page( MForm );
      PageDictionary[URL] = UsePage;
      // Don't delete things from this dictionary unless
      // you reindex the whole thing.
      PageDictionary[URL].IndexNumber = PageDictionary.Count;
      }

    // Notice that if the URL aleady exists then
    // the contents of that page are updated but
    // the old contents are not saved.  So for example
    // the main index page at www.durangoherald.com
    // is updated but the old one is not saved.
    UsePage.UpdateFromTempFile( URL, FileName );
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

