// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace DGOLibrary
{
  public partial class GetURLManagerForm : Form
  {
  private MainForm MForm;
  private GetFromURLForm[] GetURLArray;
  private int GetURLArrayLast = 0;
  private ECTime QueueTime;
  private int TempFileNameCount = 0;


  private GetURLManagerForm()
    {
    InitializeComponent();
    }



  internal GetURLManagerForm( MainForm UseForm )
    {
    InitializeComponent();

    MForm = UseForm;
    GetURLArray = new GetFromURLForm[64];
    QueueTime = new ECTime();
    QueueTime.SetToNow();

    AddInitialLinks();

    QueueTimer.Interval = 1000;
    QueueTimer.Start();
    }



  private void AddInitialLinks()
    {
    // This is hard-coded for now, but it could be from a
    // list in a configuration file.
    AddURLForm( "http://www.durangoherald.com" );
    AddURLForm( "http://www.durangoherald.com/section/News01/" );
    }



  private string GetNextTempFileName()
    {
    string TempName = MForm.GetTempFileDirectory() +
      "Temp" + TempFileNameCount.ToString() + ".txt";

    TempFileNameCount++;

    // Since this is getting one new page once every
    // three seconds (depending on your timer event
    // setting) it would not come even remotely close
    // to using all 127 unique file names at one time.

    // Bring it back to zero after 127.
    TempFileNameCount = TempFileNameCount & 0x7F;
    return TempName;
    }



  internal bool AddURLForm( string URL )
    {
    string FileName = GetNextTempFileName();

    GetFromURLForm GetForm = new GetFromURLForm( MForm, URL, FileName );
    GetURLArray[GetURLArrayLast] = GetForm;
    GetURLArrayLast++;

    if( GetURLArrayLast >= GetURLArray.Length )
      {
      try
      {
      Array.Resize( ref GetURLArray, GetURLArray.Length + 64 );
      }
      catch
        {
        return false;
        }
      }

    ShowStatus( "Added to queue: " + URL );
    return true;
    }



  private void ShowStatus( string Status )
    {
    if( IsDisposed )
      return;

    if( MainTextBox.Text.Length > 10000 )
      MainTextBox.Text = "";

    MainTextBox.AppendText( Status + "\r\n" ); 
    }



  private void testToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }



  internal void FreeEverything()
    {
    QueueTimer.Stop();

    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( !GetURLArray[Count].IsDisposed )
        {
        GetURLArray[Count].Hide();
        GetURLArray[Count].FreeEverything();
        GetURLArray[Count].Dispose();
        }

      GetURLArray[Count] = null;
      }

    GetURLArrayLast = 0;
    }



  private void FreeOldForms()
    {
    try
    {
    try
    {
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( !GetURLArray[Count].GetHasStarted())
        continue;
 
      if( GetURLArray[Count].GetFileIsDone() ||
        GetURLArray[Count].GetHadErrorOrCancel())
        {
        ShowStatus( "Closing finished form." );
        if( !GetURLArray[Count].IsDisposed )
          {
          GetURLArray[Count].Hide();
          GetURLArray[Count].FreeEverything();
          GetURLArray[Count].Dispose();
          }

        GetURLArray[Count] = null;
        continue;
        }

      if( GetURLArray[Count].GetStartTimeSecondsToNow() > (60 * 2))
        {
        // It's hung up or something and it shouldn't
        // take this long.
        ShowStatus( "Form timed out." );
        if( !GetURLArray[Count].IsDisposed )
          {
          GetURLArray[Count].Hide();
          GetURLArray[Count].FreeEverything();
          GetURLArray[Count].Dispose();
          }

        GetURLArray[Count] = null;
        continue;
        }
      }
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in FreeOldForms():" );
      ShowStatus( Except.Message );
      }

    int MoveTo = 0;
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( GetURLArray[Count] != null )
        {
        GetURLArray[MoveTo] = GetURLArray[Count];
        MoveTo++;
        }
      }

    GetURLArrayLast = MoveTo;
    ShowStatus( "After freeing old ones last is: " + GetURLArrayLast.ToString());
    }
    catch( Exception Except )
      {
      ShowStatus( "Exception FreeOldForms() (second):" );
      ShowStatus( Except.Message );
      }
    }



  private void StartNextInQueue()
    {
    try
    {
    for( int Count = 0; Count < GetURLArrayLast; Count++ )
      {
      if( GetURLArray[Count].GetHasStarted())
        continue;

      // Start up the first one that hasn't already been started.
      ShowStatus( "Starting next in queue." );

      GetURLArray[Count].StartHttp( true );
      return;
      }

    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in StartNextInQueue():" );
      ShowStatus( Except.Message );
      }
    }



  private void QueueTimer_Tick(object sender, EventArgs e)
    {
    try
    {
    if( MForm.GetIsClosing())
      return;

    FreeOldForms();
 
    // Play nice and don't send requests too often.
    if( QueueTime.GetSecondsToNow() > 3.0 )
      {
      ShowStatus( "Timer for next in queue." );
      StartNextInQueue();
      QueueTime.SetToNow();
      }

    }
    catch( Exception Except )
      {
      ShowStatus( "Exception in QueueTimer_Tick():" );
      ShowStatus( Except.Message );
      }
    }



  }
}


