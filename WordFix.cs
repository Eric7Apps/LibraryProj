// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
using System.Text;


  /*
    // ReplaceWordsDictionary["jan"] = "january";
    ReplaceWordsDictionary["feb"] = "february";
    // ReplaceWordsDictionary["mar"] = "march";
    // ReplaceWordsDictionary["apr"] = "april";
    // ReplaceWordsDictionary["may"] = "may";
    // ReplaceWordsDictionary["jun"] = "june";
    // ReplaceWordsDictionary["jul"] = "july";
    ReplaceWordsDictionary["aug"] = "august";
    ReplaceWordsDictionary["sept"] = "september";
    ReplaceWordsDictionary["oct"] = "october";
    ReplaceWordsDictionary["nov"] = "november";
    // ReplaceWordsDictionary["dec"] = "december";
    */

namespace DGOLibrary
{
  // Compare this hard-coded stuff with what's in the
  // WordsIndex.cs file.  It would eventually have to
  // be done more like in that file.  But the main
  // FixWord() function is called for _every_ word
  // that gets parsed, so it has to be very efficient.
  static class WordFix
  {


  internal static string FixWord( string Word )
    {
    // It often changes it to the basic root word
    // like: 'slowin' to 'slow', rather than 'slowing'.

    if( Word == null )
      return "";

   if( Word.Length < 3 )
     return "";

    Word = Word.ToLower();

    if( Word[0] == 'a' )
      {
      // if( Word[1] == 'a' )
      return FixWordA( Word );

      }

    if( Word[0] == 'b' )
      {
      return FixWordB( Word );
      }

    if( Word[0] == 'c' )
      {
      return FixWordC( Word );
      }

    if( Word[0] == 'd' )
      {
      return FixWordD( Word );
      }

    if( Word[0] == 'f' )
      {
      return FixWordF( Word );
      }

    if( Word[0] == 'i' )
      {
      return FixWordI( Word );
      }

    if( Word[0] == 'm' )
      {
      return FixWordM( Word );
      }

    if( Word[0] == 'o' )
      {
      return FixWordO( Word );
      }

    if( Word[0] == 'p' )
      {
      return FixWordP( Word );
      }

    if( Word[0] == 'r' )
      {
      return FixWordR( Word );
      }

    if( Word[0] == 's' )
      {
      return FixWordS( Word );
      }

    if( Word[0] == 't' )
      {
      return FixWordT( Word );
      }
    if( Word[0] == 'w' )
      {
      return FixWordW( Word );
      }

    return Word;
    }

    // For N:
    // nutrition
    // nutritional
    // nutritionist
    // nutritionists



  internal static string FixWordA( string Word )
    {
    if( Word == "ariz" )
      return "arizona";

    return Word;
    }



  internal static string FixWordB( string Word )
    {
    if( Word == "beyonc" )
      return "beyoncé";

    if( Word == "broomfiled" )
      return "broomfield";

    if( Word == "brucoli" )
      return "brocoli";

    return Word;
    }


  internal static string FixWordC( string Word )
    {
    if( Word == "calif" )
      return "california";

    if( Word == "celerbate" )
      return "celebrate";

    if( Word == "celerbating" )
      return "celebrating";

    if( Word == "chidlren" )
      return "children";

    if( Word == "childen" )
      return "children";

    if( Word == "cinderela" )
      return "cinderella";

    if( Word == "colo" )
      return "colorado";

    if( Word == "conn" )
      return "connecticut";

    return Word;
    }



  internal static string FixWordD( string Word )
    {
    if( Word == "dicapio" )
      return "dicaprio";

    if( Word == "downton" )
      return "downtown";

    return Word;
    }


  internal static string FixWordF( string Word )
    {
    if( Word == "fianc" )
      return "fiancé";

    if( Word == "fiance" )
      return "fiancé";

    if( Word == "febuary" )
      return "february";

    if( Word == "fla" )
      return "florida";

    return Word;
    }



  internal static string FixWordI( string Word )
    {
    if( Word == "inacio" )
      return "ignacio";

    if( Word == "intro" )
      return "introduction";

    return Word;
    }


  internal static string FixWordM( string Word )
    {
    if( Word == "mangement" )
      return "management";

    if( Word == "margaritis" )
      return "margaritas";

    if( Word == "memorablia" )
      return "memorabilia";

    if( Word == "moeny" )
      return "money";

    return Word;
    }



  internal static string FixWordO( string Word )
    {
    if( Word == "outsude" )
      return "outside";

    return Word;
    }



  internal static string FixWordP( string Word )
    {
    if( Word == "participaes" )
      return "participate";

    if( Word == "partnerhip" )
      return "partnership";

    // persevere
    // perseverate
    if( Word == "perservation" )
      return "preservation";

    return Word;
    }



  internal static string FixWordR( string Word )
    {
    if( Word == "risqu" )
      return "risqué";

    if( Word == "resum" )
      return "resumé";

    if( Word == "recidivisim" )
      return "recidivism";

    return Word;
    }


  internal static string FixWordS( string Word )
    {
    if( Word == "slowin" )
      return "slow";

    if( Word == "sydrome" )
      return "syndrome";

    return Word;
    }


  internal static string FixWordT( string Word )
    {
    if( Word == "tardition" )
      return "tradition";

    if( Word == "thinkin" )
      return "think";

    if( Word == "througout" )
      return "throughout";

    return Word;
    }


  internal static string FixWordW( string Word )
    {
    if( Word == "wearin" )
      return "wear";

    return Word;
    }



  internal static string FixAbbreviations( string InString )
    {
    string Result = InString;

    Result = Result.Replace( "capt.", "captain" );

    return Result;
    }



  internal static string ReplaceForSplitWords( string InString )
    {
    // If you were searching for 'book' you'd find
    // 'book' but not 'bookstore' or 'bookshop'.

    string Result = InString;

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