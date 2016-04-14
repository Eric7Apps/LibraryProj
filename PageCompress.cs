// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace DGOLibrary
{
  class PageCompress
  {
  private MainForm MForm;
  private LinesRec[] LinesRecArray;
  private const int LinesRecArrayLength = 0xFFFF + 1;
  private string[] StringsArray;
  private int StringsArrayLast = 0;
  private string FileName = "";
  internal const int MinimumStringLength = 3;


  public struct LinesRec
    {
    public LineIndexRec[] LineIndexArray;
    public int LineIndexArrayLast;
    }


  public struct LineIndexRec
    {
    public string Line;
    public int Index;
    public int Frequency;
    // public int Weight;
    }



  private PageCompress()
    {
    }


  internal PageCompress( MainForm UseForm )
    {
    MForm = UseForm;
    LinesRecArray = new LinesRec[LinesRecArrayLength];

    int ArraySize = MForm.GlobalProps.GetLastIndexInCompress1();
    ArraySize++;
    if( ArraySize < 1024 )
      ArraySize = 1024;

    StringsArray = new string[ArraySize];
    FileName = MForm.GetDataDirectory() + "CompressDictionary.txt";
    }



  internal bool AddFrequencyLine( string Line, int Frequency )
    {
    try
    {
    if( Line == null )
      return false;

    // The most frequent strings (not the highest
    // weight values) are added first, so they are
    // given the smallest index numbers.
    // The most frequent string of all is given the
    // index zero.
    // So the most frequent strings take up less space
    // as strings.
    if( Line.Length < MinimumStringLength )
      return false;

    LineIndexRec Rec = new LineIndexRec();
    Rec.Line = Line;
    Rec.Frequency = Frequency;
    // Rec.Weight = Frequency * Line.Length;

    StringsArray[StringsArrayLast] = Line;
    Rec.Index = StringsArrayLast;
    StringsArrayLast++;
    if( StringsArrayLast >= StringsArray.Length )
      Array.Resize( ref StringsArray, StringsArray.Length + (1024 * 32) );

    uint CRCIndex = Utility.GetCRC16( Line );

    if( LinesRecArray[CRCIndex].LineIndexArray ==  null )
      LinesRecArray[CRCIndex].LineIndexArray = new LineIndexRec[8];

    LinesRecArray[CRCIndex].LineIndexArray[LinesRecArray[CRCIndex].LineIndexArrayLast] = Rec;
    LinesRecArray[CRCIndex].LineIndexArrayLast++;
    if( LinesRecArray[CRCIndex].LineIndexArrayLast >= LinesRecArray[CRCIndex].LineIndexArray.Length )
      Array.Resize( ref LinesRecArray[CRCIndex].LineIndexArray, LinesRecArray[CRCIndex].LineIndexArray.Length + 8 );

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddFrequencyLine():" );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal void AddVerifiedLine( string FileLine )
    {
    try
    {
    if( FileLine == null )
      return;

    if( FileLine.Length < 2 )
      return;

    string[] SplitS = FileLine.Split( new Char[] { '\t' } );
    if( SplitS.Length < 3 )
      return;

    LineIndexRec Rec = new LineIndexRec();
    try
    {
    Rec.Index = Int32.Parse( SplitS[0] );
    }
    catch( Exception )
      {
      throw( new Exception( "Bad index in AddVerifiedLine()." ));
      }

    if( Rec.Index < 0 )
      throw( new Exception( "Rec.Index < 0 in AddVerifiedLine()." ));

    if( Rec.Index > 1000000 )
      throw( new Exception( "Rec.Index > 1000000 in AddVerifiedLine()." ));

    try
    {
    Rec.Frequency = Int32.Parse( SplitS[1] );
    }
    catch( Exception )
      {
      throw( new Exception( "Bad frequency in AddVerifiedLine()." ));
      }

    Rec.Line = SplitS[2];
    if( Rec.Line.Length < MinimumStringLength )
      throw( new Exception( "Rec.Line.Length < MinimumStringLength in AddVerifiedLine()." ));

    // Rec.Weight = Rec.Frequency * Rec.Line.Length;

    if( StringsArrayLast <= Rec.Index )
      StringsArrayLast = Rec.Index + 1;

    if( Rec.Index >= StringsArray.Length )
      Array.Resize( ref StringsArray, Rec.Index + 1024 );

    StringsArray[Rec.Index] = Rec.Line;

    uint CRCIndex = Utility.GetCRC16( Rec.Line );

    if( LinesRecArray[CRCIndex].LineIndexArray ==  null )
      LinesRecArray[CRCIndex].LineIndexArray = new LineIndexRec[8];

    LinesRecArray[CRCIndex].LineIndexArray[LinesRecArray[CRCIndex].LineIndexArrayLast] = Rec;
    LinesRecArray[CRCIndex].LineIndexArrayLast++;
    if( LinesRecArray[CRCIndex].LineIndexArrayLast >= LinesRecArray[CRCIndex].LineIndexArray.Length )
      Array.Resize( ref LinesRecArray[CRCIndex].LineIndexArray, LinesRecArray[CRCIndex].LineIndexArray.Length + 8 );

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddVerifiedLine():" );
      MForm.ShowStatus( Except.Message );
      }
    }



  private string GetLineFromIndex( int Where )
    {
    if( Where < 0 )
      {
      MForm.ShowStatus( "Where < 0 in GetLineFromIndex()." );
      return "";
      }

    if( Where >= StringsArrayLast )
      {
      MForm.ShowStatus( "Where >= StringsArrayLast in GetLineFromIndex()." );
      return "";
      }

    // This is why decompressing is a lot faster
    // than compressing.
    string Result = StringsArray[Where];

    if( Result == null )
      {
      MForm.ShowStatus( "GetLineFromIndex(): Result was null at: " + Where.ToString() );
      return "";
      }

    return Result;
    }



  internal int GetIndex( string Line )
    {
    try
    {
    if( Line == null )
      return -1;

    if( Line.Length < MinimumStringLength )
      return -1;

    uint Index = Utility.GetCRC16( Line );
    if( LinesRecArray[Index].LineIndexArray ==  null )
      return -1;

    int Last = LinesRecArray[Index].LineIndexArrayLast;
    for( int Count = 0; Count < Last; Count++ )
      {
      // Notice that this is case-sensitive.
      if( Line == LinesRecArray[Index].LineIndexArray[Count].Line )
        return LinesRecArray[Index].LineIndexArray[Count].Index;

      }

    return -1;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetIndex():" );
      MForm.ShowStatus( Except.Message );
      return -1;
      }
    }



  internal bool WriteToTextFile()
    {
    try
    {
    int Written = 0;
    int LastIndex = 0;
    using( StreamWriter SWriter = new StreamWriter( FileName, false, Encoding.UTF8 ))
      {
      for( int Index = 0; Index < LinesRecArrayLength; Index++ )
        {
        if( LinesRecArray[Index].LineIndexArray ==  null )
          continue;

        int Last = LinesRecArray[Index].LineIndexArrayLast;
        for( int Count = 0; Count < Last; Count++ )
          {
          LineIndexRec Rec =  LinesRecArray[Index].LineIndexArray[Count];
          if( Rec.Line == null )
            throw( new Exception( "Bug: Rec.Line should not be null here." ));

          if( LastIndex < Rec.Index )
            LastIndex = Rec.Index;

          Written++;
          string Line = Rec.Index.ToString() + "\t" +
             Rec.Frequency.ToString() + "\t" +
             Rec.Line;

          SWriter.WriteLine( Line );
          }
        }
      }

    MForm.GlobalProps.SetLastIndexInCompress1( LastIndex );

    MForm.ShowStatus( "PageCompress wrote " + Written.ToString( "N0" ) + " lines to the file." );
    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in PageCompress.WriteToTextFile():" );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  private void ClearAll()
    {
    try
    {
    StringsArrayLast = 0;

    for( int Index = 0; Index < LinesRecArrayLength; Index++ )
      {
      LinesRecArray[Index].LineIndexArray =  null;
      LinesRecArray[Index].LineIndexArrayLast = 0;
      }
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in PageCompress.ClearAll():" );
      MForm.ShowStatus( Except.Message );
      }
    }



  internal void VerifyStringsArray()
    {
    int LastIndex = MForm.GlobalProps.GetLastIndexInCompress1();
    if( (StringsArrayLast - 1) != LastIndex )
      {
      MForm.ShowStatus( " " );
      MForm.ShowStatus( " " );
      MForm.ShowStatus( " " );
      MForm.ShowStatus( " " );
      MForm.ShowStatus( "LastIndexInCompress1 isn't what it should be." );
      MForm.ShowStatus( "LastIndex: " + LastIndex.ToString());
      MForm.ShowStatus( "StringsArrayLast: " + StringsArrayLast.ToString());
      MForm.ShowStatus( " " );
      MForm.ShowStatus( " " );
      MForm.ShowStatus( " " );
      MForm.ShowStatus( " " );
      // throw( new Exception( "Check on this." ));
      }

    for( int Count = 0; Count < StringsArrayLast; Count++ )
      {
      if( StringsArray[Count] == null )
        {
        // MForm.ShowStatus( "There was a null string at: " + Count.ToString());
        throw( new Exception( "There was a null string at: " + Count.ToString() + " in VerifyStringsArray()." ));
        }
      }
    }



  internal bool ReadFromTextFile()
    {
    try
    {
    ClearAll();
    if( !File.Exists( FileName ))
      return false;

    int HowMany = 0;
    using( StreamReader SReader = new StreamReader( FileName, Encoding.UTF8 ))
      {
      // int Loops = 0;
      while( SReader.Peek() >= 0 ) 
        {
        /*
        Loops++;
        if( (Loops & 0xFFF) == 0 )
          {
          if( !MForm.CheckEvents())
            return false;

          }
          */

        string Line = SReader.ReadLine();
        if( Line == null )
          continue;

        if( !Line.Contains( "\t" ))
          continue;

        // This might throw an exception.
        AddVerifiedLine( Line );
        HowMany++;
        }
      }

    // This might throw an exception.
    VerifyStringsArray();

    MForm.ShowStatus( "Number of compress lines after verify: " + HowMany.ToString( "N0" ));
    return true;
    }
    catch( Exception Except )
      {
      ClearAll();
      MForm.ShowStatus( "Exception in PageCompress.ReadFromTextFile()." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }


  internal static string ReplaceCharacters( string InString )
    {
    // Replace some super-common ones with one symbol.

    // The marker symbols are 3 UTF8 bytes, but only one
    // byte in CharacterIndex.cs.
    InString = InString.Replace( '\r', MarkersDelimiters.CRReplace );
    // InString = InString.Replace( '\n', MarkersDelimiters.NewLineReplace );
    InString = InString.Replace( "href=\"", Char.ToString( MarkersDelimiters.AnchorTag ) );
    InString = InString.Replace( "www.durangoherald.com", Char.ToString( MarkersDelimiters.HeraldDomain ));
    InString = InString.Replace( "fb:comments-count", Char.ToString( MarkersDelimiters.FaceBookComments ));
    // InString = InString.Replace( "<p class=\"articleText\"", Char.ToString( MarkersDelimiters.Paragraph ));
    InString = InString.Replace( "the", Char.ToString( MarkersDelimiters.TheReplace ));
    InString = InString.Replace( "and", Char.ToString( MarkersDelimiters.AndReplace ));
    InString = InString.Replace( "div", Char.ToString( MarkersDelimiters.DivReplace ));
    InString = InString.Replace( "<li", Char.ToString( MarkersDelimiters.LiTag ));
    InString = InString.Replace( "article", Char.ToString( MarkersDelimiters.ArticleWord ));
    InString = InString.Replace( "\"fb_comment_link\"", Char.ToString( MarkersDelimiters.FaceBookCommentLink ));
    InString = InString.Replace( "/", Char.ToString( MarkersDelimiters.SlashReplace ) + " " );
    InString = InString.Replace( ">", Char.ToString( MarkersDelimiters.TagReplace ) + " " );
    InString = InString.Replace( "=", Char.ToString( MarkersDelimiters.EqualsReplace ) + " " );
    InString = InString.Replace( "class➇", Char.ToString( MarkersDelimiters.ClassReplace ));
    InString = InString.Replace( "-", Char.ToString( MarkersDelimiters.DashReplace ) + " " );

    return InString;
    }


  internal static string ReverseReplaceCharacters( string InString )
    {
    InString = InString.Replace( Char.ToString( MarkersDelimiters.DashReplace ) + " ", "-" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.ClassReplace ), "class➇" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.EqualsReplace ) + " ", "=" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.TagReplace ) + " ", ">" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.SlashReplace ) + " ", "/" );

    InString = InString.Replace( Char.ToString( MarkersDelimiters.FaceBookCommentLink ), "\"fb_comment_link\"" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.ArticleWord ), "article" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.LiTag ), "<li" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.DivReplace ), "div" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.AndReplace ), "and");
    InString = InString.Replace( Char.ToString( MarkersDelimiters.TheReplace ), "the");
    // InString = InString.Replace( Char.ToString( MarkersDelimiters.Paragraph ), "<p class=\"articleText\"" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.FaceBookComments ), "fb:comments-count" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.HeraldDomain ), "www.durangoherald.com" );
    InString = InString.Replace( Char.ToString( MarkersDelimiters.AnchorTag ), "href=\"" );
    // InString = InString.Replace( MarkersDelimiters.NewLineReplace, '\n' );
    InString = InString.Replace( MarkersDelimiters.CRReplace, '\r' );
    return InString;
    }



  internal bool ReadFromFrequencyFile()
    {
    try
    {
    string FreqFileName = MForm.GetDataDirectory() + "FrequencyCountPage.txt";

    ClearAll();
    if( !File.Exists( FreqFileName ))
      return false;

    int LongestLine = 0;
    int HowMany = 0;
    long TotalSize = 0;
    using( StreamReader SReader = new StreamReader( FreqFileName, Encoding.UTF8 ))
      {
      int Loops = 0;
      while( SReader.Peek() >= 0 ) 
        {
        Loops++;
        if( (Loops & 0xFFF) == 0 )
          {
          if( !MForm.CheckEvents())
            return false;

          }

        string Line = SReader.ReadLine();
        if( Line == null )
          continue;

        if( !Line.Contains( "\t" ))
          continue;

        string[] SplitS = Line.Split( new Char[] { '\t' } );
        if( SplitS.Length < 2 )
          continue;

        int Frequency = 0;
        try
        {
        Frequency = Int32.Parse( SplitS[0] );
        }
        catch( Exception )
          {
          continue;
          }

        string FreqLine = SplitS[1];

        if( FreqLine.Length > LongestLine )
          LongestLine = FreqLine.Length;

        TotalSize += (Frequency * FreqLine.Length);
        if( AddFrequencyLine( FreqLine, Frequency ))
          HowMany++;

        }
      }

    MForm.ShowStatus( "Number of compress lines: " + HowMany.ToString( "N0" ));
    MForm.ShowStatus( "LongestLine: " + LongestLine.ToString( "N0" ));
    MForm.ShowStatus( "TotalSize: " + TotalSize.ToString( "N0" ));
    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in ReadFromFrequencyFile()." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal string GetCompressedPage( string InString )
    {
    try
    {
    StringBuilder SBuilder = new StringBuilder();

    InString = ReplaceCharacters( InString );

    int FieldCount = Utility.CountCharacters( InString, ' ' );
    SBuilder.Append( FieldCount.ToString() + MarkersDelimiters.FieldCountMark );

    string[] SplitS = InString.Split( new Char[] { ' ' } );

    if( FieldCount > SplitS.Length )
      throw( new Exception( "FieldCount > SplitS.Length in compress." ));

    for( int Count = 0; Count < FieldCount; Count++ )
      {
      string Line = SplitS[Count];
      if( Line.Length == 0 )
        {
        SBuilder.Append( " " );
        continue;
        }

      int Index = GetIndex( Line );
      if( Index >= 0 )
        {
        Line = Char.ToString( MarkersDelimiters.IndexBeginMark ) + Index.ToString();
        }
      // else leave the string as-is.

      SBuilder.Append( Line + " " );
      }

    string Result = SBuilder.ToString();
    Result = Result.Replace( " " + Char.ToString( MarkersDelimiters.IndexBeginMark ), Char.ToString( MarkersDelimiters.IndexBeginAndSpaceReplace ));
    return Result;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetCompressedPage():" );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  internal string GetDecompressedPage( string InString )
    {
    try
    {
    StringBuilder SBuilder = new StringBuilder();

    InString = InString.Replace( Char.ToString( MarkersDelimiters.IndexBeginAndSpaceReplace ), " " + Char.ToString( MarkersDelimiters.IndexBeginMark ));

    int Index = InString.IndexOf( MarkersDelimiters.FieldCountMark );
    if( Index < 0 )
      {
      MForm.ShowStatus( "The decompressed page didn't have the field count marker." );
      return "";
      }

    string FieldPart = InString.Remove( Index );
    // MForm.ShowStatus( "FieldPart: " + FieldPart );
    string MainPart = InString.Remove( 0, Index + 1 );

    int FieldCount = 0;
    try
    {
    FieldCount = Int32.Parse( FieldPart );
    }
    catch( Exception ) // Except )
      {
      MForm.ShowStatus( "Exception getting the FieldCount value." );
      return "";
      }

    string[] SplitS = MainPart.Split( new Char[] { ' ' } );
    if( FieldCount > SplitS.Length )
      throw( new Exception( "FieldCount > SplitS.Length in decompress." ));

    for( int Count = 0; Count < FieldCount; Count++ )
      {
      string Line = SplitS[Count];
      // Line might be "".
      if( Line.Length == 0 )
        {
        SBuilder.Append( " " );
        continue;
        }

      if( Line.StartsWith( Char.ToString( MarkersDelimiters.IndexBeginMark )))
        {
        Line = Line.Remove( 0, 1 );
        try
        {
        Index = Int32.Parse( Line );
        }
        catch( Exception ) // Except )
          {
          MForm.ShowStatus( "Exception getting the integer in: " + Line );
          return "";
          }

        Line = GetLineFromIndex( Index );
        if( Line == "" )
          {
          MForm.ShowStatus( "There was no line at index: " + Index.ToString() );
          return "";
          }
        }

      SBuilder.Append( Line + " " );
      }

    string Result = ReverseReplaceCharacters( SBuilder.ToString());
    return Result;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in GetDecompressedPage():" );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  }
}

