// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;


namespace DGOLibrary
{
  class FrequencyCounter
  {
  private MainForm MForm;
  private SortedDictionary<string, int> WordsDictionary;
  private StringRec[] StringRecArray;
  private int[] SortIndexArray;


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
    }



  internal void ClearAll()
    {
    WordsDictionary.Clear();
    }


  internal void AddString( string InString )
    {
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

    while( true )
      {
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



  }
}
