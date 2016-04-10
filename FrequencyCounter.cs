// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace DGOLibrary
{
  class FrequencyCounter
  {
  private MainForm MForm;
  private SortedDictionary<string, int> WordsDictionary;
  private StringRec[] StringRecArray;
  private int[] SortIndexArray;
  private string FileName = "";


  public struct StringRec
    {
    public string Word;
    public int Count;
    }


  private FrequencyCounter()
    {
    }



  internal FrequencyCounter( MainForm UseForm )
    {
    MForm = UseForm;

    WordsDictionary = new SortedDictionary<string, int>();
    FileName = MForm.GetDataDirectory() + "FrequencyCount.txt";
    }



  internal void ClearAll()
    {
    WordsDictionary.Clear();
    }


  internal void AddString( string InString )
    {
    if( InString.Length < 3 )
      return;

    InString = InString.ToLower();
    if( WordsDictionary.ContainsKey( InString ))
      {
      WordsDictionary[InString] = WordsDictionary[InString] + 1;
      }
    else
      {
      WordsDictionary[InString] = 1;
      }
    }



  private void MakeSortedArray()
    {
    try
    {
    int ArrayLength = WordsDictionary.Count;
    StringRecArray = new StringRec[ArrayLength];
    SortIndexArray = new int[ArrayLength];

    int Where = 0;
    foreach( KeyValuePair<string, int> Kvp in WordsDictionary )
      {
      StringRec Rec = new StringRec();
      Rec.Word = Kvp.Key;
      Rec.Count = Kvp.Value;

      // Drastically improve sorting and file size by
      // removing all of those unique or rare values.
      if( Rec.Count < 3 )
        continue;

      StringRecArray[Where] = Rec;
      SortIndexArray[Where] = Where;
      Where++;
      }

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in MakeSortedArray()." );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal void SortByCount()
    {
    if( WordsDictionary.Count < 2 )
      return;

    MakeSortedArray();

    int Loops = 0;
    while( true )
      {
      Loops++;
      if( (Loops & 0xFF) == 0 )
        {
        if( !MForm.CheckEvents())
          return;

        }

      bool Swapped = false;

      for( int Count = 0; Count < (StringRecArray.Length - 1); Count++ )
        {
        if( StringRecArray[SortIndexArray[Count]].Count < StringRecArray[SortIndexArray[Count + 1]].Count )
          {
          int TempIndex = SortIndexArray[Count];
          SortIndexArray[Count] = SortIndexArray[Count + 1];
          SortIndexArray[Count + 1] = TempIndex;
          Swapped = true;
          }
        }

      if( !Swapped )
        break;

      }
    }



  internal void ShowValues( int Max )
    {
    if( WordsDictionary.Count < 2 )
      return;

    for( int Count = 0; Count < StringRecArray.Length; Count++ )
      {
      if( Count >= Max )
        break;

      if( (Count & 0xFF) == 0 )
        {
        // MForm.ShowStatus( "CheckEvents." );
        if( !MForm.CheckEvents())
          return;

        }

      int WordCount = StringRecArray[SortIndexArray[Count]].Count;
      string Word = StringRecArray[SortIndexArray[Count]].Word;
      MForm.ShowStatus( WordCount.ToString() + ") " + Word );
      }
    }



  internal bool WriteToTextFile()
    {
    try
    {
    using( StreamWriter SWriter = new StreamWriter( FileName, false, Encoding.UTF8 ))
      {
      for( int Count = 0; Count < StringRecArray.Length; Count++ )
        {
        int WordCount = StringRecArray[SortIndexArray[Count]].Count;
        if( WordCount < 3 )
          continue;

        string Word = StringRecArray[SortIndexArray[Count]].Word;
        // int Weight = WordCount * Word.Length;
        string Line = WordCount.ToString() + "\t" + Word;
        SWriter.WriteLine( Line );
        }

      SWriter.WriteLine( " " );
      }

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write the frequency data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }


  }
}
