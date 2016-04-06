// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace DGOLibrary
{
  class WordsDictionary
  {
  private MainForm MForm;
  private SortedDictionary<string, int> MainWordsDictionary;
  private string FileName = "";


  private WordsDictionary()
    {
    }



  internal WordsDictionary( MainForm UseForm )
    {
    MForm = UseForm;

    FileName = MForm.GetDataDirectory() + "WordsDictionary.txt";

    MainWordsDictionary = new SortedDictionary<string, int>();
    }







  /*
  private bool WordIsExcluded( string Word )
    {
    // This is mainly needed to help find new words
    // that aren't yet in any dictionary.

    if( Word == null ) // If it ever got a null.
      return true;

    if( Word.Length < 3 )
      return true;

    if( ExcludedWordsDictionary.ContainsKey( Word ))
      return true;

    for( int Count = 0; Count < Word.Length; Count++ )
      {
      if( !Utility.IsALetter( Word[Count] ))
        return true;

      }

    return false;
    }
    */


  internal bool ReadFromTextFile()
    {
    MainWordsDictionary.Clear();

    if( !File.Exists( FileName ))
      return false;

    try
    {
    // It has to read this first.
    // ReadExcludedFromTextFile();
    // Write them back to be sorted and unique.
    // WriteToExcludedWordsTextFile();

    using( StreamReader SReader = new StreamReader( FileName, Encoding.UTF8 )) 
      {
      while( SReader.Peek() >= 0 ) 
        {
        string Line = SReader.ReadLine();
        if( Line == null )
          continue;

        Line = Line.Trim();
        if( Line == "" )
          continue;

        string[] SplitS = Line.Split( new Char[] { '\t' } );
        if( SplitS.Length < 1 )
          continue;

        string KeyWord = SplitS[0].Trim().ToLower();
        if( ExcludedWords.IsExcluded( KeyWord ))
          {
          // if( KeyWord == "something or other" )
            // MForm.ShowStatus( " is excluded." );

          continue;
          }

        MForm.MainWordsData.AddWord( KeyWord );
        MainWordsDictionary[KeyWord] = 1;
        }
      }

    // Write them back to be sorted and unique.
    WriteToTextFile();

    MForm.ShowStatus( " " );
    MForm.ShowStatus( "Words Count: " + MainWordsDictionary.Count.ToString( "N0" ));
    MForm.ShowStatus( " " );

    return true;

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the file: \r\n" + FileName );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal bool WriteToTextFile()
    {
    try
    {
    using( StreamWriter SWriter = new StreamWriter( FileName, false, Encoding.UTF8 ))
      {
      foreach( KeyValuePair<string, int> Kvp in MainWordsDictionary )
        {
        string Line = Kvp.Key; //  + "\t" + Kvp.Value.ToString();
        SWriter.WriteLine( Line );
        }

      SWriter.WriteLine( " " );
      }

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write the words dictionary data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  /*
  private bool ReadExcludedFromTextFile()
    {
    ExcludedWordsDictionary.Clear();

    if( !File.Exists( ExcludedFileName ))
      return false;

    try
    {
    using( StreamReader SReader = new StreamReader( ExcludedFileName, Encoding.UTF8 ))
      {
      while( SReader.Peek() >= 0 ) 
        {
        string Line = SReader.ReadLine();
        if( Line == null )
          continue;

        Line = Line.Trim();
        if( Line == "" )
          continue;

        string[] SplitS = Line.Split( new Char[] { '\t' } );
        if( SplitS.Length < 1 )
          continue;

        string KeyWord = SplitS[0].Trim().ToLower();
        ExcludedWordsDictionary[KeyWord] = 1;
        }
      }

    MForm.ShowStatus( " " );
    MForm.ShowStatus( "Excluded words Count: " + ExcludedWordsDictionary.Count.ToString( "N0" ));
    MForm.ShowStatus( " " );

    return true;

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the file: \r\n" + FileName );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }
    */


  /*
  internal bool WriteToExcludedWordsTextFile()
    {
    try
    {
    using( StreamWriter SWriter = new StreamWriter( ExcludedFileName, false, Encoding.UTF8 ))
      {
      foreach( KeyValuePair<string, int> Kvp in ExcludedWordsDictionary )
        {
        string Line = Kvp.Key; // + "\t" + Kvp.Value.ToString();
        SWriter.WriteLine( Line );
        }

      SWriter.WriteLine( " " );
      }

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write the excluded words dictionary data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }
    */


  }
}

