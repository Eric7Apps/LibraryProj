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
          return -1;
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
    // This should all be running in a background thread,
    // but for now, check the UI thread events.
    if( !MForm.CheckEvents())
      return;

    int StartAt = 0;
    string StartName = "";
    Tag TagToAdd;
    while( true )
      {
      int Index = GetFirstTagIndex( StartAt );
      if( Index < 0 )
        {
        MForm.ShowStatus( "No more tags." );
        return;
        }

      StartAt = Index + 1;
      StartName = GetTagName( StartAt );
      if( StartName.Length < 1 )
        {
        MForm.ShowStatus( "There was no tag name." );
        return;
        }

      MForm.ShowStatus( " " );
      MForm.ShowStatus( " " );
      MForm.ShowStatus( " " );
      MForm.ShowStatus( " " );
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "StartName: " + StartName );
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
          MForm.ShowStatus( "Didn't find the full tag end." );
          continue;
          }

        int SubLength = TestEnd - StartAt;
        NewTagS = MainText.Substring( StartAt, SubLength );
        if( !((StartName == "html" ) ||
              (StartName == "head" ) ||
              (StartName == "body" )))
          MForm.ShowStatus( "NewTagS: " + NewTagS );

        TagToAdd = new Tag( MForm, CallingPage, NewTagS );
        AddContainedTag( TagToAdd );
        TagToAdd.MakeContainedTags();
        StartAt = TestEnd + 1;
        }
      else
        {
        int SubLength = TestEnd - StartAt;
        NewTagS = MainText.Substring( StartAt, SubLength );
        MForm.ShowStatus( "NewTagS: " + NewTagS );
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


  /*
  private void ParseContainedTags()
    {
    try
    {
    if( ContainedTags == null )
      return;

    for( int Count = 0; Count < ContainedTagsLast; Count++ )
      ContainedTags[Count].MakeContainedTags();
  
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in Tag.ParseContainedTags()." );
      MForm.ShowStatus( Except.Message );
      }
    }
    */


  }
}

