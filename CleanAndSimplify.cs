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

    //       single quotes for a 'soft' opening
    // A â€˜softâ€™ opening
    // Where do those links come from?

    // http://www.DurangoTelegraph.com/index.cfm/archives/2009/november-26-2009/a-asoftae284a2-opening/


    Result = Result.Replace( "%E2%80%98", "'" );
    Result = Result.Replace( "%E2%80%99", "'" );
    // A â€˜softâ€™ opening
    // Result = Result.Replace( "%E2", "â" );
    // Control character.
    // Result = Result.Replace( "%80", " " );
    // Result = Result.Replace( "%98", " " );
    Result = Result.Replace( "%2526", " " );
    Result = Result.Replace( "%2527", " " );
    Result = Result.Replace( "%252F", " " );
    Result = Result.Replace( "%253D", " " );

    // 0x20 is the space character.
    Result = Result.Replace( "%20", " " );

    Result = Result.Replace( "%2f", "/" );

    // Replace some Unicode characters with ASCII.
    Result = Result.Replace( "”", "\"" );
    Result = Result.Replace( "“", "\"" );
    Result = Result.Replace( "’", "'" );
    Result = Result.Replace( "‘", "'" );

    // A control code.
    Result = Result.Replace( "&#145;", " " );

    // &#0187;
    // This one is very high in the frequency count.
    Result = Result.Replace( "&#0187;", "»" );

    // 160 is a control code.
    Result = Result.Replace( "&#160;", " " );

    Result = Result.Replace( "&#x2013;", "-" );
    Result = Result.Replace( "&#x2014;", "—" );


    Result = Result.Replace( "&#x2018;", "'" );
    Result = Result.Replace( "&#x2019;", "'" );
    Result = Result.Replace( "&#x201c;", "\"" );
    Result = Result.Replace( "&#x201d;", "\"" );
    Result = Result.Replace( "&#x2026;", "..." );

    Result = Result.Replace( "&#8211;", "-" );

    Result = Result.Replace( "&#8216;", "'" );
    Result = Result.Replace( "&#8217;", "'" );
    Result = Result.Replace( "&#8220;", "\"" );
    Result = Result.Replace( "&#8221;", "\"" );
    Result = Result.Replace( "&#8226;", "•" );
    Result = Result.Replace( "&#8230;", "…" );

    Result = Result.Replace( "&#10050;", " " ); // Some kind of circular character for a bullet list.

    // 39 is ASCII apostrophe.
    Result = Result.Replace( "&#39;", "'" );
    Result = Result.Replace( "&#039;", "'" );

    Result = Result.Replace( "&#xa9;", "©" );
    Result = Result.Replace( "&#xad;", " " ); // hyphen for word wrap I think.

    Result = Result.Replace( "&#xbc;", "¼" );
    Result = Result.Replace( "&#xbd;", "½" );

    Result = Result.Replace( "&#xe0;", "à" );
    Result = Result.Replace( "&#xe1;", "á" );
    Result = Result.Replace( "&#xe2;", "â" );
    Result = Result.Replace( "&#xe3;", "ã" );
    Result = Result.Replace( "&#xe4;", "ä" );
    Result = Result.Replace( "&#xe5;", "å" );
    Result = Result.Replace( "&#xe6;", "æ" );

    Result = Result.Replace( "&#xe7;", "ç" );

    Result = Result.Replace( "&#xe8;", "è" );
    Result = Result.Replace( "&#xe9;", "é" );
    Result = Result.Replace( "&#xea;", "ê" );
    Result = Result.Replace( "&#xeb;", "ë" );

    Result = Result.Replace( "&#xec;", "ì" );
    Result = Result.Replace( "&#xed;", "í" );
    Result = Result.Replace( "&#xee;", "î" );
    Result = Result.Replace( "&#xef;", "ï" );

    Result = Result.Replace( "&#xf0;", "ð" );

    Result = Result.Replace( "&#xf1;", "ñ" );

    Result = Result.Replace( "&#xf2;", "ò" );
    Result = Result.Replace( "&#xf3;", "ó" );
    Result = Result.Replace( "&#xf4;", "ô" );
    Result = Result.Replace( "&#xf5;", "õ" );
    Result = Result.Replace( "&#xf6;", "ö" );

    // F7) ÷
    // F8) ø

    Result = Result.Replace( "&#xf9;", "ù" );
    Result = Result.Replace( "&#xfa;", "ú" );
    Result = Result.Replace( "&#xfb;", "û" );
    Result = Result.Replace( "&#xfc;", "ü" );

    // FE) þ
    Result = Result.Replace( "&#xfd;", "ý" );
    Result = Result.Replace( "&#xff;", "ÿ" );

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
    Result = Result.Replace( "&shy;", " " ); // hyphen for word wrap?
    //    As in Resum&eacute;
    Result = Result.Replace( "&eacute;", "é" );


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

    Result = Result.Replace( "%26", "&" );

    return Result;
    }



  }
}
