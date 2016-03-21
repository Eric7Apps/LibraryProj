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
  private string FileName = "";



  private WordsDictionary()
    {
    }



  internal WordsDictionary( MainForm UseForm )
    {
    MForm = UseForm;

    FileName = MForm.GetDataDirectory() + "WordsDictionary.txt";

    MainWordsDictionary = new SortedDictionary<string, int>();
    ReplaceWordsDictionary = new SortedDictionary<string, string>();

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

        // favorably
        // favorable
        string ETest = TestWord = "e";
        if( MainWordsDictionary.ContainsKey( ETest ))
          return ETest;

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

      // enlargement
      if( (InWord[InWord.Length - 4] == 'm' ) &&
          (InWord[InWord.Length - 3] == 'e' ) &&
          (InWord[InWord.Length - 2] == 'n' ) &&
          (InWord[InWord.Length - 1] == 't' ))
        {
        TestWord = Utility.TruncateString( InWord, InWord.Length - 4 );
        if( MainWordsDictionary.ContainsKey( TestWord ))
          {
          MForm.ShowStatus( "Changed From: " + InWord + "  To: " + TestWord );
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



  internal bool WordIsExcluded( string Word )
    {
    // This is mainly needed to help find new words
    // that aren't yet in the dictionary.  These words
    // aren't in the dictionary and I don't want it to
    // keep telling me that they aren't.

    // If this gets bigger it will have to be
    // done in a different way.  I sometimes start out
    // with a few things hard-coded and then if it gets
    // out of hand I write something else,
    // like the main dictionary.

    if( Word == null ) // If it ever got a null.
      return true;

    if( Word.Length < 3 )
      return true;

    ///////
    if( Word[0] == 'a' )
      {
      if( Word == "about" )
        return true;

      if( Word == "and" )
        return true;

      if( Word == "animascitytheatre" )
        return true;

      if( Word == "are" )
        return true;

      }


    if( Word == "been" )
      return true;

    if( Word == "but" )
      return true;

    if( Word == "com" )
      return true;

    ///////
    if( Word[0] == 'd' )
      {
      if( Word == "did" )
        return true;

      if( Word == "didn" ) // didn't
        return true;

      if( Word == "does" )
        return true;

      if( Word == "doesn" ) // doesn't
        return true;

      if( Word == "durango" ) // It's the Durango Herald.
        return true;

      }

    ///////
    if( Word[0] == 'f' )
      {
      if( Word == "for" )
        return true;

      if( Word == "from" )
        return true;

      if( Word == "frontpage" )
        return true;

      }

    //////
    if( Word[0] == 'g' )
      {
      if( Word == "get" )
        return true;

      if( Word == "gets" )
        return true;

      if( Word == "getting" )
        return true;

      if( Word == "goes" )
        return true;

      if( Word == "going" )
        return true;

      }

    //////
    if( Word[0] == 'h' )
      {
      if( Word == "has" )
        return true;

      if( Word == "hasn" ) // hasn't
        return true;

      if( Word == "have" )
        return true;

      if( Word == "having" )
        return true;

      if( Word == "henrystratertheatre" )
        return true;

      if( Word == "herald" ) // It's the Durango Herald.
        return true;

      if( Word == "homepage" )
        return true;

      }

    //////
    if( Word[0] == 'i' )
      {
      if( Word == "img" )
        return true;

      if( Word == "isn" ) // isn't
        return true;

      if( Word == "its" )
        return true;

      }

    ////////
    if( Word[0] == 'n' )
      {
      if( Word == "newsmemory" )
        return true;

      if( Word == "non" )
        return true;

      if( Word == "nor" )
        return true;

      if( Word == "not" )
        return true;

      }


    if( Word == "ops" )
      return true;


    if( Word == "png" )
      return true;

    if( Word == "pvt" )
      return true;

    //////
    if( Word[0] == 't' )
      {
      if( Word == "that" )
        return true;

      if( Word == "the" )
        return true;

      if( Word == "their" )
        return true;

      if( Word == "theirs" )
        return true;

      if( Word == "them" )
        return true;

      if( Word == "then" )
        return true;

      if( Word == "there" )
        return true;

      if( Word == "these" )
        return true;

      if( Word == "they" )
        return true;

      if( Word == "this" )
        return true;

      if( Word == "those" )
        return true;

      if( Word == "thus" )
        return true;

      if( Word == "too" )
        return true;

      }


    if( Word == "using" )
      return true;

    ///////
    if( Word[0] == 'w' )
      {
      if( Word == "was" )
        return true;

      if( Word == "wasn" ) // Wasn't
        return true;

      if( Word == "went" )
        return true;

      if( Word == "were" )
        return true;

      if( Word == "what" )
        return true;

      if( Word == "with" )
        return true;

      if( Word == "www" )
        return true;

      }

    for( int Count = 0; Count < Word.Length; Count++ )
      {
      if( !Char.IsLetter( Word[Count] ))
        return true;

      }

    return false;
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
    ReplaceWordsDictionary["broomfiled"] = "broomfield";
    ReplaceWordsDictionary["calif"] = "california";
    ReplaceWordsDictionary["colo"] = "colorado";
    ReplaceWordsDictionary["conn"] = "connecticut";
    ReplaceWordsDictionary["downton"] = "downtown";
    ReplaceWordsDictionary["fla"] = "florida";
    ReplaceWordsDictionary["intro"] = "introduction";
    ReplaceWordsDictionary["sen"] = "senator";
    ReplaceWordsDictionary["thinkin"] = "think";

                              
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

        // For test:
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
      if( Kvp.Value < 100 )
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

