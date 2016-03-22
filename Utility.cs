// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DGOLibrary
{
  static class Utility
  {

  internal static string CleanAsciiString( string InString, int MaxLength )
    {
    if( InString == null )
      return "";

    StringBuilder SBuilder = new StringBuilder();
    
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      if( Count >= MaxLength )
        break;

      if( InString[Count] > 127 )
        continue; // Don't want this character.

      if( InString[Count] < ' ' )
        continue; // Space is lowest ASCII character.

      SBuilder.Append( Char.ToString( InString[Count] ) );
      }

    string Result = SBuilder.ToString();
    // Result = Result.Replace( "\"", "" );
    return Result;
    }



  internal static string GetCleanUnicodeString( string InString, int HowLong )
    {
    if( InString == null )
      return "";

    if( InString.Length > HowLong )
      InString = InString.Remove( HowLong );

    StringBuilder SBuilder = new StringBuilder();
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      char ToCheck = InString[Count];

      if( ToCheck < ' ' )
        continue; // Don't want this character.

      //  Don't go higher than D800 (Surrogates).
      if( ToCheck >= 0xD800 )
        continue;

      SBuilder.Append( Char.ToString( ToCheck ));
      }

    return SBuilder.ToString().Trim();
    }


  internal static string TruncateString( string InString, int HowLong )
    {
    if( InString.Length <= HowLong )
      return InString;

    return InString.Remove( HowLong );
    }



  // You could use Base64 instead.
  internal static string BytesToLetterString( byte[] InBytes )
    {
    StringBuilder SBuilder = new StringBuilder();
    for( int Count = 0; Count < InBytes.Length; Count++ )
      {
      uint ByteHigh = InBytes[Count];
      uint ByteLow = ByteHigh & 0x0F;
      ByteHigh >>= 4;
      SBuilder.Append( (char)('A' + (char)ByteHigh) );
      SBuilder.Append( (char)('A' + (char)ByteLow) );
      // MForm.ShowStatus( SBuilder.ToString() );
      }

    return SBuilder.ToString();
    }



  private static bool IsInLetterRange( uint Letter )
    {
    const uint MaxLetter = (uint)('A') + 15;
    const uint MinLetter = (uint)'A';

    if( Letter > MaxLetter )
      {
      // MForm.ShowStatus( "Letter > MaxLetter" );
      return false;
      }

    if( Letter < MinLetter )
      {
      // MForm.ShowStatus( "Letter < MinLetter" );
      return false;
      }

    return true;
    }



  internal static byte[] LetterStringToBytes( string InString )
    {
    try
    {

    if( InString == null )
      return null;

    if( InString.Length < 2 )
      return null;

    byte[] OutBytes;

    try
    {
    OutBytes = new byte[InString.Length >> 1];
    }
    catch( Exception )
      {
      return null;
      }

    int Where = 0;
    for( int Count = 0; Count < OutBytes.Length; Count++ )
      {
      uint Letter = InString[Where];
      if( !IsInLetterRange( Letter ))
        return null;

      uint ByteHigh = Letter - (uint)'A';
      ByteHigh <<= 4;
      Where++;
      Letter = InString[Where];
      if( !IsInLetterRange( Letter ))
        return null;

      uint ByteLow = Letter - (uint)'A';
      Where++;

      OutBytes[Count] = (byte)(ByteHigh | ByteLow);
      }

    return OutBytes;

    }
    catch( Exception )
      {
      return null;
      }
    }



  internal static string RemoveFromStartToEnd( string Start, string End, string InString )
    {
    try
    {
    // Regular expressions?

    if( Start.Length > 100 )
      return "Start length can't be more than 100.";

    if( End.Length > 100 )
      return "End length can't be more than 100.";

    char[] StartBuf = new char[100];
    char[] EndBuf = new char[100];

    int StartIndex = 0;
    int EndIndex = 0;
    StringBuilder SBuilder = new StringBuilder();
    bool IsInside = false;
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      if( !IsInside )
        SBuilder.Append( InString[Count] );

      if( !IsInside )
        {
        // ToLower() so it matches something like <ScRipT
        StartBuf[StartIndex] = Char.ToLower( InString[Count] );
        if( StartBuf[StartIndex] == Start[StartIndex] )
          {
          StartIndex++;
          }
        else
          {
          // No match, so start at zero again.
          StartIndex = 0;
          StartBuf[StartIndex] = Char.ToLower( InString[Count] );
          // Is it already at the beginning of a match here?
          // pppppattern
          if( StartBuf[StartIndex] == Start[StartIndex] )
            StartIndex++;

          }

        if( StartIndex == Start.Length )
          {
          // Remove the start pattern.
          int TruncateToLength = SBuilder.Length - Start.Length;
          // if( TruncateToLength >= 0 )
          SBuilder.Length = TruncateToLength;

          IsInside = true;
          EndIndex = 0;
          }
        }

      // This is not the "else" from above because
      // it might have changed above.
      if( IsInside )
        {
        EndBuf[EndIndex] = Char.ToLower( InString[Count] );
        if( EndBuf[EndIndex] == End[EndIndex] )
          {
          EndIndex++;
          }
        else
          {
          EndIndex = 0;
          EndBuf[EndIndex] = Char.ToLower( InString[Count] );
          if( EndBuf[EndIndex] == End[EndIndex] )
            EndIndex++;

          }

        if( EndIndex == End.Length )
          {
          IsInside = false;
          StartIndex = 0;
          }
        }
      }

    return SBuilder.ToString();

    }
    catch( Exception Except )
      {
      return "Exception in Utilit.RemoveFromStartToEnd().\r\n" +
        Except.Message;

      }
    }



  internal static string  SimplifyAndCleanCharacters( string InString )
    {
    string Result = InString;
    Result = Result.Replace( "&#x2018;", "'" );
    Result = Result.Replace( "&#x2019;", "'" );
    // Result = Result.Replace( "&amp;", "&" );
    Result = Result.Replace( "&#x2013", " " ); // A weird symbol.

    Result = Result.Replace( "&laquo;", "\"" );
    Result = Result.Replace( "&raquo;", "\"" );
    Result = Result.Replace( "&lsaquo;", "'" );
    Result = Result.Replace( "&rsaquo;", "'" );
    Result = Result.Replace( "&ldquo;", "\"" );
    Result = Result.Replace( "&rdquo;", "\"" );
    Result = Result.Replace( "&lsquo;", "'" );
    Result = Result.Replace( "&rsquo;", "'" );
    Result = Result.Replace( "&ndash;", "-" );
    Result = Result.Replace( "&mdash;", "-" );

    // HTML character codes
    // http://www.w3schools.com/html/html_symbols.asp

    // This was done in ReadFromTextFile().
    //  Don't go higher than D800 (Surrogates).
    //  if( ToCheck >= 0xD800 )
 
    return Result;
    }




  }
}

