// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
// using System.Collections.Generic;
using System.Text;
// using System.Threading.Tasks;


// This has a ways to go.

namespace DGOLibrary
{
  class WordsIndex
  {
  private WordsData CallingWordsData;
  private WordsArrayRec[,,,] WordsArray;
  private const int LetterAOffset = (int)'a';
  private const int WordsArraySize = 28;

  private int NextIndex = 1;


  public struct WordRec
    {
    public string WordTail;
    public IntegerCollection IndexCol;
    public int Index; // For compression.
    public int HowMany;
    }

  public struct WordsArrayRec
    {
    public WordRec[] WordRecArray;
    public int WordRecArrayLast;
    }


  private WordsIndex()
    {
    }



  internal WordsIndex( WordsData UseWordsData )
    {
    CallingWordsData = UseWordsData;
    WordsArray = new WordsArrayRec[WordsArraySize, WordsArraySize, WordsArraySize, WordsArraySize];
    }


  private void ShowStatus( string InString )
    {
    if( CallingWordsData == null )
      return;

    CallingWordsData.ShowStatus( InString );
    }


  private int GetLetterIndex( char Letter )
    {
    // Everything is lower case.
    // Almost all letters are ASCII letters between
    // 'a' and 'z'.
    // z - a = 25
    // a - a = 0
    if( ('z' - 'a') > 25 )
      throw( new Exception( "This can't happen." ));


    if( Letter == ' ' )
      return 26;

    // Check WordsArraySize.
    if( (Letter < 'a') || (Letter > 'z'))
      return 27; // Put all non-ASCII characters here.

    return (int)Letter - LetterAOffset;
    }



  private string FixupWord( string Word )
    {
    if( Word == null )
      return "";

    Word = Word.Trim().ToLower();
    int WLength = Word.Length;

    if( WLength < 3 )
      return "";

    // Make it at least five characters long.

    if( WLength == 3 )
      return Word + "  ";

    if( WLength == 4 )
      return Word + " ";

    return Word;
    }


  private string RemoveHead( string InString )
    {
    // This assumes the string is at least 5
    // characters long.
    return InString.Remove( 0, 4 );
    }


  internal bool WordExists( string Word )
    {
    try
    {
    Word = FixupWord( Word );
    if( Word.Length == 0 )
      return false;

    int Index0 = GetLetterIndex( Word[0] );
    int Index1 = GetLetterIndex( Word[1] );
    int Index2 = GetLetterIndex( Word[2] );
    int Index3 = GetLetterIndex( Word[3] );

    if( null == WordsArray[Index0, Index1, Index2, Index3].WordRecArray )
      return false;

    int Last = WordsArray[Index0, Index1, Index2, Index3].WordRecArrayLast;

    string Tail = Word;
    if( !( (Index0 > 26) || // Non-ASCII is 27.
           (Index1 > 26) ||
           (Index2 > 26) ||
           (Index3 > 26)))
      {
      Tail = RemoveHead( Word );
      }

    // Sorted for a binary search?
    for( int Count = 0; Count < Last; Count++ )
      {
      if( Tail == WordsArray[Index0, Index1, Index2, Index3].WordRecArray[Count].WordTail )
        return true;

      }

    return false;
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in WordsIndex.WordExists():" );
      ShowStatus( Except.Message );
      return false;
      }
    }



  internal void AddWord( string Word )
    {
    try
    {
    Word = FixupWord( Word );
    if( Word.Length == 0 )
      return;

    if( WordExists( Word ))
      return;

    int Index0 = GetLetterIndex( Word[0] );
    int Index1 = GetLetterIndex( Word[1] );
    int Index2 = GetLetterIndex( Word[2] );
    int Index3 = GetLetterIndex( Word[3] );

    if( null == WordsArray[Index0, Index1, Index2, Index3].WordRecArray )
      WordsArray[Index0, Index1, Index2, Index3].WordRecArray = new WordRec[8];

    string Tail = Word;
    if( !( (Index0 > 26) || // Non-ASCII is 27.
           (Index1 > 26) ||
           (Index2 > 26) ||
           (Index3 > 26)))
      {
      Tail = RemoveHead( Word );
      }

    WordRec Rec = new WordRec();
    Rec.WordTail = Tail;
    Rec.IndexCol = null; // IntegerCollection is empty.
    Rec.HowMany = 1;
    Rec.Index = NextIndex;
    NextIndex++;

    WordsArray[Index0, Index1, Index2, Index3].WordRecArray[WordsArray[Index0, Index1, Index2, Index3].WordRecArrayLast] = Rec;
    WordsArray[Index0, Index1, Index2, Index3].WordRecArrayLast++;
    if( WordsArray[Index0, Index1, Index2, Index3].WordRecArrayLast >= WordsArray[Index0, Index1, Index2, Index3].WordRecArray.Length )
      {
      Array.Resize( ref WordsArray[Index0, Index1, Index2, Index3].WordRecArray, WordsArray[Index0, Index1, Index2, Index3].WordRecArray.Length + 64 );
      }
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in WordsIndex.AddWord():" );
      ShowStatus( Except.Message );
      }
    }



  /*
  internal string GetAllWordsString()
    {
    try
    {
    // Sort each array.

    // Non-ASCII is 27.
    for( int Count0 = 0; Count0 < WordsArraySize; Count0++ )
      {
      for( int Count1 = 0; Count1 < WordsArraySize; Count1++ )
        {
        for( int Count2 = 0; Count2 < WordsArraySize; Count2++ )
          {
          for( int Count3 = 0; Count3 < WordsArraySize; Count3++ )
            {
            if( null == WordsArray[Count0, Count1, Count2, Count3].WordRecArray )
              continue;

            int Last = WordsArray[Count0, Count1, Count2, Count3].WordRecArrayLast;

    string Tail = Word;
    if( !( (Index0 > 26) || // Non-ASCII is 27.
           (Index1 > 26) ||
           (Index2 > 26) ||
           (Index3 > 26)))
      {
      Tail = RemoveHead( Word );
      }

    // Sorted for a binary search?
    for( int Count = 0; Count < Last; Count++ )
      {
      if( Tail == WordsArray[Index0, Index1, Index2, Index3].WordRecArray[Count].WordTail )
        return true;

      }

    return false;
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in WordsIndex.WordExists():" );
      ShowStatus( Except.Message );
      return false;
      }
    }
    */


  }
}

