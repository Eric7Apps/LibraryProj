// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com



using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DGOLibrary
{
  static class CleanAndSimplify
  {

  internal static string SimplifyCharacterCodes( string InString )
    {
    // HTML character codes
    // http://www.w3schools.com/html/html_symbols.asp

    // This was done in ReadFromTextFile().
    //  Don't go higher than 0xD800 (Surrogates).

    // Don't do this with a URL.
    // This link has semi-colons and everything:
    // http://www.durangoherald.com/article/20160317/ARTS01/160319611/0/Arts/&#x2018;Legally-Blonde-the-Musical&#x2019;-better-than-the-book-or-movie

    string Result = InString;

    // Replace some Unicode characters with ASCII.
    Result = Result.Replace( "”", "\"" );
    Result = Result.Replace( "’", "'" );

    Result = Result.Replace( "&#x2013;", " " ); // A weird symbol.
    Result = Result.Replace( "&#x2018;", "'" );
    Result = Result.Replace( "&#x2019;", "'" );
    Result = Result.Replace( "&#x201c;", "\"" );
    Result = Result.Replace( "&#x201d;", "\"" );
    Result = Result.Replace( "&#x2026;", "..." );

    Result = Result.Replace( "&#10050;", " " ); // Some kind of circular character for a bullet list.

    // 39 is ASCII apostrophe.
    Result = Result.Replace( "&#39;", "'" );

    Result = Result.Replace( "&#xad;", "" ); // hyphen for word wrap I think.

    // Simplify characters for words dicionary.
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


    Result = Result.Replace( "&nbsp;", " " );
    Result = Result.Replace( "&quot;", "\"" );
    Result = Result.Replace( "&apos;", "'" );
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
    Result = Result.Replace( "&shy;", "" ); // hyphen for word wrap?
    //    As in Resum&eacute;
    Result = Result.Replace( "&eacute;", "e" );

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

    // Do this last.
    Result = Result.Replace( "&amp;", "&" );

    return Result;
    }



  }
}
