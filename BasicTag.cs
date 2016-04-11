// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
// using System.Collections.Generic;
using System.Text;
// using System.Threading.Tasks;


namespace DGOLibrary
{
  class BasicTag
  {
  // private BasicTag[] ContainedTags;
  // private int ContainedTagsLast = 0;
  private string MainText = "";
  private Page CallingPage;
  private string RelativeURLFixed = "";



  private BasicTag()
    {
    }



  internal BasicTag( Page UsePage, string UseText, string RelativeURL )
    {
    CallingPage = UsePage;
    MainText = UseText;
    RelativeURLFixed = RelativeURL;
    }


  protected string GetMainText()
    {
    return MainText;
    }


  protected Page GetCallingPage()
    {
    return CallingPage;
    }


  protected string GetRelativeURL()
    {
    return RelativeURLFixed;
    }



  protected int FindFirstTagIndex( int StartAt, string InString )
    {
    // InString = InString.ToLower();
    for( int Count = StartAt; (Count + 1) < InString.Length; Count++ )
      {
      if( (InString[Count] == '<' ) &&
          (InString[Count + 1] != '!' ) &&
          (InString[Count + 1] != '/' ))
        return Count;

      }

    return -1;
    }


  private bool IsAllLetters( string InString )
    {
    for( int Count = 0; Count < InString.Length; Count++ )
      {
      if( !Char.IsLetter( InString[Count] ))
        return false;

      }

    return true;
    }



  protected string FindTagName( int StartAt, string InString )
    {
    // ToLower() takes a lot of time on a long string.
    // InString = InString.ToLower();
    string Result = "";
    for( int Count = StartAt; Count < InString.Length; Count++ )
      {
      if( (InString[Count] == ' ' ) ||
          (InString[Count] == ':' ) ||  // fb:recommendations:
          (InString[Count] == '/' ) ||  // <p/>
          (InString[Count] == '\r' ) ||
          (InString[Count] == '>' ))
        return Result.ToLower();

      Result += InString[Count];
      }

    return Result.ToLower();
    }



  protected int FindIMGTypeTagEnd( int StartAt, string InString )
    {
    // The img tag and the br tag can end in />
    // like in <br/> or they can end with
    // just the > character like with <br>.

    // <a href="/" title="Home" rel="home" 
    //       class="site-logo">
    //   <img id="main-logo" 
    //   src="/sites/all/themes/colorado_gov/logo.svg"
    //   alt="Home" />
    // </a>

    // InString = InString.ToLower();
    for( int Count = StartAt; Count < InString.Length; Count++ )
      {
      if( InString[Count] == '>' )
        {
        return Count + 1;
        }
      }

    return -1;
    }



  protected int FindSingleTagEnd( int StartAt, string InString )
    {
    // InString = InString.ToLower();

    // This doesn't work if it has nested tags within it.
    // But it's not supposed to have nested tags within it.
    for( int Count = StartAt; Count < InString.Length; Count++ )
      {
      if( InString[Count] == '>' )
        {
        if( InString[Count - 1] == '/' )
          {
          return Count + 1;
          }
        else
          {
          return -1; // It's not a simple tag.
          }
        }
      }

    return -1;
    }



  protected int FindParagraphTagEnd( int StartAt, string InString )
    {
    // Some things don't require an end tag.  Like
    // a <p> tag with no ending tag.  It might just
    // get to the start of a new paragraph.
    // InString = InString.ToLower();

    for( int Count = StartAt; Count < InString.Length; Count++ )
      {
      // Paragraph tags can't be nested.
      if( Count >= 1 )
        {
        if( (InString[Count] == 'p') ||
            (InString[Count] == 'P'))
          {
          if( InString[Count - 1] == '<' )
            return Count - 2;

          }
        }

      // /p>
      if( Count >= 2 )
        {
        if( InString[Count] == '>' )
          {
          if( (InString[Count - 1] == 'p') ||
              (InString[Count - 1] == 'P'))
            {
            if( InString[Count - 2] == '/' )
              return Count + 1;

            }
          }
        }
      }

    return -1;
    }


  protected bool IsSimpleTag( string TagName )
    {
    if( (TagName == "link") ||
        // (TagName == "area") ||
        (TagName == "meta") ||
        (TagName == "param") ||
        (TagName == "embed") ||
        (TagName == "input"))
      return true;

    return false;
    }



  protected bool IsFullTag( string TagName )
    {
    if( (TagName == "a") ||
        (TagName == "abbr") ||
        (TagName == "acronym") ||
        (TagName == "address") ||
        (TagName == "applet") ||
        // (TagName == "area") ||
        (TagName == "article") ||
        (TagName == "aside") ||
        (TagName == "audio") ||
        (TagName == "b") ||
        (TagName == "base") ||
        (TagName == "basefont") ||
        (TagName == "bdi") ||
        (TagName == "bdo") ||
        (TagName == "big") ||
        (TagName == "blockquote") ||
        (TagName == "body") ||
        (TagName == "button") ||
        (TagName == "canvas") ||
        (TagName == "caption") ||
        (TagName == "center") ||
        (TagName == "cite") ||
        (TagName == "code") ||
        (TagName == "col") ||
        (TagName == "colgroup") ||
        (TagName == "datalist") ||
        (TagName == "dd") ||
        (TagName == "del") ||
        (TagName == "details") ||
        (TagName == "dfn") ||
        (TagName == "dialog") ||
        (TagName == "dir") ||
        (TagName == "div") ||
        (TagName == "dl") ||
        (TagName == "dt") ||
        (TagName == "em") ||
        // (TagName == "embed") ||
        (TagName == "fieldset") ||
        (TagName == "figcaption") ||
        (TagName == "figure") ||
        (TagName == "font") ||
        (TagName == "footer") ||
        (TagName == "form") ||
        (TagName == "frame") ||
        (TagName == "frameset") ||
        (TagName == "h1") ||
        (TagName == "h2") ||
        (TagName == "h3") ||
        (TagName == "h4") ||
        (TagName == "h5") ||
        (TagName == "h6") ||
        (TagName == "head") ||
        (TagName == "header") ||
        // (TagName == "hr") ||
        (TagName == "html") ||
        (TagName == "i") ||
        (TagName == "iframe") ||
        (TagName == "ins") ||
        // input
        (TagName == "kbd") ||
        (TagName == "keygen") ||
        (TagName == "label") ||
        (TagName == "legend") ||
        (TagName == "li") ||
        // link
        (TagName == "main") ||
        (TagName == "map") ||
        (TagName == "mark") ||
        (TagName == "menu") ||
        (TagName == "menuitem") ||
        // meta
        (TagName == "meter") ||
        (TagName == "nav") ||
        (TagName == "noframes") ||
        (TagName == "noscript") ||
        (TagName == "object") ||
        (TagName == "ol") ||
        (TagName == "optgroup") ||
        (TagName == "option") ||
        (TagName == "output") ||
        // (TagName == "p") ||
        // (TagName == "param") ||
        (TagName == "pre") ||
        (TagName == "progress") ||
        (TagName == "q") ||
        (TagName == "rp") ||
        (TagName == "rt") ||
        (TagName == "ruby") ||
        (TagName == "s") ||
        (TagName == "samp") ||
        (TagName == "script") ||
        (TagName == "section") ||
        (TagName == "select") ||
        (TagName == "small") ||
        (TagName == "source") ||
        (TagName == "span") ||
        (TagName == "strike") ||
        (TagName == "strong") ||
        (TagName == "style") ||
        (TagName == "sub") ||
        (TagName == "summary") ||
        (TagName == "sup") ||
        (TagName == "table") ||
        (TagName == "tbody") ||
        (TagName == "td") ||
        (TagName == "textarea") ||
        (TagName == "tfoot") ||
        (TagName == "th") ||
        (TagName == "thead") ||
        (TagName == "time") ||
        (TagName == "title") ||
        (TagName == "tr") ||
        (TagName == "track") ||
        (TagName == "tt") ||        // Teletype
        (TagName == "u") ||
        (TagName == "ul") ||
        (TagName == "var") ||
        (TagName == "video") ||
        (TagName == "wbr" ))
      return true;

    return false;
    }



  protected int FindFullTagEnd( int StartAt, string TagName, string InString )
    {
    // TagName is already lower case.
    // InString = InString.ToLower();

    // If you have
    // <div>
    //   <div>
    //     <div>
    //     </div>
    //   </div>
    // </div>
    // Then this has to find the matching end tag.

    int NestLevel = 1;
    int TagLengthPlus1 = TagName.Length + 1;

    for( int Count = StartAt; (Count + TagLengthPlus1) < InString.Length; Count++ )
      {
      if( InString[Count] == '<' )
        {
        if( Utility.MatchesTestString( Count + 1, InString, TagName ))
          {
          NestLevel++;
          // if( TagName == "div" )
            // CallingPage.AddStatusString( "NestLevel: " + NestLevel.ToString(), 500 );

          }
        }

      if( InString[Count] == '/' )
        {
        if( Utility.MatchesTestString( Count + 1, InString, TagName + ">" ))
          {
          NestLevel--;
          // if( TagName == "div" )
            // CallingPage.AddStatusString( "NestLevel: " + NestLevel.ToString(), 500 );

          if( NestLevel < 1 )
            return Count + TagLengthPlus1 + 1;

          }
        }
      }

    return -1;
    }



  protected string GetTruncatedErrorText( int StartAt, string InString, int HowLong )
    {
    StringBuilder SBuilder = new StringBuilder();

    for( int Count = StartAt; Count < InString.Length; Count++ )
      {
      // MForm.ShowStatus( Char.ToString( MainText[Count] ));
      SBuilder.Append( InString[Count] );
      if( SBuilder.Length >= HowLong )
        break;

      }

    return SBuilder.ToString();
    }



  private bool IsTagStart( int StartAt, string InString )
    {
    // <!DOCTYPE
    if( (StartAt + 1) >= InString.Length )
      return false;

    if( Char.IsLetter( InString[StartAt + 1] ))
      return true;

    return false;
    }



  internal void MakeContainedTags()
    {
    try
    {
    if( CallingPage.GetIsCancelled())
      return;

    int StartAt = 0;
    while( true )
      {
      int Index = FindFirstTagIndex( StartAt, MainText );
      if( Index < 0 )
        {
        // MForm.ShowStatus( "No more tags." );
        return;
        }

      if( !IsTagStart( Index, MainText ))
        {
        StartAt = Index + 1;
        continue;
        }

      StartAt = Index + 1;
      string TagName = FindTagName( StartAt, MainText );
      if( TagName.Length < 1 )
        {
        string ShowS = "There was no tag name: " + MainText;
        CallingPage.AddStatusString( ShowS, 500 );
        continue;
        }

      if( TagName.Length > 15 )
        continue;

      if( TagName[0] != 'h' )
        {
        if( !IsAllLetters( TagName ))
          {
          // CallingPage.AddStatusString( "Tag name is not all letters.", 500 );
          // CallingPage.AddStatusString( TagName, 500 );
          continue;
          }
        }

      StartAt += TagName.Length;
      if( TagName == "br/" )
        continue;

      if( TagName == "br" )
        continue;

      if( TagName == "fb" ) // Facebook's own tags.
        continue;     // fb:comments-count:

      int TestEnd = -1;
      if( (TagName == "img") ||
          (TagName == "area") ||
          (TagName == "link") ||
          (TagName == "hr"))
        {
        TestEnd = FindIMGTypeTagEnd( StartAt, MainText );
        }

      if( TestEnd < 0 )
        {
        if( TagName == "p" )
          {
          TestEnd = FindParagraphTagEnd( StartAt, MainText );
          }
        }

      if( TestEnd < 0 )
        {
        if( IsSimpleTag( TagName ))
          {
          // CallingPage.AddStatusString( "Finding single tag end.", 500 );
          TestEnd = FindSingleTagEnd( StartAt, MainText );
          if( TestEnd < 0 )
            {
            TestEnd = FindIMGTypeTagEnd( StartAt, MainText );
            }
          }
        }

      if( TestEnd < 0 )
        {
        if( IsFullTag( TagName ))
          {
          // CallingPage.AddStatusString( "Finding full tag end.", 500 );
          TestEnd = FindFullTagEnd( StartAt, TagName, MainText );
          }
        }

      if( TestEnd < 0 )
        {
        if( TagName == "a" )
          {
          TestEnd = FindSingleTagEnd( StartAt, MainText );
          if( TestEnd > 0 )
            {
            // CallingPage.AddStatusString( " ", 5 );
            // CallingPage.AddStatusString( "A tag I don't want: " + TagName + ": " + GetTruncatedErrorText( StartAt, MainText, 500 ), 500 );
            StartAt = TestEnd; //  + 1;
            continue;
            }
          else
            {
            // CallingPage.AddStatusString( " ", 5 );
            // CallingPage.AddStatusString( "Part 2: A tag I don't want: " + TagName + ": " + GetTruncatedErrorText( StartAt, MainText, 500 ), 500 );
            TestEnd = FindIMGTypeTagEnd( StartAt, MainText );
            StartAt = TestEnd; //  + 1;
            continue;
            }
          }
        }

      if( TestEnd < 0 )
        {
        TestEnd = FindSingleTagEnd( StartAt, MainText );
        if( TestEnd < 0 )
          {
          TestEnd = FindIMGTypeTagEnd( StartAt, MainText );
          }

        ShowUnknownTag( TagName, StartAt );

        if( TestEnd > 0 )
          StartAt = TestEnd;

        continue;
        }

      if( TestEnd < 0 )
        {
        CallingPage.AddStatusString( " ", 5 );
        CallingPage.AddStatusString( "Part 2: Unknown tag: " + TagName + ": " + GetTruncatedErrorText( StartAt, MainText, 5000 ), 5000 );
        continue;
        }

      int SubLength = TestEnd - StartAt;
      // CallingPage.AddStatusString( "SubLength: " + SubLength.ToString(), 500 );

      string NewTagS = MainText.Substring( StartAt, SubLength );
      // Don't trim().
      // NewTagS = NewTagS.Trim();
        // CallingPage.AddStatusString( "NewTagS: " + NewTagS, 500 );

      if( TagName == "p" )
        {
        ParagraphTag PTag = new ParagraphTag( CallingPage, NewTagS, RelativeURLFixed );
        PTag.ParseParagraph();
        }

      if( TagName == "a" )
        {
        LinkTag LTag = new LinkTag( CallingPage, NewTagS, RelativeURLFixed );
        LTag.ParseLink();
        }

      // There are links in Paragraphs sometimes.
      if( !DoNotParseTag( TagName ))
        {
        BasicTag TagToAdd = new BasicTag( CallingPage, NewTagS, RelativeURLFixed );
        // AddContainedTag( TagToAdd );
        TagToAdd.MakeContainedTags();
        }

      StartAt = TestEnd; //  + 1;
      }
    }
    catch( Exception Except )
      {
      CallingPage.AddStatusString( "Exception in Tag.MakeContainedTags().", 500 );
      CallingPage.AddStatusString( Except.Message, 500 );
      }
    }



  private void ShowUnknownTag( string TagName, int StartAt )
    {
    // For tags that need work:

    if( TagName == "base" )
      return;


    if( TagName == "hl2" ) // HL2 Powerball.
      return;

    if( TagName == "pbs" )
      return;

    if( TagName == "html" )
      return;

    if( TagName == "b" )
      return;

    if( TagName == "body" )
      return;

    if( TagName == "span" )
      return;

    if( TagName == "div" )
      return;

    if( TagName == "head" )
      return;

    if( TagName == "form" )
      return;

    if( TagName == "telerik" )
      return;

    if( TagName == "o" )
      return;

    if( TagName == "li" )
      return;

    if( TagName == "ul" )
      return;

    if( TagName == "byttl" )
      return;

    if( TagName == "iframe" )
      return;

    if( TagName == "h1" )
      return;

    if( TagName == "h2" )
      return;

    if( TagName == "h3" )
      return;

    if( TagName == "h4" )
      return;

    if( TagName == "h5" )
      return;

    if( TagName == "h6" )
      return;

    if( TagName == "cr" )
      return;

    if( TagName == "object" )
      return;

    if( TagName == "p" )
      return;

    if( TagName == "dd" )
      return;

    // if( TagName == "area" )
      // return;

    if( TagName == "em" )
      return;

    if( TagName == "strong" )
      return;

    if( TagName == "tr" )
      return;

    if( TagName == "td" )
      return;

    if( TagName == "table" )
      return;

    if( TagName == "fecomposite" )
      return;

    if( TagName == "fecolormatrix" )
      return;

    if( TagName == "fegaussianblur" )
      return;

    if( TagName == "filter" )
      return;

    if( TagName == "defs" )
      return;

    if( TagName == "svg" )
      return;

    if( TagName == "gcse" )
      return;

    string TestS = GetTruncatedErrorText( StartAt, MainText, 500 );

    // if( IgnoreTagForNow( TagName, TestS ))
      // return;

    // if( TagName == "p" )
      // return;

    CallingPage.AddStatusString( " ", 5 );
    CallingPage.AddStatusString( "Unknown for tag: " + TagName + ": " + TestS, 500 );
    }


  private bool DoNotParseTag( string TagName )
    {
    // Don't parse anchor tag because it's already parsed.
    if( TagName == "a" )
      return true;

    if( TagName == "td" )
      {
      // If a td tag contains class="cutline">
      // or img src= 
      // don't parse it.
      /*
      What about the paragraph tag?

      <td><img src="/06-04-06/images/0406penn-

      \Y2016\M4\D3\H10M47S53T94.txt

      <!-- begin imagebox -->
      <table width="612"  border="0" cellspacing="0"
       cellpadding="2" align="center">
      <tr>
      <td><img src="/06-04-06/images/0406penn-cut1.htm"
       border="0"></td>
       </tr>
       <tr>
       <td class="cutline">ÿØÿàJFIFddÿìDuckyý?ÓQ>2¨?æYÏ8&/"Jl8?[P>¶Dç?ßÓú§«ækËm?
       -Pa:Ð|¾´?±° ç?mX
       ßÖ???§²yq²p?¨b¯eo??²_ßmh
       t6>7£???dKÊ??Ì?>SbÚ?ÆâôÄy®;t4?Ý¯³?ï Â²ô²Ç­ºÃ¼
       */

      return true;
      }

    if( TagName == "area" )
      return true;

    if( TagName == "filter" )
      return true;

    if( TagName == "defs" )
      return true;

    if( TagName == "svg" )
      return true;

    if( TagName == "gcse" )
      return true;

    if( TagName == "form" )
      return true;

    if( TagName == "input" )
      return true;

    // if( TagName == "aspnetform" )
      // return true;

    if( TagName == "filter" )
      return true;

    if( TagName == "defs" )
      return true;

    if( TagName == "svg" )
      return true;

    if( TagName == "cr" )
      return true;

    if( TagName == "object" )
      return true;

    if( TagName == "iframe" )
      return true;

    if( TagName == "embed" )
      return true;

    if( TagName == "o" )
      return true;

    return false;
    }



  /*
  private bool IgnoreTagForNow( string TagName, string TestS )
    {
    // if( TagName == "span" )
      // return true;


    if( TagName == "head" )
      return true;

    if( TagName == "dd" )
      return true;

    if( TagName == "td" )
      return true;

    if( TagName == "byttl" )   // Byline title apparently.
      return true;

    if( TagName == "b" )
      return true;

    if( TagName == "i" )
      return true;

    if( TagName == "li" )
      return true;

    if( TagName == "fecolormatrix" )
      return true;

   if( TagName == "ul" )
      return true;

    if( TagName == "fecomposite" )
      return true;

    if( TagName == "o" )
      return true;

    if( TagName == "gcse" )
      return true;

    if( TagName == "pbs" )
      return true;

    if( TagName == "fegaussianblur" )
      return true;

    if( TagName == "defs" )
      return true;

    if( TagName == "filter" )
      return true;

    if( TagName == "telerik" )
      return true;


    // ...</li>
    if( TagName == "p" )
      {
      if( TestS.Contains( "</li>" ))
        return true;

      }

    if( TestS.Contains( "civicplus.com" ))
      return true;

    if( TestS.Contains( "obituaries" ))
      return true;

    return false;
    }
    */



  /*
  private void AddContainedTag( BasicTag ToAdd )
    {
    try
    {
    if( ContainedTags == null )
      ContainedTags = new Tag[8];

    ContainedTags[ContainedTagsLast] = ToAdd;
    ContainedTagsLast++;

    if( ContainedTagsLast >= ContainedTags.Length )
      Array.Resize( ref ContainedTags, ContainedTags.Length + 32 );

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Tag.AddContainedTag()." );
      MForm.ShowStatus( Except.Message );
      }
    }
    */



  }
}

