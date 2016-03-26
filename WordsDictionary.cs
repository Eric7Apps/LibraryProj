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
  private SortedDictionary<string, string> ReplaceWordsDictionary;
  private SortedDictionary<string, int> ExcludedWordsDictionary;
  private string FileName = "";
  private string ExcludedFileName = "";



  private WordsDictionary()
    {
    }



  internal WordsDictionary( MainForm UseForm )
    {
    MForm = UseForm;

    FileName = MForm.GetDataDirectory() + "WordsDictionary.txt";
    ExcludedFileName = MForm.GetDataDirectory() + "ExcludedWordsDictionary.txt";

    MainWordsDictionary = new SortedDictionary<string, int>();
    ReplaceWordsDictionary = new SortedDictionary<string, string>();
    ExcludedWordsDictionary = new SortedDictionary<string, int>();

    AddCommonReplacements();
    }


  internal void IncrementWordCount( string InWord )
    {
    if( !MainWordsDictionary.ContainsKey( InWord ))
      return;

    MainWordsDictionary[InWord] += 1;
    }



  // This word form thing could get a _lot_ more
  // complicated.
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

    MForm.ShowStatus( "Word? " + InWord );
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

    if( InWord.Length < 4 )
      return InWord;

    if( InWord.Length >= 4 )
      {
      TestWord = FixSuffix1( InWord );
      if( TestWord != "" )
        return TestWord;

      }

    if( InWord.Length >= 5 )
      {
      TestWord = FixSuffix2( InWord );
      if( TestWord != "" )
        return TestWord;

      }

    if( InWord.Length >= 6 )
      {
      TestWord = FixSuffix3( InWord );
      if( TestWord != "" )
        return TestWord;

      }

    if( InWord.Length >= 7 )
      {
      TestWord = FixSuffix4( InWord );
      if( TestWord != "" )
        return TestWord;

      }

    return "";
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in FixSuffix():" );
      MForm.ShowStatus( "Test is: " + Test.ToString() );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }




  private string FixSuffix1( string InWord )
    {
    int Test = 1;
    try
    {
    string TestWord = "";

    if( InWord.Length < 4 )
      return "";

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

      // favorably
      // favorable
      string ETest = TestWord + "e";
      if( MainWordsDictionary.ContainsKey( ETest ))
        return ETest;

      }

    return "";
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in FixSuffix1():" );
      MForm.ShowStatus( "Test is: " + Test.ToString() );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  private string FixSuffix2( string InWord )
    {
    int Test = 1;
    try
    {
    string TestWord = "";

    if( InWord.Length < 5 )
      return "";

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

    if( (InWord[InWord.Length - 2] == 'o' ) &&
        (InWord[InWord.Length - 1] == 'r' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
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

      string ETest = TestWord + "e";
      if( MainWordsDictionary.ContainsKey( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
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

      string ETest = TestWord + "y";
      if( MainWordsDictionary.ContainsKey( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
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

    if( (InWord[InWord.Length - 2] == 'r' ) &&
        (InWord[InWord.Length - 1] == 'y' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
      if( MainWordsDictionary.ContainsKey( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }
      }

    return "";
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in FixSuffix2():" );
      MForm.ShowStatus( "Test is: " + Test.ToString() );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }




  private string FixSuffix3( string InWord )
    {
    int Test = 1;
    try
    {
    string TestWord = "";

    if( InWord.Length < 6 )
      return InWord;

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
            // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
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
      if( MainWordsDictionary.ContainsKey( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "y";
      if( MainWordsDictionary.ContainsKey( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return ETest;
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
        (InWord[InWord.Length - 1] == 'n' ))
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
        }*/
      }

    if( (InWord[InWord.Length - 3] == 'o' ) &&
        (InWord[InWord.Length - 2] == 'u' ) &&
        (InWord[InWord.Length - 1] == 's' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
      if( MainWordsDictionary.ContainsKey( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      // polygomy
      // polygomous
      string YTest = TestWord + "y";
      if( MainWordsDictionary.ContainsKey( YTest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + YTest );
        return YTest;
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
 
      ETest = TestWord + "y";
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
        (InWord[InWord.Length - 2] == 'l' ) &&
        (InWord[InWord.Length - 1] == 'y' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
      if( MainWordsDictionary.ContainsKey( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "y";
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

      string ETest = TestWord + "e";
      if( MainWordsDictionary.ContainsKey( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }
      }

    if( (InWord[InWord.Length - 3] == 'i' ) &&
        (InWord[InWord.Length - 2] == 'v' ) &&
        (InWord[InWord.Length - 1] == 'e' ))
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
        (InWord[InWord.Length - 1] == 't' ))
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

      // entomology
      // entomologist
      ETest = TestWord + "y";
      if( MainWordsDictionary.ContainsKey( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }

      }

    if( (InWord[InWord.Length - 3] == 'i' ) &&
        (InWord[InWord.Length - 2] == 's' ) &&
        (InWord[InWord.Length - 1] == 'h' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
      if( MainWordsDictionary.ContainsKey( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }
      }

    if( (InWord[InWord.Length - 3] == 'a' ) &&
        (InWord[InWord.Length - 2] == 'r' ) &&
        (InWord[InWord.Length - 1] == 'y' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
      if( MainWordsDictionary.ContainsKey( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      /*
      ETest = TestWord + "y";
      if( MainWordsDictionary.ContainsKey( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }
        */
      }

    return "";
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in FixSuffix3():" );
      MForm.ShowStatus( "Test is: " + Test.ToString() );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }


  private string FixSuffix4( string InWord )
    {
    int Test = 1;
    try
    {
    string TestWord = "";

    if( InWord.Length < 7 )
      return InWord;

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

    if( (InWord[InWord.Length - 4] == 'i' ) &&
        (InWord[InWord.Length - 3] == 'n' ) &&
        (InWord[InWord.Length - 2] == 'g' ) &&
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

    // enlargement
    if( (InWord[InWord.Length - 4] == 'm' ) &&
        (InWord[InWord.Length - 3] == 'e' ) &&
        (InWord[InWord.Length - 2] == 'n' ) &&
        (InWord[InWord.Length - 1] == 't' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 4 );
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
      }

    if( (InWord[InWord.Length - 4] == 'a' ) &&
        (InWord[InWord.Length - 3] == 'l' ) &&
        (InWord[InWord.Length - 2] == 'l' ) &&
        (InWord[InWord.Length - 1] == 'y' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 4 );
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
      }

    if( (InWord[InWord.Length - 4] == 'i' ) &&
        (InWord[InWord.Length - 3] == 'a' ) &&
        (InWord[InWord.Length - 2] == 'n' ) &&
        (InWord[InWord.Length - 1] == 's' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 4 );
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
      }

    if( (InWord[InWord.Length - 4] == 'i' ) &&
        (InWord[InWord.Length - 3] == 'e' ) &&
        (InWord[InWord.Length - 2] == 's' ) &&
        (InWord[InWord.Length - 1] == 't' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 4 );
      if( MainWordsDictionary.ContainsKey( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "y";
      if( MainWordsDictionary.ContainsKey( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }
      }

    return "";
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in FixSuffix4():" );
      MForm.ShowStatus( "Test is: " + Test.ToString() );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }




  private bool WordIsExcluded( string Word )
    {
    // This is mainly needed to help find new words
    // that aren't yet in any dictionary.

    if( Word == null ) // If it ever got a null.
      return true;

    if( Word.Length < 3 )
      return true;

    if( ExcludedWordsDictionary.ContainsKey( Word ))
      return true;

    for( int Count = 0; Count < Word.Length; Count++ )
      {
      if( !Char.IsLetter( Word[Count] ))
        return true;

      }

    return false;
    }




  // See the note above about how this is used.  It has
  // already been checked against the main words dictionary
  // and it's not in that.  So it's not replacing part
  // of a good word with something else.
  private string ReplaceCommonWordForms( string InWord )
    {
    if( !ReplaceWordsDictionary.ContainsKey( InWord ))
      return InWord;

    return ReplaceWordsDictionary[InWord];
    }



  private void AddCommonReplacements()
    {
    ReplaceWordsDictionary["ariz"] = "arizona";
    ReplaceWordsDictionary["beyonc"] = "beyonce";
    ReplaceWordsDictionary["broomfiled"] = "broomfield";
    ReplaceWordsDictionary["brucoli"] = "brocoli";
    ReplaceWordsDictionary["calif"] = "california";
    ReplaceWordsDictionary["cinderela"] = "cinderella";
    ReplaceWordsDictionary["colo"] = "colorado";
    ReplaceWordsDictionary["conn"] = "connecticut";
    ReplaceWordsDictionary["dicapio"] = "dicaprio";
    ReplaceWordsDictionary["downton"] = "downtown";
    ReplaceWordsDictionary["febuary"] = "february";
    ReplaceWordsDictionary["fla"] = "florida";
    ReplaceWordsDictionary["inacio"] = "ignacio";
    ReplaceWordsDictionary["intro"] = "introduction";
    ReplaceWordsDictionary["mangement"] = "management";
    ReplaceWordsDictionary["margaritis"] ="margaritas";
    ReplaceWordsDictionary["outsude"] = "outside";
    ReplaceWordsDictionary["participaes"] = "participates";
    ReplaceWordsDictionary["recidivisim"] = "recidivism";

    ReplaceWordsDictionary["slowin"] = "slowing";
    ReplaceWordsDictionary["tardition"] = "tradition";
    ReplaceWordsDictionary["thinkin"] = "think";
    ReplaceWordsDictionary["wearin"] = "wearing";

    // ReplaceWordsDictionary["jan"] = "january";
    ReplaceWordsDictionary["feb"] = "february";
    // ReplaceWordsDictionary["mar"] = "march";
    // ReplaceWordsDictionary["apr"] = "april";
    // ReplaceWordsDictionary["may"] = "may";
    // ReplaceWordsDictionary["jun"] = "june";
    // ReplaceWordsDictionary["jul"] = "july";
    ReplaceWordsDictionary["aug"] = "august";
    ReplaceWordsDictionary["sept"] = "september";
    ReplaceWordsDictionary["oct"] = "october";
    ReplaceWordsDictionary["nov"] = "november";
    // ReplaceWordsDictionary["dec"] = "december";
    }


  internal string FixUserInput( string UserInput )
    {
    // CleanUnicode ...
    // ==========
    return "";
    }




  internal string ReplaceForSplitWords( string InString )
    {
    // If you were searching for 'book' you'd find
    // 'book' but not 'bookstore' or 'bookshop'.

    string Result = InString;

    // Put these in a file?

    Result = Result.Replace( "bookstore", "book store" );
    Result = Result.Replace( "bookshop", "book shop" );
    Result = Result.Replace( "colostate", "colorado state" );
    Result = Result.Replace( "puertorico", "puerto rico" );
    Result = Result.Replace( "ragtimefestival", "ragtime festival" );
    Result = Result.Replace( "realestate", "real estate" );
    Result = Result.Replace( "realproperty", "real property" );
    Result = Result.Replace( "runningclub", "running club" );
    Result = Result.Replace( "worksite", "work site" );

    return Result;
    }



  internal bool ReadFromTextFile()
    {
    MainWordsDictionary.Clear();

    if( !File.Exists( FileName ))
      return false;

    try
    {
    // It has to read this first.
    ReadExcludedFromTextFile();

    // Write them back sorted and unique.
    WriteToExcludedWordsTextFile();

    string Line;
    string KeyWord = "";
    int CountValue = 0;

    // I would rather not post all of the words in the
    // text file where I've collected non-G-rated words
    // along with all the others.  (They are collected
    // from newspaper articles.)
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

        // For testing set it back to zero:
        // CountValue = 0;
        MainWordsDictionary[KeyWord] = CountValue;
        }
      }

    ShowMostFrequentWords();

    MForm.ShowStatus( " " );
    MForm.ShowStatus( "Words Count: " + MainWordsDictionary.Count.ToString( "N0" ));
    MForm.ShowStatus( " " );

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




  private bool ReadExcludedFromTextFile()
    {
    ExcludedWordsDictionary.Clear();

    if( !File.Exists( ExcludedFileName ))
      return false;

    try
    {
    string Line;
    string KeyWord = "";
    int CountValue = 0;

    using( StreamReader SReader = new StreamReader( ExcludedFileName )) 
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

        // This is the excluded words:
        // CountValue = 0;
        ExcludedWordsDictionary[KeyWord] = CountValue;
        }
      }

    MForm.ShowStatus( " " );
    MForm.ShowStatus( "Excluded words Count: " + ExcludedWordsDictionary.Count.ToString( "N0" ));
    MForm.ShowStatus( " " );

    return true;

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Could not read the file: \r\n" + FileName );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal bool WriteToExcludedWordsTextFile()
    {
    try
    {
    using( StreamWriter SWriter = new StreamWriter( ExcludedFileName  )) 
      {
      foreach( KeyValuePair<string, int> Kvp in ExcludedWordsDictionary )
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
      MForm.ShowStatus( "Could not write the excluded words dictionary data to the file." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }




  internal void ShowMostFrequentWords()
    {
    try
    {
    MForm.ShowStatus( " " );
    MForm.ShowStatus( "Most frequent words:" );

    // Higher numbered counts are usually unique, so 
    // this works as a way to find words like 'the' that
    // should not be indexed.  (As opposed to sorting
    // them all uniquely.)
    SortedDictionary<int, string> FrequentDictionary = new SortedDictionary<int, string>();

    foreach( KeyValuePair<string, int> Kvp in MainWordsDictionary )
      {
      if( Kvp.Value < 1000 )
        continue;

      // If it's not unique this gets the last one.
      FrequentDictionary[Kvp.Value] = Kvp.Key;
      }

    foreach( KeyValuePair<int, string> KvpCount in FrequentDictionary )
      {
      MForm.ShowStatus( KvpCount.Key.ToString() + ") " + KvpCount.Value );
      }
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in ShowMostFrequentWords()." );
      MForm.ShowStatus( Except.Message );
      }
    }



  }
}

