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
  private IntegerCollection IntCollection;



  private OneWord()
    {
    }


  internal OneWord( MainForm UseForm )
    {
    MForm = UseForm;

    IntCollection = new IntegerCollection();
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
    string Result = Word + "\t" +
      IntCollection.ObjectToString();

    return Result;
    }



  internal bool StringToObject( string InString )
    {
    try
    {
    string[] SplitS = InString.Split( new Char[] { '\t' } );

    if( SplitS.Length < 2 )
      return false;

    Word = Utility.GetCleanUnicodeString( SplitS[0], 100, true );

    string IndexS = Utility.GetCleanUnicodeString( SplitS[1], 2000000000, true );
    if( !IntCollection.StringToObject( IndexS ))
      return false;

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
    return IntCollection.IntegerExists( Index );
    }



  internal void AddPageIndex( int Index, bool CheckExists )
    {
    try
    {
    IntCollection.AddInteger( Index, CheckExists );
    }
    catch( Exception Except )
      {
      MForm.ShowStatus( "Exception in AddPageIndex()." );
      MForm.ShowStatus( Except.Message );
      }
    }


  internal IntegerCollection GetIntegerCollection()
    {
    // Or return a copy of it.
    return IntCollection;
    }


  }
}

