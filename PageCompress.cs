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
  private string[] SplitPageArray;
  private int SplitPageArrayLast = 0;
  private string FileName = "";
  internal const int MinimumStringLength = 5;


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
    public int Weight;
    }


    // These strings can include a CR character.
    // 510346	\r</div
    // 0	510346	✀</div
    // They are not trimmed.

  private PageCompress()
    {
    }


  internal PageCompress( MainForm UseForm )
    {
    MForm = UseForm;
    LinesRecArray = new LinesRec[LinesRecArrayLength];
    StringsArray = new string[1024];
    SplitPageArray = new string[1024];
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
    // 0	510346	✀</div
    // So the most frequent strings take up less space
    // as strings.
    // If there are 400,000 strings that means the
    // biggest index values are 6 characters long
    // (as strings).
    // The most frequent string (so far) is "\r</div"
    // which is 6 characters long.  It would be 
    // replaced by a two-byte marker followed by
    // the index value of 0.  So that's two characters
    // and 4 bytes.  Three bytes as UTF8.
    // The next most frequent tag is "</script"
    // which is 8 characters.

    // Delim is one character (two bytes), plus a max
    // of 123456 for an index string, so that's 8
    // characters.
    // Make it 6 to make it worth keeping.
    if( Line.Length < MinimumStringLength )
      return false;

    if( Frequency < 3 )
      return false;

    LineIndexRec Rec = new LineIndexRec();
    Rec.Line = Line;
    Rec.Frequency = Frequency;
    Rec.Weight = Frequency * Line.Length;

    StringsArray[StringsArrayLast] = Line;
    Rec.Index = StringsArrayLast;
    StringsArrayLast++;
    if( StringsArrayLast >= StringsArray.Length )
      Array.Resize( ref StringsArray, StringsArray.Length + 1024 );

    uint CRCIndex = Utility.GetCRC16( Line );

    if( LinesRecArray[CRCIndex].LineIndexArray ==  null )
      LinesRecArray[CRCIndex].LineIndexArray = new LineIndexRec[16];

    LinesRecArray[CRCIndex].LineIndexArray[LinesRecArray[CRCIndex].LineIndexArrayLast] = Rec;
    LinesRecArray[CRCIndex].LineIndexArrayLast++;
    if( LinesRecArray[CRCIndex].LineIndexArrayLast >= LinesRecArray[CRCIndex].LineIndexArray.Length )
      Array.Resize( ref LinesRecArray[CRCIndex].LineIndexArray, LinesRecArray[CRCIndex].LineIndexArray.Length + 16 );

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddFrequencyLine():" );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal void AddVerifiedLine( string Line )
    {
    try
    {
    if( Line == null )
      return;

    if( Line.Length < 2 )
      return;

    string[] SplitS = Line.Split( new Char[] { '\t' } );
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

    Rec.Weight = Rec.Frequency * Rec.Line.Length;

    if( Rec.Index >= StringsArray.Length )
      Array.Resize( ref StringsArray, Rec.Index + 1024 );

    StringsArray[Rec.Index] = Line;
    if( StringsArrayLast < Rec.Index )
      StringsArrayLast = Rec.Index;

    uint CRCIndex = Utility.GetCRC16( Line );

    if( LinesRecArray[CRCIndex].LineIndexArray ==  null )
      LinesRecArray[CRCIndex].LineIndexArray = new LineIndexRec[16];

    LinesRecArray[CRCIndex].LineIndexArray[LinesRecArray[CRCIndex].LineIndexArrayLast] = Rec;
    LinesRecArray[CRCIndex].LineIndexArrayLast++;
    if( LinesRecArray[CRCIndex].LineIndexArrayLast >= LinesRecArray[CRCIndex].LineIndexArray.Length )
      Array.Resize( ref LinesRecArray[CRCIndex].LineIndexArray, LinesRecArray[CRCIndex].LineIndexArray.Length + 16 );

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

    string Result = StringsArray[Where];
    if( Result == null )
      return "";

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

          Written++;
          string Line = Rec.Index.ToString() + "\t" +
             Rec.Frequency.ToString() + "\t" +
             Rec.Line;

          SWriter.WriteLine( Line );
          }
        }
      }

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
    StringsArray = new string[1024];
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



  internal bool VerifyStringsArray()
    {
    for( int Count = 0; Count < StringsArrayLast; Count++ )
      {
      if( StringsArray[Count] == null )
        {
        MForm.ShowStatus( "There was a null string at: " + Count.ToString());
        throw( new Exception( "There was a null string at: " + Count.ToString() + " in VerifyStringsArray()." ));
        }
      }

    return true;
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
      while( SReader.Peek() >= 0 ) 
        {
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



  internal bool ReadFromFrequencyFile()
    {
    try
    {
    string FreqFileName = MForm.GetDataDirectory() + "FrequencyCount.txt";

    ClearAll();
    if( !File.Exists( FreqFileName ))
      return false;

    int LongestLine = 0;
    int HowMany = 0;
    int TotalSize = 0;
    using( StreamReader SReader = new StreamReader( FreqFileName, Encoding.UTF8 ))
      {
      int Loops = 0;
      while( SReader.Peek() >= 0 ) 
        {
        Loops++;
        if( (Loops & 0xFF) == 0 )
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


  private void AddSplitPageLine( string Line )
    {
    SplitPageArray[SplitPageArrayLast] = Line;
    SplitPageArrayLast++;
    if( SplitPageArrayLast >= SplitPageArray.Length )
      Array.Resize( ref SplitPageArray, SplitPageArray.Length + 1024 );

    }



  internal string GetCompressedPage( string InString )
    {
    try
    {
    SplitPageArrayLast = 0;
    string[] SplitS = InString.Split( new Char[] { '>' } );
    for( int Count = 0; Count < SplitS.Length; Count++ )
      {
      string Line = SplitS[Count];
      Line = Line.Replace( "\r", MarkersDelimiters.CRReplace );
      AddSplitPageLine( Line );
      }

    StringBuilder SBuilder = new StringBuilder();
    for( int Count = 0; Count < SplitPageArrayLast; Count++ )
      {
      int Index = GetIndex( SplitPageArray[Count] );
      if( Index >= 0 )
        {
        SplitPageArray[Count] = MarkersDelimiters.IndexBeginMark + Index.ToString();
        }
      // else leave the string as-is.

      SBuilder.Append( SplitPageArray[Count] + ">" );
      }

    return SBuilder.ToString();

    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddFrequencyLine():" );
      MForm.ShowStatus( Except.Message );
      return "";
      }
    }



  }
}
