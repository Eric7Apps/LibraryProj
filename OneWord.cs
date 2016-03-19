// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DGOLibrary
{
  class OneWord
  {
  private MainForm MForm;
  private string Word = "";
  private int[] PageIndexArray; // Index to pages that have this word.
  private int PageIndexArrayLast = 0;


  private OneWord()
    {
    }


  internal OneWord( MainForm UseForm )
    {
    MForm = UseForm;

    }


  internal string GetWord()
    {
    return Word;
    }


  internal void SetWord( string SetTo )
    {
    Word = SetTo;
    }



  internal string ObjectToString()
    {
    string Result = Word + "\t"; // +

    if( (PageIndexArray == null) ||
        (PageIndexArrayLast == 0 ))
      {
      Result += ";\t";
      }
    else
      {
      StringBuilder SBuilder = new StringBuilder();
      for( int Count = 0; Count < PageIndexArrayLast; Count++ )
        SBuilder.Append( PageIndexArray[Count].ToString() + ";" );

      Result += SBuilder.ToString() + "\t";
      }

    return Result;
    }



  internal bool StringToObject( string InString )
    {
    try
    {
    string[] SplitS = InString.Split( new Char[] { '\t' } );

    if( SplitS.Length < 2 )
      return false;

    Word = Utility.GetCleanUnicodeString( SplitS[0], 100 );

    string IndexS = Utility.GetCleanUnicodeString( SplitS[1], 2000000000 );

    string[] SplitIndex = IndexS.Split( new Char[] { ';' } );

    for( int Count = 0; Count < SplitIndex.Length; Count++ )
      {
      if( SplitIndex[Count].Length < 1 )
        break;

      try
      {
      int Index = Int32.Parse( SplitIndex[Count] );
      AddPageIndex( Index, false );
      }
      catch( Exception )
        {
        break;
        }
      }


    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in OneWord.StringToObject()." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }



  internal void UpdateURL( string URL )
    {
    if( MForm.GetIsClosing())
      return;

    int Index = MForm.PageList1.GetIndex( URL );
    if( Index < 0 )
      return;

    AddPageIndex( Index, true );
    }



  internal bool IndexExists( int Index )
    {
    if( PageIndexArray == null )
      return false;

    // This gets called on every word that is added from
    // a file so it should be fast.
    // Do a binary sort and search.
    // Or partition it by a bit mask (like mod 32).
    for( int Count = 0; Count < PageIndexArrayLast; Count++ )
      {
      if( Index == PageIndexArray[Count] )
        return true;

      }

    return false;
    }



  internal bool AddPageIndex( int Index, bool CheckExists )
    {
    try
    {
    if( CheckExists )
      {
      if( IndexExists( Index ))
        return true;

      }

    if( PageIndexArray == null )
      PageIndexArray = new int[8];

    PageIndexArray[PageIndexArrayLast] = Index;
    PageIndexArrayLast++;

    if( PageIndexArrayLast >= PageIndexArray.Length )
      {
      try
      {
      Array.Resize( ref PageIndexArray, PageIndexArray.Length + 64 );
      }
      catch
        {
        return false;
        }
      }

    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddPageIndex()." );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }


  }
}

