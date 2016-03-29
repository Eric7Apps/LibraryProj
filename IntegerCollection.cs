// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.Text;
// using System.Threading.Tasks;

namespace DGOLibrary
{
  class IntegerCollection
  {
  private OneArray[] MainArray;
  private const int MainArrayLength = 8;
  private const int MainArrayMask = 7;

  public struct OneArray
    {
    public int[] IntArray;
    public int IntArrayLast;
    }


  internal IntegerCollection()
    {
    MainArray = new OneArray[MainArrayLength];
    }


  internal string ObjectToString()
    {
    StringBuilder SBuilder = new StringBuilder();
    for( int WhichArray = 0; WhichArray < MainArrayLength; WhichArray++ )
      {
      if( MainArray[WhichArray].IntArray == null )
        continue;

      for( int Count = 0; Count < MainArray[WhichArray].IntArrayLast; Count++ )
        SBuilder.Append( MainArray[WhichArray].IntArray[Count].ToString() + ";" );

      }

    return SBuilder.ToString();
    }



  internal int[] GetIndexArray()
    {
    try
    {
    int TotalLength = GetTotalArrayLength();
    if( TotalLength == 0 )
      return null;

    int[] Result = new int[TotalLength];
    int Where = 0;
    for( int WhichArray = 0; WhichArray < MainArrayLength; WhichArray++ )
      {
      if( MainArray[WhichArray].IntArray == null )
        continue;

      for( int Count = 0; Count < MainArray[WhichArray].IntArrayLast; Count++ )
        {
        Result[Where] = MainArray[WhichArray].IntArray[Count];
        Where++;
        }
      }

    return Result;
    }
    catch( Exception ) // Except )
      {
      return null;
      }
    }



  internal int GetTotalArrayLength()
    {
    int Total = 0;
    for( int WhichArray = 0; WhichArray < MainArrayLength; WhichArray++ )
      Total += MainArray[WhichArray].IntArrayLast;

    return Total;
    }



  internal void ClearAll()
    {
    for( int WhichArray = 0; WhichArray < MainArrayLength; WhichArray++ )
      {
      MainArray[WhichArray].IntArray = null;
      MainArray[WhichArray].IntArrayLast = 0;
      }
    }



  internal bool StringToObject( string InString )
    {
    try
    {
    ClearAll();

    string[] SplitS = InString.Split( new Char[] { ';' } );
    for( int Count = 0; Count < SplitS.Length; Count++ )
      {
      if( SplitS[Count].Length < 1 )
        break;

      try
      {
      int Index = Int32.Parse( SplitS[Count] );
      AddInteger( Index, false );
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
      string ShowS = "Exception in IntegerCollection.StringToObject().\r\n" +
        Except.Message;

      throw( new Exception( ShowS ));
      }
    }



  internal bool IntegerExists( int IntToCheck )
    {
    int WhichArray = IntToCheck & MainArrayMask;
    if( MainArray[WhichArray].IntArray == null )
      return false;

    for( int Count = 0; Count < MainArray[WhichArray].IntArrayLast; Count++ )
      {
      if( IntToCheck == MainArray[WhichArray].IntArray[Count] )
        return true;

      }

    return false;
    }



  internal void AddInteger( int IntToAdd, bool CheckExists )
    {
    try
    {
    if( CheckExists )
      {
      if( IntegerExists( IntToAdd ))
        return;

      }

    int WhichArray = IntToAdd & MainArrayMask;
    if( MainArray[WhichArray].IntArray == null )
      MainArray[WhichArray].IntArray = new int[8];

    MainArray[WhichArray].IntArray[MainArray[WhichArray].IntArrayLast] = IntToAdd;
    MainArray[WhichArray].IntArrayLast++;

    if( MainArray[WhichArray].IntArrayLast >= MainArray[WhichArray].IntArray.Length )
      {
      Array.Resize( ref MainArray[WhichArray].IntArray, MainArray[WhichArray].IntArray.Length + 64 );
      }

    }
    catch( Exception Except )
      {
      string ShowS = "Exception in IntegerCollection.AddInteger().\r\n" +
        Except.Message;

      throw( new Exception( ShowS ));
      }
    }


  // This is like a logical OR because it will collect
  // together anything that is in one or the other.
  internal void AddFromCollection( IntegerCollection AddFrom )
    {
    for( int WhichArray = 0; WhichArray < MainArrayLength; WhichArray++ )
      {
      if( AddFrom.MainArray[WhichArray].IntArray == null )
        continue;

      for( int Count = 0; Count < AddFrom.MainArray[WhichArray].IntArrayLast; Count++ )
        AddInteger( AddFrom.MainArray[WhichArray].IntArray[Count], true );

      }
    }



  internal void LogicANDFromCollections( IntegerCollection From1, IntegerCollection From2 )
    {
    ClearAll();

    if( From1 == null )
      return;

    if( From2 == null )
      return;

    for( int WhichArray = 0; WhichArray < MainArrayLength; WhichArray++ )
      {
      if( From1.MainArray[WhichArray].IntArray == null )
        continue;

      for( int Count = 0; Count < From1.MainArray[WhichArray].IntArrayLast; Count++ )
        {
        // If it exists in both.
        if( From2.IntegerExists( From1.MainArray[WhichArray].IntArray[Count] ))
          AddInteger( From1.MainArray[WhichArray].IntArray[Count], false );

        }
      }
    }


  }
}
