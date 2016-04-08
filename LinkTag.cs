// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;


namespace DGOLibrary
{
  class LinkTag : BasicTag
  {


  internal LinkTag( Page UsePage, string UseText, string RelativeURL ) : base( UsePage, UseText, RelativeURL )
    {
    //  : base( UsePage, UseText, RelativeURL )
    }


  internal void ParseLink()
    {
    // This link has semi-colons and everything:
    // http://www.durangoherald.com/article/20160317/ARTS01/160319611/0/Arts/&#x2018;Legally-Blonde-the-Musical&#x2019;-better-than-the-book-or-movie

    // Arts/&#x2018;Legally-Blonde-the-Musical&#x2019;-better-than-the-book-or-movie

    try
    {
    if( GetCallingPage().GetIsCancelled())
      return;

    string FullText = GetMainText();
    // GetCallingPage().AddStatusString( "Doing ParseLink with:", 500 );
    // GetCallingPage().AddStatusString( FullText, 5000 );

    if( FullText.EndsWith( ">" ))                              // </a>
      FullText = Utility.TruncateString( FullText, FullText.Length - 4 );

    if( FullText.Length < 10 )
      return;

    if( LinkStringContainsBadStuff( FullText ))
      {
      // GetCallingPage().AddStatusString( "Link has bad stuff.", 5000 );
      return;
      }

    if( FullText.Contains( "<span" ))
      {
      // This has a span tag:
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

      // ( "The img tag was removed: " + InString );
      }

    string[] LinkParts = FullText.Split( new Char[] { '>' } );
    if( LinkParts.Length < 2 )
      {
      GetCallingPage().AddStatusString( "LinkParts.Length < 2: " + FullText, 500 );
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

   if( Title.ToLower() == "...read more" )
      return;

    // ==========
    // Title = CleanAndSimplify.SimplifyCharacterCodes( Title );

    if( !Attributes.ToLower().Contains( "href=" ))
      {
      // GetCallingPage().AddStatusString(  "No link in: " + FullText, 500 );
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
      // "Pbcs.dll: " + InString );
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
      GetCallingPage().AddStatusString( "AttribParts.Length < 1: " + Attributes, 500 );
      return;
      }

    string LinkURL = AttribParts[0].Trim();

    if( LinkURL == "/" )
      {
      //  "Ignoring the main page for parsing." );
      return;
      }

    if( !(LinkURL.StartsWith( "http://" ) ||
          LinkURL.StartsWith( "https://" )))
      LinkURL = GetRelativeURLBase() + LinkURL;
      // LinkURL = "http://www.durangoherald.com" + LinkURL;

    if( !LinkIsGood( LinkURL, Title, GetRelativeURLBase() ))
      {
      // GetCallingPage().AddStatusString( "Not using URL: " + LinkURL, 500 );
      return;
      }

    /*
    if( InString.Contains( "Caption: class=\"caption" ))
      {
      GetCallingPage().AddStatusString( " ", 10 );
      GetCallingPage().AddStatusString( "Title: " + Title, 500 );
      GetCallingPage().AddStatusString( "LinkURL: " + LinkURL, 500 );
      } */

    // Add it if it's not already there.
    GetCallingPage().AddLink( Title, LinkURL );

    }
    catch( Exception Except )
      {
      GetCallingPage().AddStatusString( "Exception in LinkTag.ParseLink().", 500 );
      GetCallingPage().AddStatusString( Except.Message, 500 );
      }
    }



  private bool LinkStringContainsBadStuff( string InString )
    {
    InString = InString.ToLower();

    if( InString.Contains( "ctl09_rdpdatepicker" ))
      return true;

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



  private bool LinkIsGood( string URL, string Title, string BaseURL )
    {
    string TestURL = URL.ToLower();
    Title = Title.ToLower();

    /*
    if( TestURL.Contains( "www.durangotelegraph" ))
      {
      if( !TestURL.Contains( ".cfm" ))
        {
        GetCallingPage().AddStatusString( " ", 100 );
        GetCallingPage().AddStatusString( "Telegraph non cfm page: " + Title, 500 );
        GetCallingPage().AddStatusString( "URL: " + URL, 500 );
        }
      } */

    if( LinkIsBad( TestURL, Title, BaseURL ))
      return false;

    // Check to see if it's bad first.

    // Limit the scope of this project to only certain
    // domain names.

    // if( TestURL.StartsWith( "https://www.colorado.gov/" ))
      // return true;

    if( TestURL.StartsWith( "http://www.durangogov.org/" ))
      return true;

    if( TestURL.StartsWith( "http://www.durangoherald.com/" ))
      return true;

    if( TestURL.StartsWith( "http://obituaries.durangoherald.com/" ))
      return true;

    if( TestURL.StartsWith( "http://www.durangotelegraph.com/" ))
      return true;

    return false;
    }



  internal static bool LinkIsBad( string URL, string Title, string BaseURL )
    {
    string TestURL = URL.ToLower();
    Title = Title.ToLower();

    if( TestURL.EndsWith( ".pdf" ))
      return true;

    if( TestURL.EndsWith( ".exe" ))
      return true;

    if( TestURL.EndsWith( ".bat" ))
      return true;

    // Part of Google's early success was because of
    // their algorithm that showed that a site was
    // popular if a lot of other site linked to it.
    // But for now this is ignoring links from one
    // domain to another.

    // By the way, cross-site scripting is a common
    // security problem.

    // Don't do cross-site linking for now, like
    // from Durango city gov to Durango Herald or 
    // Telegraph.
    // The Durango Gov page links to a page on the Herald
    // or the Telegraph (the full link, not a relative
    // link) but then that page has relative
    // links in it like /section/COLUMNISTS07/something
    // and so it applies the base URL of DurangoGov.org
    // to make it:
    // DurangoGov.org/section/COLUMNISTS07/something
    // So don't get that full-link page in the first
    // place.  Then it won't get the wrong relative
    // links from that.

    if( !TestURL.ToLower().StartsWith( BaseURL ))
      return true;

    /*
    // All index.cfm?
    // If it's not .cfm is it on the Telegraph?
    // Does the Telegraph use a Cold Fusion (.cfm) server?
    if( TestURL.Contains( "www.durangogov.org/index.cfm/" ))
      return true;

    // Is this on the Telegraph?
    if( TestURL.Contains( "http://www.durangotelegraph.com/section/" ))
      return true;

    // And this?
    if( TestURL.Contains( "http://www.durangotelegraph.com/article/" ))
      return true;

    // if( TestURL.Contains( "www.durangogov.org/index.cfm/archives/" ))
      // return true;

    // www.durangogov.org/section/COLUMNISTS07
    if( TestURL.Contains( "www.durangogov.org/section/" ))
      return true;

    // News articles from the Herald.
    if( TestURL.Contains( "www.durangogov.org/article/" ))
      return true;

    if( TestURL.Contains( "www.durangogov.org/archives/" ))
      return true;

    if( TestURL.Contains( "www.durangogov.org/contact/" ))
      return true;

    // News archives or archived civic alerts?
    if( TestURL.Contains( "www.durangogov.org/civicalerts.aspx?arc=" ))
      return true;

    */

    // if( TestURL.Contains( "/sitemap.aspx" ))
      // return true;

    if( TestURL.Contains( "/rss.aspx" ))
      return true;

    if( TestURL.Contains( "/myaccount.aspx?" ))
      return true;

    if( TestURL.Contains( "/myaccount?" ))
      return true;

    if( TestURL.Contains( "durangoherald.com/#tab" ))
      return true;

    if( TestURL.Contains( "/rss/" ))
      return true;

    if( TestURL.Contains( "/msxml2.xmlhttp/" ))
      return true;

    if( TestURL.Contains( "/taxonomy/" ))
      return true;

    if( TestURL.Contains( "/frontpage/" ))
      return true;

    if( TestURL == "http://www.durangoherald.com/frontpage" )
      return true;

    if( TestURL == "http://www.durangogov.org/frontpage" )
      return true;

    if( Title == "read more" )
      return true;

    if( TestURL.Contains( "/section/maps" ))
      return true;

    return false;
    }


  }
}


