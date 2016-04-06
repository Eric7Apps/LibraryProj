// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Text;

namespace DGOLibrary
{
  static class SuffixForms
  {



  internal static string FixSuffix( string InWord, WordsIndex MainWordsIndex )
    {
    try
    {
    string TestWord = "";

    if( InWord.Length < 4 )
      return InWord;

    if( InWord.Length >= 4 )
      {
      TestWord = FixSuffix1( InWord, MainWordsIndex );
      if( TestWord != "" )
        return TestWord;

      }

    if( InWord.Length >= 5 )
      {
      TestWord = FixSuffix2( InWord, MainWordsIndex );
      if( TestWord != "" )
        return TestWord;

      }

    if( InWord.Length >= 6 )
      {
      TestWord = FixSuffix3( InWord, MainWordsIndex );
      if( TestWord != "" )
        return TestWord;

      }

    if( InWord.Length >= 7 )
      {
      TestWord = FixSuffix4( InWord, MainWordsIndex );
      if( TestWord != "" )
        return TestWord;

      }

    return "";
    }
    catch( Exception Except )
      {
      string ShowS = "Exception in FixSuffix():\r\n" +
        Except.Message;

      throw( new Exception( ShowS ));
      }
    }



  private static string FixSuffix1( string InWord, WordsIndex MainWordsIndex )
    {
    try
    {
    string TestWord = "";

    if( InWord.Length < 4 )
      return "";

    if( InWord[InWord.Length - 1] == 's' )
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 1 );
      if( MainWordsIndex.WordExists( TestWord ))
        return TestWord;

      }

    if( InWord[InWord.Length - 1] == 'r' )
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 1 );
      if( MainWordsIndex.WordExists( TestWord ))
        return TestWord;

      }

    if( InWord[InWord.Length - 1] == 'd' )
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 1 );
      if( MainWordsIndex.WordExists( TestWord ))
        return TestWord;

      }

    if( InWord[InWord.Length - 1] == 'n' )
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 1 );
      if( MainWordsIndex.WordExists( TestWord ))
        return TestWord;

      }

    if( InWord[InWord.Length - 1] == 'y' )
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 1 );
      if( MainWordsIndex.WordExists( TestWord ))
        return TestWord;

      // favorably
      // favorable
      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
        return ETest;

      }

    return "";
    }
    catch( Exception Except )
      {
      string ShowS = "Exception in FixSuffix1():\r\n" +
        Except.Message;

      throw( new Exception( ShowS ));
      }
    }



  private static string FixSuffix2( string InWord, WordsIndex MainWordsIndex )
    {
    try
    {
    string TestWord = "";

    if( InWord.Length < 5 )
      return "";

    if( (InWord[InWord.Length - 2] == 'e' ) &&
        (InWord[InWord.Length - 1] == 'd' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
      if( MainWordsIndex.WordExists( TestWord ))
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
          if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }
      }

    if( (InWord[InWord.Length - 2] == 'o' ) &&
        (InWord[InWord.Length - 1] == 'r' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }
      }

    if( (InWord[InWord.Length - 2] == 'e' ) &&
        (InWord[InWord.Length - 1] == 's' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }
      }

    if( (InWord[InWord.Length - 2] == 's' ) &&
        (InWord[InWord.Length - 1] == 't' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }
      }

    if( (InWord[InWord.Length - 2] == 'a' ) &&
        (InWord[InWord.Length - 1] == 'l' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }
      }

    if( (InWord[InWord.Length - 2] == 'i' ) &&
        (InWord[InWord.Length - 1] == 'c' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "y";
      if( MainWordsIndex.WordExists( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }
      }

    if( (InWord[InWord.Length - 2] == 'l' ) &&
        (InWord[InWord.Length - 1] == 'y' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }
      }

    if( (InWord[InWord.Length - 2] == 'r' ) &&
        (InWord[InWord.Length - 1] == 'y' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 2 );
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }
      }

    return "";
    }
    catch( Exception Except )
      {
      string ShowS = "Exception in FixSuffix2():\r\n" +
        Except.Message;

      throw( new Exception( ShowS ));
      }
    }




  private static string FixSuffix3( string InWord, WordsIndex MainWordsIndex )
    {
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }

      ETest = TestWord + "y";
      if( MainWordsIndex.WordExists( ETest ))
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
          if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      /*
      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
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
          if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "y";
      if( MainWordsIndex.WordExists( ETest ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "y";
      if( MainWordsIndex.WordExists( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }
      }

    if( (InWord[InWord.Length - 3] == 'o' ) &&
        (InWord[InWord.Length - 2] == 'u' ) &&
        (InWord[InWord.Length - 1] == 's' ))
      {
      TestWord = Utility.TruncateString( InWord, InWord.Length - 3 );
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      // polygomy
      // polygomous
      string YTest = TestWord + "y";
      if( MainWordsIndex.WordExists( YTest ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }
 
      ETest = TestWord + "y";
      if( MainWordsIndex.WordExists( ETest ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "y";
      if( MainWordsIndex.WordExists( ETest ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }

      // entomology
      // entomologist
      ETest = TestWord + "y";
      if( MainWordsIndex.WordExists( ETest ))
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
      if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
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
      string ShowS = "Exception in FixSuffix3():\r\n" +
        Except.Message;

      throw( new Exception( ShowS ));
      }
    }



  private static string FixSuffix4( string InWord, WordsIndex MainWordsIndex )
    {
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
      if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "e";
      if( MainWordsIndex.WordExists( ETest ))
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
      if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
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
      if( MainWordsIndex.WordExists( TestWord ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
        return TestWord;
        }

      string ETest = TestWord + "y";
      if( MainWordsIndex.WordExists( ETest ))
        {
        // MForm.ShowStatus( "Changed From: " + InWord + "  To: " + ETest );
        return ETest;
        }
      }

    return "";
    }
    catch( Exception Except )
      {
      string ShowS = "Exception in FixSuffix4():\r\n" +
        Except.Message;

      throw( new Exception( ShowS ));
      }
    }


  }
}
