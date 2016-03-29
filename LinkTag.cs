// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;


namespace DGOLibrary
{
  class LinkTag // : Make it a super class of BasicTag?
  {
  private BasicTag BaseTag;



  private LinkTag()
    {

    }


  internal LinkTag( Page UsePage, string UseText, string RelativeURL )
    {
    BaseTag = new BasicTag( UsePage, UseText, RelativeURL );
    }


  internal void ParseLink()
    {
    // This link has semi-colons and everything:
    // http://www.durangoherald.com/article/20160317/ARTS01/160319611/0/Arts/&#x2018;Legally-Blonde-the-Musical&#x2019;-better-than-the-book-or-movie

    // Arts/&#x2018;Legally-Blonde-the-Musical&#x2019;-better-than-the-book-or-movie

    try
    {
    if( BaseTag.GetCallingPage().GetIsCancelled())
      return;

    string FullText = BaseTag.GetMainText();
    // BaseTag.GetCallingPage().AddStatusString( "Doing ParseLink with:", 500 );
    // BaseTag.GetCallingPage().AddStatusString( FullText, 5000 );

    if( FullText.EndsWith( ">" ))                              // </a>
      FullText = Utility.TruncateString( FullText, FullText.Length - 4 );

    if( FullText.Length < 10 )
      return;

    if( LinkStringContainsBadStuff( FullText ))
      {
      // BaseTag.GetCallingPage().AddStatusString( "Link has bad stuff.", 5000 );
      return;
      }

    if( FullText.Contains( "<span" ))
      {
      // MForm.ShowStatus( "This has a span tag:" );
      // MForm.ShowStatus( InString );
      // MForm.ShowStatus( " " );
      return;
      }

    // bool HasImageTag = false;
    if( FullText.Contains( "<img" ))
      {
      return; // Don't get this for now.
      // Image as link: <img> is a tag within 'a' tag
      // but it only has attributes and it has no ending
      // /> tag.  It's like <br> with no end tag.
      // <a href="http://example.org"><img src="image.gif" alt="descriptive text" width="50" height="50" border="0"></a>.

      // HasImageTag = true;

      // Not this.
      // InString = Utility.RemoveFromStartToEnd( "<img", "/>", InString );

      // InString = Utility.RemoveFromStartToEnd( "<img", ">", InString );
      // InString = InString.Trim();
      // if( InString =="href=\"/\"></a" )
        // return;

      // MForm.ShowStatus( "The img tag was removed: " + InString );
      }

    string[] LinkParts = FullText.Split( new Char[] { '>' } );
    if( LinkParts.Length < 2 )
      {
      BaseTag.GetCallingPage().AddStatusString( "LinkParts.Length < 2: " + FullText, 500 );
      return;
      }

    string Attributes = LinkParts[0].Trim();
    string Title = LinkParts[1].Trim();
    // Title = Title.Replace( "</a", "" ).Trim();
    if( Title.Length < 3 )
      {
      // Title could be: RSS.
      // if( !HasImageTag )
        // BaseTag.GetCallingPage().AddStatusString( "No link title in: " + FullText, 500 );

      return;
      }

    if( Title.Contains( "Read the next article in" ))
      return;


    if( !Attributes.ToLower().Contains( "href=" ))
      {
      BaseTag.GetCallingPage().AddStatusString(  "No link in: " + FullText, 500 );
      return;
      }

    // if( InString.Contains( "/pbcs.dll/personalia?id=" ))
    if( FullText.Contains( "/pbcs.dll/personalia" ))
      {
      /*
      if( !((Title.Contains( "General Inquiries") ))) // ||
        {
        // MForm.ShowStatus( "File Name: " + CallingPage.GetFileName());
        // MForm.ShowStatus( "By line: " + Title );
        // These words won't be linked to anything
        // since this link doesn't get used.
        ParseWords( "not used", Title );
        } */

      /*
      // Don't index these:
      InString = InString.Replace( "herald staff report", " " );
      InString = InString.Replace( "herald staff writer", " " );
      InString = InString.Replace( "associated press", " " );
        the denver post
        special to the herald
        high country news
         ap medical writer
      */
      return;
      }

    if( FullText.Contains( "/pbcs.dll/" ))
      {
      // MForm.ShowStatus( "Pbcs.dll: " + InString );
      return;
      }

    // If the string was empty this would return zero
    // instead of -1.
    int LinkStart = Attributes.ToLower().IndexOf( "href=" );
    if( LinkStart > 0 )
      Attributes = Attributes.Substring( LinkStart );

    // Substring( Start ) to the end.
    // Substring( Start, Length )

    Attributes = Attributes.Replace( "href=", "" );
    Attributes = Attributes.Replace( "HREF=", "" );
    Attributes = Attributes.Replace( "\"", "" );
    Attributes = Attributes.Trim();

    string[] AttribParts = Attributes.Split( new Char[] { ' ' } );
    if( AttribParts.Length < 1 )
      {
      BaseTag.GetCallingPage().AddStatusString( "AttribParts.Length < 1: " + Attributes, 500 );
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
      LinkURL = BaseTag.GetRelativeURLBase() + LinkURL;
      // LinkURL = "http://www.durangoherald.com" + LinkURL;

    if( !LinkIsGood( LinkURL, Title ))
      {
      // BaseTag.GetCallingPage().AddStatusString( "Not using URL: " + LinkURL, 500 );
      return;
      }

    /*
    if( InString.Contains( "Caption: class=\"caption" ))
      {
      BaseTag.GetCallingPage().AddStatusString( " ", 10 );
      BaseTag.GetCallingPage().AddStatusString( "Title: " + Title, 500 );
      BaseTag.GetCallingPage().AddStatusString( "LinkURL: " + LinkURL, 500 );
      } */

    // Add it if it's not already there.
    BaseTag.GetCallingPage().AddLink( Title, LinkURL );

    }
    catch( Exception Except )
      {
      BaseTag.GetCallingPage().AddStatusString( "Exception in LinkTag.ParseLink().", 500 );
      BaseTag.GetCallingPage().AddStatusString( Except.Message, 500 );
      }
    }



  private bool LinkStringContainsBadStuff( string InString )
    {
    InString = InString.ToLower();

    if( InString.Contains( "class=\"next\"" ))
      return true;

    if( InString.Contains( "play_video" ))
      return true;

    if( InString.Contains( "thecloudscout.com" ))
      return true;

    if( InString.Contains( "<a href=\"mailto:" ))
      return true;

    if( InString.Contains( "http://wapo.st/" ))
      return true;

    if( InString.Contains( "class=\"jflowprev" ))
      return true;

    if( InString.Contains( "class=\"jflownext" ))
      return true;

    if( InString.Contains( "class=\"other-editions" ))
      return true;

    if( InString.Contains( "onclick=\"" ))
      return true;

    if( InString.Contains( "ctl00_contentplaceholder" ))
      return true;

    if( InString.Contains( "fb:comments-count" ))
      return true;

    if( InString.Contains( "www.legacy.com/ns/termsofuse.aspx" ))
      return true;

    // Obituaries for a particular person:
    // if( InString.Contains( ".aspx?" ))
      // return true;

    // if( InString.Contains( "/storyimage/" ))
      // return true;

    return false;
    }



   private bool LinkIsGood( string TestURL, string Title )
     {
     TestURL = TestURL.ToLower();
     Title = Title.ToLower();

     if( TestURL.ToLower().EndsWith( ".pdf" ))
       return false;

     if( TestURL.ToLower().EndsWith( ".exe" ))
       return false;

     if( TestURL.ToLower().EndsWith( ".bat" ))
       return false;

     if( TestURL.Contains( "durangoherald.com/#tab" ))
       return false;

     if( TestURL.Contains( "durangoherald.com" ))
       {
       if( TestURL.Contains( "/taxonomy/" ))
         return false;

       if( TestURL.Contains( "/frontpage/" ))
         return false;

       }
 
    if( Title == "read more" )
      return false;

     if( TestURL.StartsWith( "http://www.durangoherald.com/section/maps" ))
       return false;

     /////////////////
     // Do checks for false above this point.
     // Limit the scope of this project to only certain
     // domain names.

     // if( TestURL.StartsWith( "https://www.colorado.gov/" ))
       // return true;

     // if( TestURL.StartsWith( "http://www.durangogov.org/" ))
       // return true;

     if( TestURL.StartsWith( "http://www.durangoherald.com/" ))
       return true;

     if( TestURL.StartsWith( "http://obituaries.durangoherald.com/" ))
       return true;

     return false;
     }


  }
}


