// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
using System.Text;



namespace DGOLibrary
{
  class WordsIndex
  {
  private WordsData CallingWordsData;
  private WordsArrayRec[,,,] WordsArray;
  private const int LetterAOffset = (int)'a' - 1; // See notes below.
  private const int WordsArraySize = 28;

  // private int NextIndex = 1;


  public struct WordRec
    {
    public string WordTail;
    public IntegerCollection IndexCol;
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
    // The space character is at zero, so 
    if( ((int)'a' - LetterAOffset) != 1 )
      throw( new Exception( "This can't happen with LetterAOffset." ));

    if( Letter == ' ' )
      return 0; // This way it puts it in sorted order.

    // Check WordsArraySize.
    if( (Letter < 'a') || (Letter > 'z'))
      return 27; // Put all non-ASCII characters here.

    return (int)Letter - LetterAOffset;
    }



  private char GetLetterFromIndex( int Index )
    {
    if( Index < 0 )
      return '?'; 

   if( Index >= 28 )
      return '?'; 

    if( Index == 0 )
      return ' ';

    return (char)(Index + LetterAOffset);
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

    if( Word == "bartók" )
      ShowStatus( "WordsIndex. bartók is being added." );

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
    // Rec.Index = NextIndex;
    // NextIndex++;

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



  internal IntegerCollection GetIntegerCollection( string WordToMatch )
    {
    try
    {
    WordToMatch = FixupWord( WordToMatch );
    if( WordToMatch.Length == 0 )
      return null;

    int Index0 = GetLetterIndex( WordToMatch[0] );
    int Index1 = GetLetterIndex( WordToMatch[1] );
    int Index2 = GetLetterIndex( WordToMatch[2] );
    int Index3 = GetLetterIndex( WordToMatch[3] );

    int Last = WordsArray[Index0, Index1, Index2, Index3].WordRecArrayLast;
    for( int Count = 0; Count < Last; Count++ )
      {
      string CheckWord = WordsArray[Index0, Index1, Index2, Index3].WordRecArray[Count].WordTail;
      if( !( (Index0 > 26) || // Non-ASCII is 27.
             (Index1 > 26) ||
             (Index2 > 26) ||
             (Index3 > 26)))
        {
        CheckWord = Char.ToString( GetLetterFromIndex( Index0 )) +
               Char.ToString( GetLetterFromIndex( Index1 )) +
               Char.ToString( GetLetterFromIndex( Index2 )) +
               Char.ToString( GetLetterFromIndex( Index3 )) +
               CheckWord;

        }

      if( CheckWord.Trim() == WordToMatch )
        {
        if( null == WordsArray[Index0, Index1, Index2, Index3].WordRecArray[Count].IndexCol )
          {
          WordsArray[Index0, Index1, Index2, Index3].WordRecArray[Count].IndexCol = new IntegerCollection();
          }

        return WordsArray[Index0, Index1, Index2, Index3].WordRecArray[Count].IndexCol;
        }
      }

    return null;
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in WordsIndex.GetIntegerCollection():" );
      ShowStatus( Except.Message );
      return null;
      }
    }



  internal void AddPageIndex( string Word, int PageIndex )
    {
    try
    {
    Word = FixupWord( Word );
    if( Word.Length == 0 )
      return;

    if( !WordExists( Word ))
      AddWord( Word );

    // This will make a new one if need be.
    IntegerCollection IntCol = GetIntegerCollection( Word );
    if( IntCol == null )
      return; // There was an error.


    IntCol.AddInteger( PageIndex, true );

    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in WordsIndex.AddPageIndex():" );
      ShowStatus( Except.Message );
      }
    }



  internal string GetAllWordsString()
    {
    try
    {
    // Sort each array.

    StringBuilder SBuilder = new StringBuilder();
    for( int Index0 = 0; Index0 < WordsArraySize; Index0++ )
      {
      for( int Index1 = 0; Index1 < WordsArraySize; Index1++ )
        {
        for( int Index2 = 0; Index2 < WordsArraySize; Index2++ )
          {
          for( int Index3 = 0; Index3 < WordsArraySize; Index3++ )
            {
            if( null == WordsArray[Index0, Index1, Index2, Index3].WordRecArray )
              continue;

            int Last = WordsArray[Index0, Index1, Index2, Index3].WordRecArrayLast;
            for( int Count = 0; Count < Last; Count++ )
              {
              string Word = WordsArray[Index0, Index1, Index2, Index3].WordRecArray[Count].WordTail;
              if( !( (Index0 > 26) || // Non-ASCII is 27.
                     (Index1 > 26) ||
                     (Index2 > 26) ||
                     (Index3 > 26)))
                {
                Word = Char.ToString( GetLetterFromIndex( Index0 )) +
                       Char.ToString( GetLetterFromIndex( Index1 )) +
                       Char.ToString( GetLetterFromIndex( Index2 )) +
                       Char.ToString( GetLetterFromIndex( Index3 )) +
                       Word;

                }

              IntegerCollection IntCol = WordsArray[Index0, Index1, Index2, Index3].WordRecArray[Count].IndexCol;
              string Line = "";
              if( IntCol == null )
                {
                Line = Word.Trim() + "\t \r";
                }
              else
                {
                Line = Word.Trim() + "\t" +
                  IntCol.ObjectToString() + "\r";

                }

              SBuilder.Append( Line );
              }
            }
          }
        }
      }

    return SBuilder.ToString();
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in WordsIndex.GetAllWordsString():" );
      ShowStatus( Except.Message );
      return "";
      }
    }



  internal void ClearAllIntCollections()
    {
    try
    {
    for( int Index0 = 0; Index0 < WordsArraySize; Index0++ )
      {
      for( int Index1 = 0; Index1 < WordsArraySize; Index1++ )
        {
        for( int Index2 = 0; Index2 < WordsArraySize; Index2++ )
          {
          for( int Index3 = 0; Index3 < WordsArraySize; Index3++ )
            {
            if( null == WordsArray[Index0, Index1, Index2, Index3].WordRecArray )
              continue;

            int Last = WordsArray[Index0, Index1, Index2, Index3].WordRecArrayLast;
            for( int Count = 0; Count < Last; Count++ )
              WordsArray[Index0, Index1, Index2, Index3].WordRecArray[Count].IndexCol = null;

            }
          }
        }
      }

    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in WordsIndex.ClearAllIntCollections():" );
      ShowStatus( Except.Message );
      }
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
    if( ExcludedWords.IsExcluded( InWord ))
      return "";

    if( WordExists( InWord ) )
      return InWord;

    // It doesn't get here if it's in the main dictionary.
    // InWord = ReplaceCommonWordForms( InWord );
    // if( MainWordsDictionary.ContainsKey( InWord ))
      // return InWord;

    string FixedWord = SuffixForms.FixSuffix( InWord, this );

    if( FixedWord.Length != 0 )
      {
      // MForm.ShowStatus( "To: " + FixedWord );
      return FixedWord;
      }

    ShowStatus( "Word? " + InWord );
    // if( ContainsNonASCII( InWord ))
      // {
      // if( !InWord.Contains( "fiancé" ))
        // MForm.ShowStatus( "File: " + InFile );

      // }

    return "";

    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in GetValidWordForm():" );
      ShowStatus( Except.Message );
      return "";
      }
    }


  }
}

