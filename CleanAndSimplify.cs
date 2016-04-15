// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com



using System;
using System.Text;


namespace DGOLibrary
{
  static class CleanAndSimplify
  {

  internal static string CleanupAll( string InString )
    {
    InString = FixNonLinkParts( InString );
    InString = CleanInLinkParts( InString );
    return InString;
    }


  internal static string FixNonLinkParts( string InString )
    {
    // This was done in ReadFromTextFile().
    //  Don't go higher than 0xD800 (Surrogates).

    string Result = InString;

    // &#0187;
    // This one is very high in the frequency count.
    Result = Result.Replace( "&#0187;", "»" );

    Result = Result.Replace( "&#x2013;", "-" );
    Result = Result.Replace( "&#x2014;", "—" );
    Result = Result.Replace( "&#8211;", "-" );
    Result = Result.Replace( "&#8226;", "•" );
    Result = Result.Replace( "&#8230;", "…" );
    Result = Result.Replace( "&#xa9;", "©" );
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

    Result = Result.Replace( "&eacute;", "é" );

    return Result;
    }



  internal static string CleanInLinkParts( string InString )
    {
    // This link has semi-colons and everything:
    // http://www.durangoherald.com/article/20160317/ARTS01/160319611/0/Arts/&#x2018;Legally-Blonde-the-Musical&#x2019;-better-than-the-book-or-movie

    string Result = InString;

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

    Result = Result.Replace( "&#xad;", " " ); // hyphen for word wrap I think.

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

    // Do this last.
    Result = Result.Replace( "&amp;", "&" );

    Result = Result.Replace( "%26", "&" );

    return Result;
    }


  }
}
