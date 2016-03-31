// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
// using System.Collections.Generic;
using System.Text;
// using System.Threading.Tasks;
using System.IO;


namespace DGOLibrary
{
  class WordsData
  {
  private MainForm MForm;
  private WordsIndex MainWordsIndex;
  private string FileName = "";
  // private string ExcludedFileName = "";


  private WordsData()
    {
    }



  internal WordsData( MainForm UseForm )
    {
    MForm = UseForm;

    MainWordsIndex = new WordsIndex( this );

    FileName = MForm.GetDataDirectory() + "WordsIndexData.txt";
    // ExcludedFileName = MForm.GetDataDirectory() + "ExcludedWordsDictionary.txt";

    // MainWordsDictionary = new SortedDictionary<string, int>();
    // ReplaceWordsDictionary = new SortedDictionary<string, string>();
    // ExcludedWordsDictionary = new SortedDictionary<string, int>();

    // AddCommonReplacements();
    }


  internal void ShowStatus( string InString )
    {
    if( MForm == null )
      return;

    MForm.ShowStatus( InString );
    }


  internal WordsIndex GetMainWordsIndex()
    {
    return MainWordsIndex;
    }


  internal void AddWord( string Word )
    {
    MainWordsIndex.AddWord( Word );
    }


  internal bool WordExists( string Word )
    {
    return MainWordsIndex.WordExists( Word );
    }


  internal void ClearAllIntCollections()
    {
    MainWordsIndex.ClearAllIntCollections();
    }


  internal void UpdateWord( string Word, string URL )
    {
    if( Word == null )
      return;

    string FixedWord = MForm.WordsDictionary1.GetValidWordForm( Word );
    if( FixedWord.Length == 0 )
      return;

    int PageIndex = MForm.PageList1.GetIndex( URL );
    if( PageIndex < 0 )
      return;

    MainWordsIndex.AddPageIndex( Word, PageIndex );
    }



  internal IntegerCollection GetIntegerLinks( string Word )
    {
    if( Word == null )
      return null;

    string FixedWord = MForm.WordsDictionary1.GetValidWordForm( Word );
    if( FixedWord.Length == 0 )
      return null;

    if( !MainWordsIndex.WordExists( FixedWord ))
      return null;

    return MainWordsIndex.GetIntegerCollection( FixedWord );
    }



  internal bool WriteToTextFile()
    {
    try
    {
    string AllWords = MainWordsIndex.GetAllWordsString();

    string[] Words = AllWords.Split( new Char[] { '\r' } );

    using( StreamWriter SWriter = new StreamWriter( FileName  )) 
      {
      for( int Count = 0; Count < Words.Length; Count++ )
        {
        SWriter.WriteLine( Words[Count] );
        }

      SWriter.WriteLine( " " );
      }

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write the Words Data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }




  }
}
