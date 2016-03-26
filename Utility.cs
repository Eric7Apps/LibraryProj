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
    // This was done in ReadFromTextFile().
    //  Don't go higher than D800 (Surrogates).
    //  if( ToCheck >= 0xD800 )

    // Result = Result.Replace( "&amp;", "&" );

    string Result = InString;
    Result = Result.Replace( "&#x2018;", "'" );
    Result = Result.Replace( "&#x2019;", "'" );
    Result = Result.Replace( "&#x2013;", " " ); // A weird symbol.
    Result = Result.Replace( "&#x2026;", "..." );


    Result = Result.Replace( "&#x201c;", "\"" );
    Result = Result.Replace( "&#x201d;", "\"" );

    Result = Result.Replace( "&#xad;", "" ); // hyphen for word wrap I think.

    // UTF8 bytes start with 0 for ASCII, or
    // 10 for a continuing byte, or
    // 110, 1110 for starting bytes.
    //  7 	U+007F 	0xxxxxxx
    // 11 	U+07FF 	110xxxxx 	10xxxxxx
    // 16 	U+FFFF 	1110xxxx 	10xxxxxx 	10xxxxxx

    // So character 0xA0 would be represented with two
    // bytes in UTF8.

/*
A1) ¡
A2) ¢
A3) £
A4) ¤
A5) ¥
A6) ¦
A7) §
A8) ¨
A9) ©
AA) ª
AB) «
AC) ¬
AD) ­
AE) ®
AF) ¯
B0) °
B1) ±
B2) ²
B3) ³
B4) ´
B5) µ
B6) ¶
B7) ·
B8) ¸
B9) ¹
BA) º
BB) »
BC) ¼
BD) ½
BE) ¾
BF) ¿
C0) À
C1) Á
C2) Â
C3) Ã
C4) Ä
C5) Å
C6) Æ
C7) Ç
C8) È
C9) É
CA) Ê
CB) Ë
CC) Ì
CD) Í
CE) Î
CF) Ï
D0) Ð
D1) Ñ
D2) Ò
D3) Ó
D4) Ô
D5) Õ
D6) Ö
D7) ×
D8) Ø
D9) Ù
DA) Ú
DB) Û
DC) Ü
DD) Ý
DE) Þ
DF) ß
*/

    // à, á, â, ã, ä, å, æ
    Result = Result.Replace( "&#xe0;", "a" );
    Result = Result.Replace( "&#xe1;", "a" );
    Result = Result.Replace( "&#xe2;", "a" );
    Result = Result.Replace( "&#xe3;", "a" );
    Result = Result.Replace( "&#xe4;", "a" );
    Result = Result.Replace( "&#xe5;", "a" );
    Result = Result.Replace( "&#xe6;", "ae" );

    // ç
    Result = Result.Replace( "&#xe7;", "c" );

    // è, é, ê, ë
    Result = Result.Replace( "&#xe8;", "e" );
    Result = Result.Replace( "&#xe9;", "e" );
    Result = Result.Replace( "&#xea;", "e" );
    Result = Result.Replace( "&#xeb;", "e" );

    // ì, í, î, ï, 
    Result = Result.Replace( "&#xec;", "i" );
    Result = Result.Replace( "&#xed;", "i" );
    Result = Result.Replace( "&#xee;", "i" );
    Result = Result.Replace( "&#xef;", "i" );

    // ð
    Result = Result.Replace( "&#xf0;", "o" );

    // ñ
    Result = Result.Replace( "&#xf1;", "n" );

    // ò, ó, ô, õ, ö
    Result = Result.Replace( "&#xf2;", "o" );
    Result = Result.Replace( "&#xf3;", "o" );
    Result = Result.Replace( "&#xf4;", "o" );
    Result = Result.Replace( "&#xf5;", "o" );
    Result = Result.Replace( "&#xf6;", "o" );

    // F7) ÷
    // F8) ø

    // ù, ú, û, ü

    Result = Result.Replace( "&#xf9;", "u" );
    Result = Result.Replace( "&#xfa;", "u" );
    Result = Result.Replace( "&#xfb;", "u" );
    Result = Result.Replace( "&#xfc;", "u" );

    // ý, ÿ
    // FE) þ
    Result = Result.Replace( "&#xfd;", "y" );
    Result = Result.Replace( "&#xff;", "y" );

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


    return Result;
    }




  }
}

