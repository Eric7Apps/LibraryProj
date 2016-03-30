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

  // private string FileName = "";
  // private string ExcludedFileName = "";


  private WordsData()
    {
    }



  internal WordsData( MainForm UseForm )
    {
    MForm = UseForm;

    MainWordsIndex = new WordsIndex( this );

    // FileName = MForm.GetDataDirectory() + "WordsDictionary.txt";
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


  internal void AddWord( string Word )
    {
    MainWordsIndex.AddWord( Word );
    }



  }
}
