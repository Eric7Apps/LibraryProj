// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DGOLibrary
{
  class Tag
  {
  private MainForm MForm;
  // private string BeginTag = "";
  // private string EndTag = "";
  private string MainText = "";
  private Tag[] ContainedTags;
  private int ContainedTagsLast = 0;
  private Page CallingPage;


  private Tag()
    {
    }



  internal Tag( MainForm UseForm, Page UsePage, string UseText )
    {
    MForm = UseForm;
    CallingPage = UsePage;
    MainText = UseText;
    }



  private int GetFirstTagIndex( int StartAt )
    {
    for( int Count = StartAt; (Count + 1) < MainText.Length; Count++ )
      {
      if( (MainText[Count] == '<' ) &&
          (MainText[Count + 1] != '!' ) &&
          (MainText[Count + 1] != '/' ))
        return Count;

      }

    return -1;
    }


  private string GetTagName( int StartAt )
    {
    string Result = "";
    for( int Count = StartAt; Count < MainText.Length; Count++ )
      {
      if( (MainText[Count] == ' ' ) ||
          (MainText[Count] == '>' ))
        return Result;

      Result += MainText[Count];
      }

    // Trim() for any other white space?
    return Result.Trim();
    }



  private int GetSingleTagEnd( int StartAt )
    {
    for( int Count = StartAt; Count < MainText.Length; Count++ )
      {
      if( MainText[Count] == '>' )
        {
        if( MainText[Count - 1] == '/' )
          {
          return Count;
          }
        else
          {
          return -1; // It's not a single-tag.  (Simple tag.)
          }
        }
      }

    return -1;
    }


   private int GetFullTagEnd( int StartAt, string TagName )
    {
    // If you have
    // <div>
    //   <div>
    //     <div>
    //     </div>
    //   </div>
    // </div>
    // Then this has to find the matching end tag.
    // But they aren't nested correctly like that.
    // And you could have a <div> within a tag, but its
    // corresponding </div> is not within that tag.

    int NestLevel = 0;
    bool TagNameMatches = true;
    int TagLengthPlus1 = TagName.Length + 1;

    for( int Count = StartAt; (Count + TagLengthPlus1) < MainText.Length; Count++ )
      {
      // MForm.ShowStatus( Char.ToString( MainText[Count] ));
      if( MainText[Count] == '<' )
        {
        TagNameMatches = true;
        for( int TCount = 1; TCount < TagLengthPlus1; TCount++ )
          {
          if( MainText[Count + TCount] != TagName[TCount - 1] )
            {
            TagNameMatches = false;
            break;
            }
          }

        if( TagNameMatches )
          NestLevel++;

        }

      if( MainText[Count] == '/' )
        {
        if( MainText[Count + TagLengthPlus1] == '>' )
          {
          // MForm.ShowStatus( "Matched end tag length." );

          TagNameMatches = true;
          for( int TCount = 1; TCount < TagLengthPlus1; TCount++ )
            {
            // MForm.ShowStatus( Char.ToString( MainText[Count + TCount] ));

            if( MainText[Count + TCount] != TagName[TCount - 1] )
              {
              TagNameMatches = false;
              break;
              }
            }

          if( TagNameMatches )
            {
            if( NestLevel < 1 )
              return Count + TagLengthPlus1;

            }
          }
        }
      }

    return -1;
    }



  internal void MakeContainedTags()
    {
    try
    {
    if( MForm.GetIsClosing())
      return;

    // This should all be running in a background thread
    // if it's running on a real server.  But for now,
    // check the UI thread events.
    // It won't know if it's cancelled if it can't
    // check keyboard events.
    // GetCancelled()

    if( !MForm.CheckEvents())
      return;

    int StartAt = 0;
    string StartName = "";
    Tag TagToAdd;
    while( true )
      {
      if( MForm.GetIsClosing())
        return;

      int Index = GetFirstTagIndex( StartAt );
      if( Index < 0 )
        {
        // MForm.ShowStatus( "No more tags." );
        return;
        }

      StartAt = Index + 1;
      StartName = GetTagName( StartAt );
      if( StartName.Length < 1 )
        {
        MForm.ShowStatus( "There was no tag name." );
        return;
        }

      StartAt += StartName.Length;

      // <li><a href="/section/News04/">Business</a></li>
      // <link rel="shortcut icon" type="image/x-icon" href="/favicon.ico" />
      // <body id="home">
      // <div id="fb-root"></div>
      // <div class="container_16">

      string NewTagS = "";
      int TestEnd = GetSingleTagEnd( StartAt );
      if( TestEnd < 0 )
        {
        // MForm.ShowStatus( "Not a single-tag." );
        TestEnd = GetFullTagEnd( StartAt, StartName );
        if( TestEnd < 0 )
          {
          // Apparently this is fairly common with certain
          // tags like for bold or h3 or something like that.
          // The bold might start nested within one tag
          // but then the end tag is nested somewhere else.
          // If you are editing HTML visually then you can't
          // see that your bold tag is nexted outside of
          // the original tag.

          if( StartName == "a" )
            {
            MForm.ShowStatus( " " );
            MForm.ShowStatus( StartName + ": Didn't find the full tag end." );
            MForm.ShowStatus( MainText );
            }

          continue;
          }

        int SubLength = TestEnd - StartAt;
        NewTagS = MainText.Substring( StartAt, SubLength );

        /*
        Lede?
        StartName: p
        NewTagS:  class=lede>If there were ever a 

        There is no class name on this one:
        StartName: p
        NewTagS: >Omigod Durango, grab your finest \pink tops and glitter-studded accessories and head over to Durango High School at 7 tonight (and the next two weekends) for an...</p
        N

        By <a href="/apps/pbcs.dll/personalia?ID=jlivingston">John Livingston</a>

        <title>The Durango Herald
        03/16/2016 |
        Fort Lewis women&#x2019;s lacrosse opens home schedule in style against Oklahoma Baptist
        </title>

        <title>The Durango Herald
        03/16/2016 |
        Los Angeles Dodger Yasiel Puig homers against Colorado Rockies after finding out he won&#x2019;t be suspended
        </title>

        <p class="articleText">


        <body onload="doLoadEvent()">

        <td>
        <button type="button" onclick="zoomInButtonClick()">Zoom In</button>
        </td>

        <form>
        */

        /*
        if( !((StartName == "html" ) ||
              (StartName == "head" ) ||
              (StartName == "a" ) ||
             (StartName == "body" )))
          {
          MForm.ShowStatus( " " );
          MForm.ShowStatus( " " );
          MForm.ShowStatus( " " );
          MForm.ShowStatus( " " );
          MForm.ShowStatus( " " );
          MForm.ShowStatus( "StartName: " + StartName );
          MForm.ShowStatus( "NewTagS: " + NewTagS );
          }
          */

        if( StartName == "a" )
          {
          ParseLink( NewTagS );
          }

        // Let it process image tags in links.
        // if( !(StartName == "a" ))
          {
          TagToAdd = new Tag( MForm, CallingPage, NewTagS );
          AddContainedTag( TagToAdd );
          TagToAdd.MakeContainedTags();
          }

        StartAt = TestEnd + 1;
        }
      else
        {
        // This is a single-tag.
        int SubLength = TestEnd - StartAt;
        NewTagS = MainText.Substring( StartAt, SubLength );

        /*
        if( StartName == "some tag" )
          {
          MForm.ShowStatus( " " );
          MForm.ShowStatus( " " );
          MForm.ShowStatus( " " );
          MForm.ShowStatus( " " );
          MForm.ShowStatus( " " );
          MForm.ShowStatus( "Single-tag StartName: " + StartName );
          MForm.ShowStatus( "NewTagS: " + NewTagS );
          }
          */

        TagToAdd = new Tag( MForm, CallingPage, NewTagS );
        AddContainedTag( TagToAdd );
        TagToAdd.MakeContainedTags();
        StartAt = TestEnd + 1;
        }
      }
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Tag.MakeContainedTags()." );
      MForm.ShowStatus( Except.Message );
      }
    }



  private void ParseLink( string InString )
    {
    if( MForm.GetIsClosing())
      return;

    // This link has semi-colons and everything:
    // http://www.durangoherald.com/article/20160317/ARTS01/160319611/0/Arts/&#x2018;Legally-Blonde-the-Musical&#x2019;-better-than-the-book-or-movie

    // Arts/&#x2018;Legally-Blonde-the-Musical&#x2019;-better-than-the-book-or-movie

    try
    {
    if( InString == null )
      return;

    if( InString.Length < 10 )
      return;

    InString = Utility.GetCleanUnicodeString( InString, 5000 );

    bool HasImageTag = false;
    if( InString.Contains( "<img" ))
      {
      HasImageTag = true;
      InString = Utility.RemoveFromStartToEnd( "<img", "/>", InString );
      InString = InString.Trim();
      // MForm.ShowStatus( "The img tag was removed: " + InString );
      }

    string[] LinkParts = InString.Split( new Char[] { '>' } );
    if( LinkParts.Length < 2 )
      {
      MForm.ShowStatus( "LinkParts.Length < 2: " + InString );
      return;
      }

    string Attributes = LinkParts[0].Trim();
    string Title = LinkParts[1].Trim();
    Title = Title.Replace( "</a", "" ).Trim();
    if( Title.Length < 3 )
      {
      // Title could be: RSS.
      if( !HasImageTag )
        MForm.ShowStatus( "No link title in: " + InString );

      return;
      }

    if( !Attributes.Contains( "href=" ))
      {
      MForm.ShowStatus( "No link in: " + InString );
      return;
      }

    // If the string was empty this would return zero
    // instead of -1.
    int LinkStart = Attributes.IndexOf( "href=" );
    if( LinkStart > 0 )
      Attributes = Attributes.Substring( LinkStart );

    // Substring( Start ) to the end.
    // Substring( Start, Length )

    Attributes = Attributes.Replace( "href=", "" );
    Attributes = Attributes.Replace( "\"", "" );
    Attributes = Attributes.Trim();

    string[] AttribParts = Attributes.Split( new Char[] { ' ' } );
    if( AttribParts.Length < 1 )
      {
      MForm.ShowStatus( "AttribParts.Length < 1: " + Attributes );
      return;
      }

    string LinkURL = AttribParts[0].Trim();

    if( LinkURL == "/" )
      {  
      // MForm.ShowStatus( "Ignoring the main page for parsing." );
      return;
      }

    if( !(LinkURL.StartsWith( "http://" ) ||
          LinkURL.StartsWith( "https://" )))
      LinkURL = "http://www.durangoherald.com" + LinkURL;

    if( !LinkIsGood( LinkURL ))
      {
      // MForm.ShowStatus( "Not using URL: " + LinkURL );
      return;
      }
      
    // MForm.ShowStatus( " " );
    // MForm.ShowStatus( "Title: " + Title );
    // MForm.ShowStatus( "LinkURL: " + LinkURL );

    if( !MForm.PageList1.ContainsURL( LinkURL ))
      {
      // Get this new page:
      if( MForm.GetURLMgrForm != null )
        MForm.GetURLMgrForm.AddURLForm( Title, LinkURL, false );

      }


    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Tag.ParseLink()." );
      MForm.ShowStatus( Except.Message );
      }
    }


   private bool LinkIsGood( string LinkURL )
     {
     // Limit the scope of this project to only certain
     // domain names.

     if( LinkURL.StartsWith( "http://www.durangoherald.com/" ))
       return true;

     if( LinkURL.StartsWith( "http://obituaries.durangoherald.com/" ))
       return true;

     return false;
     }



  private void AddContainedTag( Tag ToAdd )
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



  }
}

