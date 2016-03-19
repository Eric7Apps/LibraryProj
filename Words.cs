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
  class Words
  {
  private MainForm MForm;
  private SortedDictionary<string, OneWord> WordsDictionary;
  private string FileName = "";

  private Words()
    {
    }



  internal Words( MainForm UseForm )
    {
    MForm = UseForm;

    WordsDictionary = new SortedDictionary<string, OneWord>();
    FileName = MForm.GetDataDirectory() + "Words.txt";
    }



  internal void UpdateWord( string InWord, string URL )
    {
    if( InWord == null )
      return;

    string FixedWord = MForm.WordsDictionary1.GetValidWordForm( InWord );
    if( FixedWord.Length == 0 )
      return;

    // This only counts for one word per file.  Not
    // all of the same words per file.
    MForm.WordsDictionary1.IncrementWordCount( FixedWord );

    OneWord Word1;
    if( !WordsDictionary.ContainsKey( FixedWord ))
      {
      Word1 = new OneWord( MForm );
      Word1.SetWord( FixedWord );
      WordsDictionary[FixedWord] = Word1;
      // MForm.ShowStatus( "New valid word: " + FixedWord );
      }
    else
      {
      Word1 = WordsDictionary[FixedWord];
      }

    Word1.UpdateURL( URL );
    }




  internal bool ContainsWord( string Word )
    {
    return WordsDictionary.ContainsKey( Word );
    }



  internal bool ReadFromTextFile()
    {
    WordsDictionary.Clear();
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

        OneWord Word1 = new OneWord( MForm );
        if( !Word1.StringToObject( Line ))
          continue;

        WordsDictionary[Word1.GetWord()] = Word1;
        }
      }

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the words file." );
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
      foreach( KeyValuePair<string, OneWord> Kvp in WordsDictionary )
        {
        string Line = Kvp.Value.ObjectToString();
        SWriter.WriteLine( Line );
        }
      }

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write the words data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  /*
  internal void ShowAllWords()
    {
    try
    {
    MForm.ShowStatus( " " );
    MForm.ShowStatus( " " );
    MForm.ShowStatus( "All Words:" );

    foreach( KeyValuePair<string, OneWord> Kvp in WordsDictionary )
      {
      // string Line = Kvp.Value.ObjectToString();
      string Word = Kvp.Key;
      // if( Word.Length == 2 )
        MForm.ShowStatus( Word );

      }
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in ShowAllWords()." );
      MForm.ShowStatus( Except.Message );
      }
    }
    */


  }
}

