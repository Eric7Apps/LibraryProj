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



  internal static string GetCleanUnicodeString( string InString, int HowLong, bool TrimIt )
    {
    if( InString == null )
      return "";

    // This is the maximum length before it's cleaned up.
    // But that's about the same as the resulting length
    // if it's already clean.  (Minus tabs, CR, LF.)
    // It is normally just a reasonable maximum limit
    // for user input.
    if( InString.Length > HowLong )
      InString = InString.Remove( HowLong );

    StringBuilder SBuilder = new StringBuilder();
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      char ToCheck = InString[Count];

      // This removes tabs and CR and LF.
      if( ToCheck < ' ' )
        continue;

      //  Don't go higher than D800 (Surrogates).
      if( ToCheck >= 0xD800 )
        continue;

      SBuilder.Append( Char.ToString( ToCheck ));
      }

    string Result = SBuilder.ToString();
    if( TrimIt )
      Result = Result.Trim();

    return Result;
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



  internal static bool MatchesTestString( int Where, string InString, string MatchS )
    {
    try
    {
    int MatchLength = MatchS.Length;
    if( InString.Length < MatchLength )
      return false;

    for( int Count = 0; Count < MatchLength; Count++ )
      {
      if( (Where + Count) >= InString.Length )
        return false;

      // ToLower() so it matches something like <ScRipT
      if( Char.ToLower( InString[Where + Count] ) != MatchS[Count] )
        return false;

      }

    return true;
    }
    catch( Exception Except )
      {
      string ShowS = "Exception in Utility.MatchesTestString().\r\n" +
        Except.Message;

      throw( new Exception( ShowS ));
      }
    }



  internal static string RemovePatternFromStartToEnd( string StartS, string EndS, string InString )
    {
    try
    {
    StringBuilder SBuilder = new StringBuilder();
    bool IsInside = false;
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      // When this is looking for <span at the beginning
      // and > at the end, it can match both at the
      // same time with the ><span pattern.
      // So put if( IsInside ) first.

      if( IsInside )
        {                  //      /script>
        if( MatchesTestString( Count - EndS.Length, InString, EndS ))
          IsInside = false;

        }

      if( !IsInside )
        {
        if( MatchesTestString( Count, InString, StartS ))
          IsInside = true;

        }

      if( !IsInside )
        SBuilder.Append( InString[Count] );

      }

    return SBuilder.ToString();

    }
    catch( Exception Except )
      {
      return "Exception in Utility.RemovePatternFromStartToEnd().\r\n" +
        Except.Message;

      }
    }



  internal static SortedDictionary<string, int> GetPatternsFromStartToEnd( string StartS, string EndS, string InString )
    {
    try
    {
    SortedDictionary<string, int> LinesDictionary = new SortedDictionary<string, int>();

    StringBuilder SBuilder = new StringBuilder();
    bool IsInside = false;
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      // Put if( IsInside ) first.

      if( IsInside )
        {                  //      /script>
        if( MatchesTestString( Count - EndS.Length, InString, EndS ))
          {
          IsInside = false;
          string Line = SBuilder.ToString().Trim().ToLower();
          SBuilder.Clear();
          LinesDictionary[Line] = 1;
          }
        }

      if( !IsInside )
        {
        if( MatchesTestString( Count, InString, StartS ))
          IsInside = true;

        }

      if( IsInside )
        SBuilder.Append( InString[Count] );

      }

    return LinesDictionary;

    }
    catch( Exception ) // Except )
      {
      return null;
      }
    }


  }
}

