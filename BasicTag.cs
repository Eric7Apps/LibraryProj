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
  private string RelativeURLBase = "";



  private BasicTag()
    {
    }



  internal BasicTag( Page UsePage, string UseText, string RelativeURL )
    {
    CallingPage = UsePage;
    MainText = UseText;
    RelativeURLBase = RelativeURL;
    }


  protected string GetMainText()
    {
    return MainText;
    }


  protected Page GetCallingPage()
    {
    return CallingPage;
    }


  protected string GetRelativeURLBase()
    {
    return RelativeURLBase;
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
    InString = InString.ToLower();
    string Result = "";
    for( int Count = StartAt; Count < InString.Length; Count++ )
      {
      if( (InString[Count] == ' ' ) ||
          (InString[Count] == ':' ) ||  // fb:recommendations:
          (InString[Count] == '/' ) ||  // <p/>
          (InString[Count] == '\r' ) ||
          (InString[Count] == '>' ))
        return Result;

      Result += InString[Count];
      }

    // Trim() for what?
    return Result; // .Trim();
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
    InString = InString.ToLower();

    for( int Count = StartAt; Count < InString.Length; Count++ )
      {
      // Paragraph tags can't be nested.
      if( Count >= 1 )
        {
        if( InString[Count] == 'p' )
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
          if( InString[Count - 1] == 'p' )
            {
            if( InString[Count - 2] == '/' )
              return Count + 1;

            }
          }
        }
      }

    return -1;
    }



  protected bool IsFullTag( string TagName )
    {
    if( (TagName == "a") ||
        (TagName == "abbr") ||
        (TagName == "address") ||
        (TagName == "area") ||
        (TagName == "article") ||
        (TagName == "aside") ||
        (TagName == "audio") ||
        (TagName == "b") ||
        (TagName == "base") ||
        (TagName == "big") ||
        (TagName == "bdi") ||
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
        (TagName == "kbd") ||
        (TagName == "keygen") ||
        (TagName == "label") ||
        (TagName == "legend") ||
        (TagName == "li") ||
        (TagName == "main") ||
        (TagName == "map") ||
        (TagName == "mark") ||
        (TagName == "menu") ||
        (TagName == "menuitem") ||
        (TagName == "meter") ||
        (TagName == "nav") ||
        (TagName == "noframes") ||
        (TagName == "noscript") ||
        (TagName == "object") ||
        (TagName == "ol") ||
        (TagName == "optgroup") ||
        (TagName == "option") ||
        (TagName == "output") ||
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
    string StartName = "";

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
      StartName = FindTagName( StartAt, MainText );
      if( StartName.Length < 1 )
        {
        string ShowS = "There was no tag name: " + MainText;
        CallingPage.AddStatusString( ShowS, 500 );
        continue;
        }

      if( StartName.Length > 20 )
        {
        // CallingPage.AddStatusString( "StartName.Length > 20.", 500 );
        // CallingPage.AddStatusString( StartName, 500 );
        continue;
        }

      if( !StartName.StartsWith( "h" ))
        {
        if( !IsAllLetters( StartName ))
          {
          // CallingPage.AddStatusString( "Tag name is not all letters.", 500 );
          // CallingPage.AddStatusString( StartName, 500 );
          continue;
          }
        }

      StartAt += StartName.Length;
      if( StartName == "br/" )
        continue;

      if( StartName == "br" )
        continue;

      if( StartName == "fb" ) // Facebook's own tags.
        continue;     // fb:comments-count:

      int TestEnd = -1;
      if( (StartName == "img") ||
          (StartName == "link") ||
          (StartName == "hr"))
        {
        TestEnd = FindIMGTypeTagEnd( StartAt, MainText );
        }

      if( TestEnd < 0 )
        {
        if( StartName == "p" )
          {
          TestEnd = FindParagraphTagEnd( StartAt, MainText );
          }
        }

      if( TestEnd < 0 )
        {
        if( (StartName == "link") ||
            (StartName == "meta") ||
            (StartName == "param") ||
            (StartName == "embed") ||
            (StartName == "input"))
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
        if( IsFullTag( StartName ))
          {
          // CallingPage.AddStatusString( "Finding full tag end.", 500 );
          TestEnd = FindFullTagEnd( StartAt, StartName, MainText );
          }
        }

      if( TestEnd < 0 )
        {
        if( StartName == "a" )
          {
          TestEnd = FindSingleTagEnd( StartAt, MainText );
          if( TestEnd > 0 )
            {
            // CallingPage.AddStatusString( " ", 5 );
            // CallingPage.AddStatusString( "A tag I don't want: " + StartName + ": " + GetTruncatedErrorText( StartAt, MainText, 500 ), 500 );
            StartAt = TestEnd; //  + 1;
            continue;
            }
          else
            {
            // CallingPage.AddStatusString( " ", 5 );
            // CallingPage.AddStatusString( "Part 2: A tag I don't want: " + StartName + ": " + GetTruncatedErrorText( StartAt, MainText, 500 ), 500 );
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

        ShowUnknownTag( StartName, StartAt );

        if( TestEnd > 0 )
          StartAt = TestEnd;

        continue;
        }

      if( TestEnd < 0 )
        {
        CallingPage.AddStatusString( " ", 5 );
        CallingPage.AddStatusString( "Part 2: Unknown tag: " + StartName + ": " + GetTruncatedErrorText( StartAt, MainText, 5000 ), 5000 );
        continue;
        }

      int SubLength = TestEnd - StartAt;
      // CallingPage.AddStatusString( "SubLength: " + SubLength.ToString(), 500 );

      string NewTagS = MainText.Substring( StartAt, SubLength );
      NewTagS = NewTagS.Trim();

      if( StartName == "p" )
        {
        ParagraphTag PTag = new ParagraphTag( CallingPage, NewTagS, RelativeURLBase );
        PTag.ParseParagraph();
        }


        // CallingPage.AddStatusString( "NewTagS: " + NewTagS, 500 );

      if( StartName == "a" )
        {
        LinkTag LTag = new LinkTag( CallingPage, NewTagS, RelativeURLBase );
        LTag.ParseLink();
        }

      if( StartName == "p" )
        {
        // LinkTag LTag = new LinkTag( CallingPage, NewTagS, RelativeURLBase );
        // LTag.ParseLink();
        }

      // There are links in Paragraphs sometimes.
      if( SubTagCanBeParsed( StartName ) )
        {
        BasicTag TagToAdd = new BasicTag( CallingPage, NewTagS, RelativeURLBase );
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



  private void ShowUnknownTag( string StartName, int StartAt )
    {
    // Stuff that needs work:

    string TestS = GetTruncatedErrorText( StartAt, MainText, 500 );
    if( StartName == "a" )
      {
      if( TestS.Contains( "thecloudscout.com" ))
        return;

      }

    if( TestS.Contains( "civicplus.com" ))
      return;

    if( StartName == "form" )
      {
      // :  name="aspnetForm" 
      return;
      }

    if( StartName == "telerik" )
      {
      // RadScriptBlock>
      return;
      }

    if( StartName == "gcse" )
      {
      return;
      }

    if( StartName == "svg" )
      {
      return;
      }

    if( StartName == "defs" )
      {
      return;
      }

    if( StartName == "filter" )
      {
      return;
      }

    if( StartName == "o" )
      {
      return;
      }

    if( StartName == "pbs" )
      {
      return;
      }

    if( StartName == "head" )
      {
      // if( TestS.ToLower().Contains( "legacy.com" ))
        return;

      }

    if( StartName == "iframe" )
      {
      if( TestS.ToLower().Contains( "washingtonpost.com/video/" ))
        return;

      }

    // Mega Millions  Estimated jackpot
    if( StartName == "hl2" ) // Like hL2.
      return;

    if( StartName == "byttl" )   // Byline title apparently.
      return;

    if( StartName == "dd" )
      return;

    if( StartName == "b" )
      return;

    if( StartName == "i" )
      return;

    if( StartName == "object" )
      {
      // if( TestS.ToLower().Contains( "legacy.com" ))
        return;

      }

    if( StartName == "span" )
      {
      // if( TestS.ToLower().Contains( "legacy.com" ))
        return;

      }

    if( TestS.ToLower().Contains( "obituaries" ))
      return;

    if( StartName == "li" )
      {
      // if( TestS.ToLower().Contains( "
        return;

      }

    if( StartName == "ul" )
      {
      // if( TestS.ToLower().Contains( "
        return;

      }

    // if( TestS.ToLower().Contains( "/images/footer-logo.png" ))
      // return;

    if( StartName == "cr" )
      {
      // class="other-editions">

      // if( TestS.ToLower().Contains( "
        return;

      }

    // ...</li>
    if( StartName == "p" )
      {
      if( TestS.ToLower().Contains( "</li>" ))
        return;

      }

    // h1, h2 ...
    if( StartName.StartsWith( "h" ))
      {
      if( StartName.Length == 2 )
        return;

      }

    if( StartName == "fegaussianblur" )
      return;

    if( StartName == "fecolormatrix" )
      return;

    if( StartName == "fecomposite" )
      return;


    CallingPage.AddStatusString( " ", 5 );
    CallingPage.AddStatusString( "Unknown for tag: " + StartName + ": " + TestS, 500 );
    }



  private bool SubTagCanBeParsed( string TagName )
    {
    if( TagName == "a" )
      return false;

    if( TagName == "fecolormatrix" )
      return false;

    if( TagName == "fecomposite" )
      return false;

    if( TagName == "o" )
      return false;

    if( TagName == "gcse" )
      return false;

    if( TagName == "svg" )
      return false;

    if( TagName == "pbs" )
      return false;

    if( TagName == "fegaussianblur" )
      return false;

    if( TagName == "defs" )
      return false;

    if( TagName == "filter" )
      return false;

    if( TagName == "telerik" )
      return false;

    if( TagName == "form" )
      return false;

    if( TagName == "input" )
      return false;

    if( TagName == "aspnetform" )
      return false;

    if( TagName == "filter" )
      return false;

    if( TagName == "defs" )
      return false;

    if( TagName == "svg" )
      return false;

    if( TagName == "cr" )
      return false;

    if( TagName == "object" )
      return false;

    if( TagName == "iframe" )
      return false;

    if( TagName == "embed" )
      return false;

    return true;
    }


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

