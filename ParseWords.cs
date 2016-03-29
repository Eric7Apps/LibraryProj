// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
using System.Collections.Generic;
using System.Text;
// using System.Threading.Tasks;


namespace DGOLibrary
{
  class ParseWords
  {


  internal ParseWords()
    {

    }


  internal SortedDictionary<string, int> ParseText( string InString )
    {
    InString = InString.ToLower();
    InString = CleanAndSimplify.SimplifyCharacterCodes( InString );

    // if( InString.Contains( "durangoherald" ))
      // InString = InString.Replace( "durangoherald", " " );

    InString = ReplaceForSplitWords( InString );
    InString = InString.Trim();

    InString = InString.Replace( "capt.", "captain" );

    // "Durango's "
    InString = InString.Replace( "'s ", " " );
    InString = InString.Replace( "'", " " );

    InString = InString.Replace( "\r", " " );
    InString = InString.Replace( "\"", " " );
    InString = InString.Replace( "'", " " );
    InString = InString.Replace( ":", " " );
    InString = InString.Replace( ";", " " );
    InString = InString.Replace( ".", " " );
    InString = InString.Replace( ",", " " );
    InString = InString.Replace( "-", " " );
    InString = InString.Replace( "_", " " );
    InString = InString.Replace( "!", " " );
    InString = InString.Replace( "?", " " );
    InString = InString.Replace( "(", " " );
    InString = InString.Replace( ")", " " );
    InString = InString.Replace( "[", " " );
    InString = InString.Replace( "]", " " );
    InString = InString.Replace( "{", " " );
    InString = InString.Replace( "}", " " );
    InString = InString.Replace( "<", " " );
    InString = InString.Replace( ">", " " );
    InString = InString.Replace( "|", " " );
    InString = InString.Replace( "\\", " " );
    InString = InString.Replace( "/", " " );

    // InString = InString.Replace( "=", " " );

    SortedDictionary<string, int> WordsDictionary = new SortedDictionary<string, int>();

    string[] WordsArray = InString.Split( new Char[] { ' ' } );
    for( int Count = 0; Count < WordsArray.Length; Count++ )
      {
      string Word = WordsArray[Count].Trim();
      if( Word.Length < 3 )
        continue;

      if( Word == "the" )
        continue;

      WordsDictionary[Word] = 1;
      }

    return WordsDictionary;
    }




  internal string ReplaceForSplitWords( string InString )
    {
    // If you were searching for 'book' you'd find
    // 'book' but not 'bookstore' or 'bookshop'.

    string Result = InString;

    // Put these in a file?

    Result = Result.Replace( "bookstore", "book store" );
    Result = Result.Replace( "bookshop", "book shop" );
    Result = Result.Replace( "colostate", "colorado state" );
    Result = Result.Replace( "puertorico", "puerto rico" );
    Result = Result.Replace( "ragtimefestival", "ragtime festival" );
    Result = Result.Replace( "realestate", "real estate" );
    Result = Result.Replace( "realproperty", "real property" );
    Result = Result.Replace( "runningclub", "running club" );
    Result = Result.Replace( "worksite", "work site" );

    return Result;
    }



  }
}
