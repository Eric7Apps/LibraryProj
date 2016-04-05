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
  private SortedDictionary<string, int> ExcludedWordsDictionary;
  private string FileName = "";
  private string ExcludedFileName = "";
  private SuffixForms SuffixForms1;


  private WordsDictionary()
    {
    }



  internal WordsDictionary( MainForm UseForm )
    {
    MForm = UseForm;

    FileName = MForm.GetDataDirectory() + "WordsDictionary.txt";
    ExcludedFileName = MForm.GetDataDirectory() + "ExcludedWordsDictionary.txt";

    SuffixForms1 = new SuffixForms();
    MainWordsDictionary = new SortedDictionary<string, int>();
    ExcludedWordsDictionary = new SortedDictionary<string, int>();
    }



  internal string GetValidWordForm( string InWord, string InFile )
    {
    try
    {
    if( InWord == null )
      return "";

    InWord = InWord.Trim();

    // as, at, be, by, do, go, he, in, is, it, no,
    // of, on, or, so, to, tv, up, us...
    if( InWord.Length < 3 )
      return "";

    if( !Utility.IsALetter( InWord[0] ))
      {
      // MForm.ShowStatus( "Not a letter: " + InWord );
      return "";
      }

    InWord = InWord.ToLower();
    if( WordIsExcluded( InWord ))
      return "";

    if( MForm.MainWordsData.WordExists( InWord ) )
      return InWord;

    // It doesn't get here if it's in the main dictionary.
    // InWord = ReplaceCommonWordForms( InWord );
    // if( MainWordsDictionary.ContainsKey( InWord ))
      // return InWord;

    string FixedWord = SuffixForms1.FixSuffix( InWord, MForm.MainWordsData.GetMainWordsIndex());

    if( FixedWord.Length != 0 )
      {
      // MForm.ShowStatus( "To: " + FixedWord );
      return FixedWord;
      }

    MForm.ShowStatus( "Word? " + InWord );
    /*
    if( ContainsNonASCII( InWord ))
      {
      if( !InWord.Contains( "fiancé" ))
        MForm.ShowStatus( "File: " + InFile );

      }
      */

    return "";

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetValidWordForm():" );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }


  private bool ContainsNonASCII( string Word )
    {
    for( int Count = 0; Count < Word.Length; Count++ )
      {
      if( Word[Count] > '~' )
        return true;

      }

    return false;
    }



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



  internal bool ReadFromTextFile()
    {
    MainWordsDictionary.Clear();

    if( !File.Exists( FileName ))
      return false;

    try
    {
    // It has to read this first.
    ReadExcludedFromTextFile();
    // Write them back to be sorted and unique.
    WriteToExcludedWordsTextFile();

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
        if( WordIsExcluded( KeyWord ))
          {
          // if( KeyWord == "somthing or other" )
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



  }
}

