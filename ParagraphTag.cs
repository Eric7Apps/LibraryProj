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

    if( FullText.Contains( "abusiveuser" ))
      GetCallingPage().AddStatusString( "abusiveuser: " + FullText, 1500 );

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
          GetCallingPage().AddStatusString( "SplitS is 1 or zero in the Paragraph: ", 500 );
          GetCallingPage().AddStatusString( FullText, 500 );
          }
        }
      }

    FullText = FullText.Replace( "herald staff writer", " " );
    FullText = FullText.Replace( "durango herald", " " );
    FullText = FullText.Replace( "<strong", " " );
    FullText = FullText.Replace( "</strong", " " );
    FullText = FullText.Replace( "<center", " " );
    FullText = FullText.Replace( "</center", " " );
    FullText = FullText.Replace( "<br/", " " );
    FullText = FullText.Replace( "<br", " " );
    // FullText = FullText.Replace( "</i", " " );
    // FullText = FullText.Replace( "<i", " " );
    // FullText = FullText.Replace( "</b", " " );
    // FullText = FullText.Replace( "<b", " " );
    FullText = FullText.Replace( "</em", " " );
    FullText = FullText.Replace( "<em", " " );
    FullText = FullText.Replace( "</li", " " );
    FullText = FullText.Replace( "<li", " " );
    FullText = FullText.Replace( "<sup", " " );
    FullText = FullText.Replace( "</sup", " " );
    FullText = FullText.Replace( "<hr", " " );

    int TagStart = FindFirstTagIndex( 0, FullText );

    if( FullText.Contains( "<byttl" ))
      {
      // by katy daigle<byttl associated press</byttl  
      string Writer = Utility.TruncateString( FullText, TagStart );
      // GetCallingPage().AddStatusString( "Writer: " + Writer, 5000 );
      FullText = Writer;
      }

    /*
    TagStart = FindFirstTagIndex( 0, FullText );
    if( TagStart >= 0 )
      {
      GetCallingPage().AddStatusString( " ", 500 );
      GetCallingPage().AddStatusString( "Tag in the Paragraph?: ", 500 );
      GetCallingPage().AddStatusString( FullText, 5000 );
      }
      */

    /*
    if( FullText.Contains( "ocntina" ))
      {
      GetCallingPage().AddStatusString( " ", 500 );
      GetCallingPage().AddStatusString( "ocntina: ", 500 );
      GetCallingPage().AddStatusString( FullText, 1500 );
      }
      */

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

    // GetCallingPage().AddStatusString( " ", 500 );
    // GetCallingPage().AddStatusString( "Paragraph: ", 500 );
    // GetCallingPage().AddStatusString( FullText, 5000 );

    string SearchText = CleanAndSimplify.SimplifyCharacterCodes( FullText );
    GetCallingPage().AddToSearchableContents( SearchText );

    SortedDictionary<string, int> WordsDictionary = ParseText( SearchText );
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
    InString = InString.Replace( "—", " " );

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

      // GetCallingPage().AddParaGraphWordCount( Word );

      if( Word == "the" )
        continue;

      if( Word == "and" )
        continue;

      // if( Word == "for" )
        // continue;

       // What are the most frequent excluded words?
      if( ExcludedWords.IsExcluded( Word ))
        continue;

      WordsDictionary[Word] = 1;
      }

    return WordsDictionary;
    }



  private bool ContainsBadStuff( string InString )
    {
    if( InString.Contains( "<u print ad dimensions" ))
      return true;

    if( InString.Contains( "twitter.com" ))
      return true;

    if( InString.Contains( "onclick=" ))
      return true;

    if( InString.Contains( "<form" ))
      return true;

    // <HL2 from the Megamillions lottery bad tag.
    if( InString.Contains( "<hl2" ))
      return true;

    if( InString.Contains( "<table" ))
      return true;

    if( InString.Contains( "<tr" ))
      return true;

    if( InString.Contains( "/img/3bull.png" ))
      return true;

    // It's either bad stuff or I don't want to deal
    // with parsing this stuff yet.
    if( InString.Contains( "<img" ))
      return true;

    // <meta content="word.document"
    if( InString.Contains( "<meta" ))
      return true;

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
