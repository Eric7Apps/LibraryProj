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
  private SortedDictionary<char, long> CharacterDictionary;
  private StringRec[] StringRecArray;
  private int[] SortIndexArray;
  private int StringRecArrayLast = 0;
  private string FileName = "";


  public struct StringRec
    {
    public string Word;
    public int Count;
    }


  private FrequencyCounter()
    {
    }



  internal FrequencyCounter( MainForm UseForm, string UseFileName )
    {
    MForm = UseForm;

    WordsDictionary = new SortedDictionary<string, int>();
    CharacterDictionary = new SortedDictionary<char, long>();

    FileName = MForm.GetDataDirectory() + UseFileName;
    }



  internal void ClearAll()
    {
    WordsDictionary.Clear();
    }



  internal void AddCharacter( char ToAdd )
    {
    if( CharacterDictionary.ContainsKey( ToAdd ))
      {
      CharacterDictionary[ToAdd] = CharacterDictionary[ToAdd] + 1;
      }
    else
      {
      CharacterDictionary[ToAdd] = 1;
      }
    }



  internal void AddString( string InString )
    {
    if( InString.Length < 3 )
      return;


    for( int Count = 0; Count < InString.Length; Count++ )
      AddCharacter( InString[Count] );

    // Don't do this ToLower().  Compression can't put
    // it back just like it was.
    // InString = InString.ToLower();
    if( WordsDictionary.ContainsKey( InString ))
      {
      WordsDictionary[InString] = WordsDictionary[InString] + 1;
      }
    else
      {
      WordsDictionary[InString] = 1;
      }
    }



  private void MakeSortedArray( int MinFrequency )
    {
    try
    {
    int ArrayLength = WordsDictionary.Count;
    StringRecArray = new StringRec[ArrayLength];
    SortIndexArray = new int[ArrayLength];

    StringRecArrayLast = 0;
    foreach( KeyValuePair<string, int> Kvp in WordsDictionary )
      {
      StringRec Rec = new StringRec();
      Rec.Word = Kvp.Key;
      Rec.Count = Kvp.Value;

      if( Rec.Count < MinFrequency )
        continue;

      StringRecArray[StringRecArrayLast] = Rec;
      SortIndexArray[StringRecArrayLast] = StringRecArrayLast;
      StringRecArrayLast++;
      }

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in MakeSortedArray()." );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal void SortByCount( int MinFrequency )
    {
    if( WordsDictionary.Count < 2 )
      return;

    MakeSortedArray( MinFrequency );

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

      for( int Count = 0; Count < (StringRecArrayLast - 1); Count++ )
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
    if( StringRecArray == null )
      return;

    if( WordsDictionary.Count < 2 )
      return;

    for( int Count = 0; Count < StringRecArrayLast; Count++ )
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




  internal void ShowCharacters()
    {
    int Count = 132;
    foreach( KeyValuePair<char, long> Kvp in CharacterDictionary )
      {
      // if( Kvp.Value < 50 )
        // continue;

      if( MForm.CharIndex.GetCharacterIndex( Kvp.Key ) >= 0 )
        continue;

      // I'm using this to write code for the array.
      int Hex = Kvp.Key;
      MForm.ShowStatus( "    CharacterArray[" + Count.ToString() + "] = '" + Char.ToString( Kvp.Key ) + "'; // " + Kvp.Value.ToString( "N0" ) + "    0x" + Hex.ToString( "X4" ) );
      Count++;
      }

    MForm.ShowStatus( " " );
    MForm.ShowStatus( " " );
    MForm.ShowStatus( " " );
    MForm.ShowStatus( " " );
    MForm.ShowStatus( " " );

    Count = 132;
    foreach( KeyValuePair<char, long> Kvp in CharacterDictionary )
      {
      // if( Kvp.Value < 50 )
        // continue;

      if( MForm.CharIndex.GetCharacterIndex( Kvp.Key ) >= 0 )
        continue;

      MForm.ShowStatus( "      case (int)'" + Char.ToString( Kvp.Key ) + "': return " + Count.ToString() + ";" );
      Count++;
      }
    }



  internal bool WriteToTextFile()
    {
    try
    {
    if( StringRecArray == null )
      return false;

    using( StreamWriter SWriter = new StreamWriter( FileName, false, Encoding.UTF8 ))
      {
      for( int Count = 0; Count < StringRecArrayLast; Count++ )
        {
        int WordCount = StringRecArray[SortIndexArray[Count]].Count;
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
