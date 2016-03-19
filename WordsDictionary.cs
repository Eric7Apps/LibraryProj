// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace DGOLibrary
{
  class WordsDictionary
  {
  private MainForm MForm;
  private SortedDictionary<string, int> MainWordsDictionary;
  private SortedDictionary<string, int> ExcludedWordsDictionary;
  private SortedDictionary<string, string> ReplaceWordsDictionary;
  private string FileName = "";



  private WordsDictionary()
    {
    }



  internal WordsDictionary( MainForm UseForm )
    {
    MForm = UseForm;

    FileName = MForm.GetDataDirectory() + "WordsDictionary.txt";

    MainWordsDictionary = new SortedDictionary<string, int>();
    ExcludedWordsDictionary = new SortedDictionary<string, int>();
    ReplaceWordsDictionary = new SortedDictionary<string, string>();

    AddExcludedWords();
    AddCommonReplacements();
    }


  internal void IncrementWordCount( string InWord )
    {
    if( !MainWordsDictionary.ContainsKey( InWord ))
      return;

    MainWordsDictionary[InWord] += 1;
    }



  internal string GetValidWordForm( string InWord )
    {
    try
    {
    if( InWord == null )
      return "";

    InWord = InWord.Trim();

    // as, at, be, by, do, go, he, in, is, it, no,
    // of, on, or, so, to, tv, up, us...
    if( InWord.Length < 3 )
      return "";

    if( !Char.IsLetter( InWord[0] ))
      return "";

    InWord = InWord.ToLower();
    if( WordIsExcluded( InWord ))
      return "";

    if( MainWordsDictionary.ContainsKey( InWord ))
      return InWord;

    // It doesn't get here if it's in the main dictionary.
    InWord = ReplaceCommonWordForms( InWord );
    if( MainWordsDictionary.ContainsKey( InWord ))
      return InWord;

    string FixedWord = FixSuffix( InWord );
    if( FixedWord.Length != 0 )
      {
      // MForm.ShowStatus( "To: " + FixedWord );
      return FixedWord;
      }

    MForm.ShowStatus( "Not a valid word form: " + InWord );
    return "";

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetValidWordForm():" );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  private string FixSuffix( string InWord )
    {
    int Test = 1;
    try
    {
    string TestWord = "";

    ///////////////
    if( InWord.Length >= 4 )
      {
      if( InWord[InWord.Length - 1] == 's' )
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 1 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          return TestWord;

        }

      if( InWord[InWord.Length - 1] == 'r' )
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 1 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          return TestWord;

        }

      if( InWord[InWord.Length - 1] == 'd' )
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 1 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          return TestWord;

        }

      if( InWord[InWord.Length - 1] == 'n' )
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 1 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          return TestWord;

        }

      if( InWord[InWord.Length - 1] == 'y' )
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 1 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          return TestWord;

        }
      }

    Test = 2;
    ///////////////
    if( InWord.Length >= 5 )
      {
      if( (InWord[InWord.Length - 2] == 'e' ) &&
          (InWord[InWord.Length - 1] == 'd' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }

        if( TestWord.Length >= 4 )
          {
          char LastLetter = TestWord[TestWord.Length - 1];
          if( (TestWord[TestWord.Length - 2] == LastLetter )) // &&
              // (TestWord[TestWord.Length - 1] == 'n' ))
            {
            TestWord = Utility.TruncateString( TestWord, TestWord.Length - 1 );
            if( MainWordsDictionary.ContainsKey( TestWord ))
              {
              // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
              return TestWord;
              }
            }
          }
        }

      if( (InWord[InWord.Length - 2] == 'e' ) &&
          (InWord[InWord.Length - 1] == 'r' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }

      if( (InWord[InWord.Length - 2] == 'e' ) &&
          (InWord[InWord.Length - 1] == 's' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }

      if( (InWord[InWord.Length - 2] == 's' ) &&
          (InWord[InWord.Length - 1] == 't' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }

      if( (InWord[InWord.Length - 2] == 'a' ) &&
          (InWord[InWord.Length - 1] == 'l' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }

      if( (InWord[InWord.Length - 2] == 'i' ) &&
          (InWord[InWord.Length - 1] == 'c' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }

      if( (InWord[InWord.Length - 2] == 'l' ) &&
          (InWord[InWord.Length - 1] == 'y' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }
      }


    Test = 3;
    ///////////////
    if( InWord.Length >= 6 )
      {
      if( (InWord[InWord.Length - 3] == 'i' ) &&
          (InWord[InWord.Length - 2] == 'n' ) &&
          (InWord[InWord.Length - 1] == 'g' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }

        string ETest = TestWord + "e";
        if( MainWordsDictionary.ContainsKey( ETest ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
          return ETest;
          }

        if( TestWord.Length >= 4 )
          {
          char LastLetter = TestWord[TestWord.Length - 1];
          if( (TestWord[TestWord.Length - 2] == LastLetter )) // &&
          //    (TestWord[TestWord.Length - 1] == LastLetter ))
            {
            TestWord = Utility.TruncateString( TestWord, TestWord.Length - 1 );
            if( MainWordsDictionary.ContainsKey( TestWord ))
              {
              // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
              return TestWord;
              }
            }
          }
        }


      if( (InWord[InWord.Length - 3] == 'e' ) &&
          (InWord[InWord.Length - 2] == 's' ) &&
          (InWord[InWord.Length - 1] == 't' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }

        /*
        string ETest = TestWord + "e";
        if( MainWordsDictionary.ContainsKey( ETest ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
          return ETest;
          }
          */

        if( TestWord.Length >= 4 )
          {
          if( (TestWord[TestWord.Length - 2] == 'g' ) &&
              (TestWord[TestWord.Length - 1] == 'g' ))
            {
            TestWord = Utility.TruncateString( TestWord, TestWord.Length - 1 );
            if( MainWordsDictionary.ContainsKey( TestWord ))
              {
              MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
              return TestWord;
              }
            }
          }
        }


      if( (InWord[InWord.Length - 3] == 'i' ) &&
          (InWord[InWord.Length - 2] == 'e' ) &&
          (InWord[InWord.Length - 1] == 's' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );

        TestWord += "y";
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }

      if( (InWord[InWord.Length - 3] == 'i' ) &&
          (InWord[InWord.Length - 2] == 'e' ) &&
          (InWord[InWord.Length - 1] == 'r' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );

        TestWord += "y";
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }



      if( (InWord[InWord.Length - 3] == 'i' ) &&
          (InWord[InWord.Length - 2] == 'e' ) &&
          (InWord[InWord.Length - 1] == 'd' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );

        TestWord += "y";
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }

      if( (InWord[InWord.Length - 3] == 'i' ) &&
          (InWord[InWord.Length - 2] == 'o' ) &&
          (InWord[InWord.Length - 1] == 'n' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }

        string ETest = TestWord + "e";
        if( MainWordsDictionary.ContainsKey( ETest ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
          return ETest;
          }
        }

      if( (InWord[InWord.Length - 3] == 'i' ) &&
          (InWord[InWord.Length - 2] == 'a' ) &&
          (InWord[InWord.Length - 1] == 'l' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }

        string ETest = TestWord + "e";
        if( MainWordsDictionary.ContainsKey( ETest ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
          return ETest;
          }
        }

      if( (InWord[InWord.Length - 3] == 'i' ) &&
          (InWord[InWord.Length - 2] == 't' ) &&
          (InWord[InWord.Length - 1] == 'y' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }

        string ETest = TestWord + "e";
        if( MainWordsDictionary.ContainsKey( ETest ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
          return ETest;
          }
        }

      if( (InWord[InWord.Length - 3] == 'i' ) &&
          (InWord[InWord.Length - 2] == 's' ) &&
          (InWord[InWord.Length - 1] == 'm' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }

      if( (InWord[InWord.Length - 3] == 'i' ) &&
          (InWord[InWord.Length - 2] == 's' ) &&
          (InWord[InWord.Length - 1] == 't' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }

      }


    Test = 4;
    ///////////////
    if( InWord.Length >= 7 )
      {
      if( (InWord[InWord.Length - 4] == 'n' ) &&
          (InWord[InWord.Length - 3] == 'e' ) &&
          (InWord[InWord.Length - 2] == 's' ) &&
          (InWord[InWord.Length - 1] == 's' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 4 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }
        }

      if( (InWord[InWord.Length - 4] == 't' ) &&
          (InWord[InWord.Length - 3] == 'i' ) &&
          (InWord[InWord.Length - 2] == 'o' ) &&
          (InWord[InWord.Length - 1] == 'n' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 4 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
          return TestWord;
          }

        string ETest = TestWord + "e";
        if( MainWordsDictionary.ContainsKey( ETest ))
          {
          // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
          return ETest;
          }
        }

      }


    return "";
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetValidWordForm():" );
      MForm.ShowStatus( "Test is: " + Test.ToString() );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  private bool WordIsExcluded( string InWord )
    {
    // The WordsDictionary.txt file is a white-list of
    // words that will be indexed.  This black-list of
    // words that will not be indexed is only needed for
    // efficiency (to quickly get rid of words like
    // 'the'), and also because when you're looking for
    // new words that should be added to the dictionary
    // file it helps when it's not showing so much garbage.
    // But there are two reasons for garbage being there.
    // One is a crude form of parsing and the other is
    // mal-formed HTML.  Even with very good parsing you
    // can't depend on well-formed HTML.  I mean you never
    // know what new form of parsing you'll have to come
    // up with to deal with a new problem.
    if( InWord == "the" )
      return true;

    if( InWord == "and" )
      return true;

    if( InWord == "not" )
      return true;

    if( ExcludedWordsDictionary.ContainsKey( InWord ))
      return true;

    for( int Count = 1; Count < InWord.Length; Count++ )
      {
      if( !Char.IsLetter( InWord[Count] ))
        return true;

      }

    return false;
    }



  private void AddExcludedWords()
    {
    // This needs to go in a file.

    ExcludedWordsDictionary["aboutus"] = 0;
    ExcludedWordsDictionary["ads"] = 0;
    ExcludedWordsDictionary["alt"] = 0;
    ExcludedWordsDictionary["amp"] = 0;
    ExcludedWordsDictionary["animasriverminewaste"] = 0;
    ExcludedWordsDictionary["anti"] = 0;
    ExcludedWordsDictionary["api"] = 0; // For now.
    ExcludedWordsDictionary["apw"] = 0;
    ExcludedWordsDictionary["are"] = 0;
    ExcludedWordsDictionary["artno"] = 0;
    ExcludedWordsDictionary["aspx"] = 0;
    ExcludedWordsDictionary["ats"] = 0;
    ExcludedWordsDictionary["authenticateusersubscription"] = 0;

    ExcludedWordsDictionary["ballantinecommunications"] = 0;
    ExcludedWordsDictionary["bci"] = 0;
    ExcludedWordsDictionary["bcimedia"] = 0;
    ExcludedWordsDictionary["bdmedia"] = 0;

    ExcludedWordsDictionary["beforeafter"] = 0;

    ExcludedWordsDictionary["chsaa"] = 0;
    ExcludedWordsDictionary["clickshare"] = 0;
    ExcludedWordsDictionary["col"] = 0;
    ExcludedWordsDictionary["coloradosbdc"] = 0;
    ExcludedWordsDictionary["com"] = 0;
    ExcludedWordsDictionary["cotrip"] = 0;
    ExcludedWordsDictionary["csloginonly"] = 0;
    ExcludedWordsDictionary["cstargeturl"] = 0;
    ExcludedWordsDictionary["cws"] = 0;

    ExcludedWordsDictionary["del"] = 0;
    ExcludedWordsDictionary["dema"] = 0;
    ExcludedWordsDictionary["dgo"] = 0;
    ExcludedWordsDictionary["dgomag"] = 0;
    ExcludedWordsDictionary["dhcams"] = 0;
    ExcludedWordsDictionary["dimages"] = 0;
    ExcludedWordsDictionary["directoryplus"] = 0;
    ExcludedWordsDictionary["did"] = 0;
    ExcludedWordsDictionary["didn"] = 0;      // 't
    ExcludedWordsDictionary["dll"] = 0;
    ExcludedWordsDictionary["docs"] = 0;
    ExcludedWordsDictionary["doradomagazine"] = 0;
    ExcludedWordsDictionary["durangoherald"] = 0;

    ExcludedWordsDictionary["eco"] = 0;
    ExcludedWordsDictionary["eherald"] = 0;
    ExcludedWordsDictionary["emplo"] = 0;
    ExcludedWordsDictionary["exacth"] = 0;
    ExcludedWordsDictionary["exactw"] = 0;

    ExcludedWordsDictionary["firstmmedia"] = 0;
    ExcludedWordsDictionary["footer"] = 0; // For now.
    ExcludedWordsDictionary["fourcornersexpos"] = 0;
    ExcludedWordsDictionary["fourcornersmarketplace"] = 0;
    ExcludedWordsDictionary["fourcornersschoolpubs"] = 0;
    ExcludedWordsDictionary["frameborder"] = 0;
    ExcludedWordsDictionary["frontpage"] = 0;

    ExcludedWordsDictionary["gdr"] = 0;
    ExcludedWordsDictionary["getting"] = 0;
    ExcludedWordsDictionary["gif"] = 0;
    ExcludedWordsDictionary["goes"] = 0;
    ExcludedWordsDictionary["going"] = 0;
    ExcludedWordsDictionary["gooddirtradio"] = 0;
    ExcludedWordsDictionary["gov"] = 0;

    ExcludedWordsDictionary["header"] = 0;
    ExcludedWordsDictionary["headuserlogin"] = 0;
    ExcludedWordsDictionary["href"] = 0;
    ExcludedWordsDictionary["html"] = 0;
    ExcludedWordsDictionary["http"] = 0;
    ExcludedWordsDictionary["https"] = 0;

    ExcludedWordsDictionary["iframe"] = 0;
    ExcludedWordsDictionary["imageversion"] = 0;
    ExcludedWordsDictionary["img"] = 0;
    ExcludedWordsDictionary["inc"] = 0;
    ExcludedWordsDictionary["issuu"] = 0;
    ExcludedWordsDictionary["its"] = 0;

    ExcludedWordsDictionary["jobsearch"] = 0;
    ExcludedWordsDictionary["jpg"] = 0;

    ExcludedWordsDictionary["lede"] = 0;
    ExcludedWordsDictionary["letterstotheeditordh"] = 0;
    ExcludedWordsDictionary["linktarget"] = 0;
    ExcludedWordsDictionary["login"] = 0;     // For now.
    ExcludedWordsDictionary["los"] = 0;

    ExcludedWordsDictionary["macro"] = 0;
    ExcludedWordsDictionary["marginheight"] = 0;
    ExcludedWordsDictionary["marginwidth"] = 0;
    ExcludedWordsDictionary["maxh"] = 0;
    ExcludedWordsDictionary["mdash"] = 0;
    ExcludedWordsDictionary["mejia"] = 0; // media?

    ExcludedWordsDictionary["nav"] = 0;
    ExcludedWordsDictionary["navbar"] = 0;
    ExcludedWordsDictionary["nbsp"] = 0;
    ExcludedWordsDictionary["newsstand"] = 0;
    ExcludedWordsDictionary["newstip"] = 0;
    ExcludedWordsDictionary["newtbl"] = 0;
    ExcludedWordsDictionary["nofollow"] = 0;
    ExcludedWordsDictionary["nskc"] = 0;

    ExcludedWordsDictionary["ogp"] = 0;
    ExcludedWordsDictionary["opengraphprotocol"] = 0;
    ExcludedWordsDictionary["org"] = 0;

    ExcludedWordsDictionary["pbcs"] = 0;
    ExcludedWordsDictionary["pbcsi"] = 0;
    ExcludedWordsDictionary["php"] = 0;
    ExcludedWordsDictionary["pinerivertimes"] = 0;
    ExcludedWordsDictionary["png"] = 0;
    ExcludedWordsDictionary["popups"] = 0;
    ExcludedWordsDictionary["premiumpopup"] = 0;
    ExcludedWordsDictionary["preview1"] = 0;
    ExcludedWordsDictionary["preview2"] = 0;
    ExcludedWordsDictionary["privacypolicy"] = 0;

    ExcludedWordsDictionary["realestate"] = 0;
    ExcludedWordsDictionary["ref"] = 0;
    ExcludedWordsDictionary["reg"] = 0;
    ExcludedWordsDictionary["rel"] = 0;
    ExcludedWordsDictionary["rss"] = 0;

    ExcludedWordsDictionary["san"] = 0;
    ExcludedWordsDictionary["schema"] = 0;
    ExcludedWordsDictionary["secondstreetapp"] = 0;
    ExcludedWordsDictionary["sct"] = 0;
    ExcludedWordsDictionary["searchresults"] = 0;
    ExcludedWordsDictionary["sen"] = 0;
    ExcludedWordsDictionary["src"] = 0;
    ExcludedWordsDictionary["sta"] = 0;
    ExcludedWordsDictionary["storyimage"] = 0;
    ExcludedWordsDictionary["swscene"] = 0;
    ExcludedWordsDictionary["subscriptioncenter"] = 0;
    ExcludedWordsDictionary["subsection"] = 0;
    ExcludedWordsDictionary["sug"] = 0;

    ExcludedWordsDictionary["tabmmedia"] = 0;
    ExcludedWordsDictionary["taleo"] = 0;
    ExcludedWordsDictionary["tbe"] = 0;
    ExcludedWordsDictionary["temp"] = 0;
    ExcludedWordsDictionary["termsofuse"] = 0;
    ExcludedWordsDictionary["that"] = 0;
    ExcludedWordsDictionary["thecloudscout"] = 0;
    ExcludedWordsDictionary["thedurangoherald"] = 0;
    ExcludedWordsDictionary["thedurangoheraldsmallpress"] = 0;
    ExcludedWordsDictionary["their"] = 0;
    ExcludedWordsDictionary["them"] = 0;
    ExcludedWordsDictionary["then"] = 0;
    ExcludedWordsDictionary["there"] = 0;
    ExcludedWordsDictionary["these"] = 0;
    ExcludedWordsDictionary["they"] = 0;
    ExcludedWordsDictionary["this"] = 0;
    ExcludedWordsDictionary["those"] = 0;
    ExcludedWordsDictionary["thus"] = 0;
    ExcludedWordsDictionary["timestamp"] = 0;
    ExcludedWordsDictionary["too"] = 0;
    ExcludedWordsDictionary["tosa"] = 0;
    ExcludedWordsDictionary["tvs"] = 0;

    ExcludedWordsDictionary["using"] = 0;

    ExcludedWordsDictionary["was"] = 0;
    ExcludedWordsDictionary["weekender"] = 0;
    ExcludedWordsDictionary["went"] = 0;
    ExcludedWordsDictionary["were"] = 0;
    ExcludedWordsDictionary["what"] = 0;
    ExcludedWordsDictionary["widget"] = 0;
    ExcludedWordsDictionary["with"] = 0;
    ExcludedWordsDictionary["wrapper"] = 0;
    ExcludedWordsDictionary["www"] = 0;

    ExcludedWordsDictionary["xcel"] = 0;
    ExcludedWordsDictionary["xhtml"] = 0;
    ExcludedWordsDictionary["xmlns"] = 0;
    }



  private string ReplaceCommonWordForms( string InWord )
    {
    if( !ReplaceWordsDictionary.ContainsKey( InWord ))
      return InWord;

    return ReplaceWordsDictionary[InWord];
    }



  private void AddCommonReplacements()
    {
    ReplaceWordsDictionary["ariz"] = "arizona";
    ReplaceWordsDictionary["colo"] = "colorado";
    ReplaceWordsDictionary["intro"] = "introduction";


    // ReplaceWordsDictionary["jan"] = "january";
    // ReplaceWordsDictionary["feb"] = "february";
    // ReplaceWordsDictionary["mar"] = "march";
    // ReplaceWordsDictionary["apr"] = "april";
    // ReplaceWordsDictionary["may"] = "may";
    // ReplaceWordsDictionary["jun"] = "june";
    // ReplaceWordsDictionary["jul"] = "july";
    // ReplaceWordsDictionary["aug"] = "august";
    ReplaceWordsDictionary["sept"] = "september";
    ReplaceWordsDictionary["oct"] = "october";
    // ReplaceWordsDictionary["nov"] = "november";
    // ReplaceWordsDictionary["dec"] = "december";
    }



  internal bool ReadFromTextFile()
    {
    MainWordsDictionary.Clear();

    if( !File.Exists( FileName ))
      return false;

    try
    {
    string Line;
    string KeyWord = "";
    int CountValue = 0;

    using( StreamReader SReader = new StreamReader( FileName  )) 
      {
      while( SReader.Peek() >= 0 ) 
        {
        Line = SReader.ReadLine();
        if( Line == null )
          continue;

        Line = Line.Trim();
        if( Line == "" )
          continue;

        string[] SplitS = Line.Split( new Char[] { '\t' } );
        if( SplitS.Length < 1 )
          continue;

        if( SplitS.Length < 2 )
          {
          CountValue = 0;
          KeyWord = SplitS[0].Trim();
          }
        else
          {
          KeyWord = SplitS[0].Trim();
          int Test = 0;
          string ValS = SplitS[1].Trim();
          if( ValS.Length > 0 )
            {
            try
            {
            Test = Int32.Parse( ValS );
            }
            catch( Exception )
              {
              Test = 0;
              }
            }

          CountValue = Test;
          }

        if( WordIsExcluded( KeyWord ))
          continue;

        MainWordsDictionary[KeyWord] = CountValue;
        }
      }

    return true;

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the file: \r\n" + FileName );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal bool WriteToTextFile()
    {
    try
    {
    using( StreamWriter SWriter = new StreamWriter( FileName  )) 
      {
      foreach( KeyValuePair<string, int> Kvp in MainWordsDictionary )
        {
        string Line = Kvp.Key + "\t" + Kvp.Value.ToString();
        SWriter.WriteLine( Line );
        }

      SWriter.WriteLine( " " );
      }

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not write the words dictionary data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }




  }
}

