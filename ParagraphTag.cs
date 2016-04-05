// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DGOLibrary
{
  class ParagraphTag : BasicTag
  {


  internal ParagraphTag( Page UsePage, string UseText, string RelativeURL ) : base( UsePage, UseText, RelativeURL )
    {
    //  : base( UsePage, UseText, RelativeURL )
    }



  internal void ParseParagraph()
    {
    try
    {
    if( GetCallingPage().GetIsCancelled())
      return;

    string FullText = GetMainText().ToLower();

    if( FullText.Length < 3 )
      return;

    if( ContainsBadStuff( FullText ))
      {
      // BaseTag.GetCallingPage().AddStatusString( "Link has bad stuff.", 5000 );
      return;
      }

    FullText = FullText.Replace( "\r", " " );

    FullText = FullText.Replace( "</p>", "" ).Trim();
    FullText = Utility.RemovePatternFromStartToEnd( "<a", ">", FullText );
    FullText = FullText.Replace( "</a>", " " );

    FullText = RemoveDropCapPart( FullText );

    FullText = Utility.RemovePatternFromStartToEnd( "<span", ">", FullText );
    FullText = FullText.Replace( "</span>", " " );

    // Get rid of the parameters on the paragraph tag.
    //  <p style="margin-bottom: 0in; font-style: normal; line-height: 120%; text-decoration: none;" align="left" lang="en-US">
    string[] SplitS = FullText.Split( new Char[] { '>' } );
    if( SplitS.Length > 1 )
      {
      FullText = "";
      for( int Count = 1; Count < SplitS.Length; Count++ )
        FullText += SplitS[Count] + " ";

      }
    else
      {
      if( SplitS.Length == 0 )
        return;

      // If this string is all there is.
      if( SplitS[0] == "class=\"caption\"" )
        return;

      if( SplitS[0] == "class=\"articletext\"" )
        return;

      if( SplitS[0].Contains( "=" ))
        {
        if( !SplitS[0].Contains( "decolonizing" ))
          {
          GetCallingPage().AddStatusString( " ", 500 );
          GetCallingPage().AddStatusString( "SPlitS is 1 or zero in the Paragraph: ", 500 );
          GetCallingPage().AddStatusString( FullText, 500 );
          }
        }
      }

    FullText = FullText.Replace( "herald staff writer", " " );
    FullText = FullText.Replace( "durango herald", " " );
    FullText = FullText.Replace( "<strong", " " );
    FullText = FullText.Replace( "</strong", " " );
    FullText = FullText.Replace( "<br/", " " );
    FullText = FullText.Replace( "<br", " " );
    // FullText = FullText.Replace( "</i", " " );
    // FullText = FullText.Replace( "<i", " " );

    int TagStart = FindFirstTagIndex( 0, FullText );
    if( TagStart >= 0 )
      {
      GetCallingPage().AddStatusString( " ", 500 );
      GetCallingPage().AddStatusString( "Tag in the Paragraph?: ", 500 );
      GetCallingPage().AddStatusString( FullText, 5000 );
      }

    // FullText = FullText.Replace( "<span class=\"abody\"", " " );

    // Don't add this for every AP story.
    FullText = FullText.Replace( "associated press", " " );
    FullText = FullText.Replace( "durango herald", " " );

    // class=lede
    // lede: "The introductory section of a news story
    // that is intended to entice the reader to read the
    // full story." ~ Google
    // FullText = FullText.Replace( "class=lede", " " );

    // <span class="mwc_subheads">Free practice
    //  tests</span>The College Board 
    // FullText = FullText.Replace( "<span class=\"mwc_subheads\"", " " );

    // <p class="articleText">
    // <span class="dropcap">W</span>alking into


    /*
    FullText = FullText.Replace( "style=\"text-indent:", " " );
    FullText = FullText.Replace( "font-style:", " " );
    FullText = FullText.Replace( "line-height:", " " );
    FullText = FullText.Replace( "margin-bottom:", " " );
    FullText = FullText.Replace( "text-decoration:", " " );
    FullText = FullText.Replace( "align=\"left\"", " " );
    FullText = FullText.Replace( "lang=\"en-US\"", " " );
    InString = InString.Replace( "href=", " " );
    InString = InString.Replace( "rel=", " " );
    InString = InString.Replace( "target=", " " );
    InString = InString.Replace( "subject=", " " );
    InString = InString.Replace( "alt=", " " );
    InString = InString.Replace( "src=", " " );
    InString = InString.Replace( "class=", " " );
    */

    // GetCallingPage().AddStatusString( " ", 500 );
    // GetCallingPage().AddStatusString( "Paragraph: ", 500 );
    // GetCallingPage().AddStatusString( FullText, 5000 );

    string SearchText = CleanAndSimplify.SimplifyCharacterCodes( FullText );
    GetCallingPage().AddToSearchableContents( SearchText );

    SortedDictionary<string, int> WordsDictionary = ParseText( FullText );
    if( WordsDictionary != null )
      GetCallingPage().AddWords( WordsDictionary, GetCallingPage().GetFileName() );

    }
    catch( Exception Except )
      {
      GetCallingPage().AddStatusString( "Exception in LinkTag.ParseLink().", 500 );
      GetCallingPage().AddStatusString( Except.Message, 500 );
      }
    }



  internal SortedDictionary<string, int> ParseText( string InString )
    {
    InString = WordFix.ReplaceForSplitWords( InString );
    InString = WordFix.FixAbbreviations( InString );

    InString = InString.Replace( "’", " " );
    InString = InString.Replace( "•", " " );

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
    InString = InString.Replace( "&", " " );
    // InString = InString.Replace( "@", " " );
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







  private bool ContainsBadStuff( string InString )
    {
    if( InString.Contains( "class=\"simpleblackborder" ))
      return true;
      
    if( InString.Contains( ".rectrac.com/"  ))
      return true;


    if( InString == "class=\"caption\"" )
      return true;

    if( InString == "class=\"articletext\"" )
      return true;

    if( InString.Contains( "<span id=\"yfs_market_time" ))
      return true;

    if( InString.Contains( "<p class=\"dropdown\">" ))
      return true;

    if( InString.Contains( "<a href=\"#moreinfo" ))
      return true;

    if( InString.Contains( "class=\"myconnections\">" ))
      return true;

    if( InString.Contains( "class=\"welcomemessage\">" ))
      return true;

    if( InString.Contains( "class=\"grid_4 omega timestamp\">article last updated:" ))
      return true;

    if( InString.Contains( "class=\"flip-nav" ))
      return true;

    if( InString.Contains( "href=\"javascript:" ))
      return true;

    if( InString.Contains( "class=\"timestamp" ))
      return true;

    if( InString.Contains( "<object" ))
      return true;

    if( InString.Contains( "castfire.com" ))
      return true;

    if( InString.Contains( "allowscriptaccess" ))
      return true;

    if( InString.Contains( "x-shockwave-flash" ))
      return true;

    if( InString.Contains( "iframe" ))
      return true;

    if( InString.Contains( "allowfullscreen" ))
      return true;

    return false;
    }



  private string RemoveDropCapPart( string InString )
    {
    if( !InString.Contains( "<span class=\"dropcap\">" ))
      return InString;

    InString = InString.Replace( "class=\"articletext\">", "" );
    // <span class="dropcap">W</span>alking into
    int Start = InString.IndexOf( "<span class=\"dropcap\">" );
    if( Start < 0 )
      return InString;

    // 1234567890123456789012
    // <span class="dropcap">

    int End = InString.IndexOf( "</span>" );
    if( End < 0 )
      return InString;

    StringBuilder SBuilder = new StringBuilder();
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      if( (Count >= Start) && (Count < (Start + 22)))
        continue;

      if( (Count >= End) && (Count < (End + 7)))
        continue;

      SBuilder.Append( InString[Count] );
      }

    // MForm.ShowStatus( "DropCap: " + SBuilder.ToString());
    return SBuilder.ToString();
    }


  }
}
