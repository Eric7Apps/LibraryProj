// Programming by Eric Chauvin.
// Notes on this source code are at:
// ericlibproj.blogspot.com


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.IO;



namespace DGOLibrary
{
  public partial class GetFromURLForm : Form
  {
  private MainForm MForm;
  private bool FileIsDone;
  private bool HadErrorOrCancel;
  private bool HasStarted = false;
  private ECTime StartTime;
  private string URLToGet = "";
  private string FileName = "";
  private string Title = "";
  private string RelativeURLBase = "";


  public struct HttpWorkerInfo
    {
    public string URLToGet;
    public string FileName;
    }



  private GetFromURLForm()
    {
    InitializeComponent();
    }


  internal GetFromURLForm( MainForm UseForm,
                           string UseURLToGet,
                           string UseFileName,
                           string UseTitle,
                           string RelativeURL )
    {
    InitializeComponent();

    MForm = UseForm;
    URLToGet = UseURLToGet;
    FileName = UseFileName;
    Title = UseTitle;
    RelativeURLBase = RelativeURL;
    StartTime = new ECTime();

    HttpBackgroundWorker.WorkerReportsProgress = true;
    HttpBackgroundWorker.WorkerSupportsCancellation = true;
    }



  private void ShowStatus( string Status )
    {
    if( IsDisposed )
      return;

    if( MainTextBox.Text.Length > 10000 )
      MainTextBox.Text = "";

    MainTextBox.AppendText( Status + "\r\n" ); 
    }



  internal double GetStartTimeSecondsToNow()
    {
    return StartTime.GetSecondsToNow();
    }



  internal string GetURL()
    {
    return URLToGet;
    }


  internal string GetTitle()
    {
    return Title;
    }



  internal bool GetHasStarted()
    {
    return HasStarted;
    }



  internal bool GetFileIsDone()
    {
    return FileIsDone;
    }




  internal void FreeEverything()
    {
    if( IsDisposed )
      return;

    try
    {
    if( HttpBackgroundWorker.IsBusy )
      {
      if( !HttpBackgroundWorker.CancellationPending )
        HttpBackgroundWorker.CancelAsync();

      }
    }
    catch( Exception Except )
      {
      ShowStatus( "Error on GetHttpForm FreeEverything(): " + Except.Message );
      }
    }



  internal bool ThreadIsBusy()
    {
    try
    {
    return HttpBackgroundWorker.IsBusy;
    }
    catch( Exception Except )
      {
      ShowStatus( "Error with ThreadIsBusy()." );
      ShowStatus( Except.Message );
      return false;
      }
    }



  internal bool GetHadErrorOrCancel()
    {
    return HadErrorOrCancel;
    }



  private void HttpBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
    BackgroundWorker Worker = (BackgroundWorker)sender;
    if( Worker.CancellationPending )
      {
      e.Cancel = true;
      return;
      }

    HttpWorkerInfo WInfo = (HttpWorkerInfo)(e.Argument);
    Worker.ReportProgress( 0, "Started HTTP background thread." );
    Worker.ReportProgress( 0, "URL to get is: " + WInfo.URLToGet );

    try
    {
    // The URL should have the http in it so that it knows what type of WebRequest this is.
    WebRequest Req = WebRequest.Create( WInfo.URLToGet );
    WebResponse Response = Req.GetResponse(); // This blocks and waits for the whole response.

    if( Worker.CancellationPending )
      {
      e.Cancel = true;
      return;
      }

    Stream RespStream = Response.GetResponseStream();
    try
    {
    long FileSize = Response.ContentLength;

    Worker.ReportProgress( 0, "File size is: " + FileSize.ToString( "N0" ));
    if( FileSize == 0 )
      {
      e.Cancel = true;
      return;
      }

    // Worker.ReportProgress( 0, " " );
    // Worker.ReportProgress( 0, "Headers are:" );
    // for( int Count = 0; Count < Response.Headers.Count; Count++ )
      // Worker.ReportProgress( 0, Response.Headers.Keys[Count] + ", " + Response.Headers[Count] );

    using( FileStream FStream = new FileStream( WInfo.FileName,
                                                FileMode.Create, // Not OpenOrCreate.
                                                FileAccess.Write,
                                                FileShare.None ))
      {
      byte[] ReadData = new byte[1024 * 16];

      int TotalBytesRead = 0; 
      while( true )
        {
        if( Worker.CancellationPending )
          {
          e.Cancel = true;
          return;
          }

        int BytesRead = RespStream.Read( ReadData, 0, ReadData.Length );
        // Worker.ReportProgress( 0, "BytesRead: " + BytesRead.ToString( "N0" ));

        if( BytesRead == 0 ) // End of the stream.
          {
          // Since WebResponse blocks until it's done, 
          // this shouldn't be necessary.
          // Is it just slow in getting data from the connection?
          // If it is, wait a while.
          Thread.Sleep( 200 );
          BytesRead = RespStream.Read( ReadData, 0, ReadData.Length );

          if( BytesRead == 0 ) // End of the stream.
            {
            if( FileSize > TotalBytesRead )
              {
              Worker.ReportProgress( 0, "Error: Could not get the complete file. " + WInfo.URLToGet );
              e.Cancel = true;
              return;
              }
            else
              {
              Worker.ReportProgress( 0, "Finished with download." );
              return;
              }
            }
          }

        TotalBytesRead += BytesRead;
        Worker.ReportProgress( 0, "TotalBytesRead: " + TotalBytesRead.ToString( "N0" ));

        FStream.Write( ReadData, 0, BytesRead );
        }
      }

    }
    finally
      {
      RespStream.Close();
      Response.Close(); 
      }
    }
    catch( Exception Except )
      {
      Worker.ReportProgress( 0, "Error downloading the file " + WInfo.URLToGet + "\r\n" + Except.Message );
      e.Cancel = true;
      return;
      }
    }



  private void HttpBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
    if( IsDisposed )
      return;

    // This runs in the UI thread.

    if( e.UserState == null )
      return;

    ShowStatus( (string)e.UserState );
    
    // e.ProgressPercentage
    }



  private void HttpBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
    // Has this form already closed?  Disposed?
    if( IsDisposed )
      return;

    if( MForm.GetIsClosing())
      return;

    ShowStatus( "Background worker is completed." );

    if( e.Cancelled )
      {
      HadErrorOrCancel = true;
      ShowStatus( "HTTP Background worker was cancelled." );
      return;
      }

    if( e.Error != null )
      {
      HadErrorOrCancel = true;
      ShowStatus( "There was an HTTP error: " + e.Error.Message );
      return;
      }

    if( MForm.PageList1 != null )
      MForm.PageList1.UpdatePageFromFile( Title, URLToGet, FileName, true, RelativeURLBase );

    FileIsDone = true;

    // e.Result
    // e.UserState
    }



  internal void StartHttp( bool ShowDiagnostics )
    {
    FileIsDone = false;
    HadErrorOrCancel = false;
    HasStarted = true;
    StartTime.SetToNow();
    ShowStatus( "About to start HTTP background thread." );

    if( ShowDiagnostics )
      {
      Show();
      Update();
      BringToFront();
      }

    HttpWorkerInfo WInfo = new HttpWorkerInfo();
    WInfo.URLToGet = URLToGet;
    WInfo.FileName = FileName;

    try
    {
    if( !HttpBackgroundWorker.IsBusy )
      HttpBackgroundWorker.RunWorkerAsync( WInfo );

    }
    catch( Exception Except )
      {
      ShowStatus( "Error starting HTTP background process." );
      ShowStatus( Except.Message );
      return;
      }
    }



  private void GetFromURLForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    e.Cancel = true;
    Hide();
    }



  internal void SaveStatusToFile( string FileName )
    {
    try
    {
    using( StreamWriter SWriter = new StreamWriter( FileName  )) 
      {
      foreach( string Line in MainTextBox.Lines )
        {
        SWriter.WriteLine( Line );
        }
      }

    // MForm.StartProgramOrFile( FileName );

    }
    catch( Exception Except )
      {
      ShowStatus( "Error: Could not write the data to the file." );
      ShowStatus( FileName );
      ShowStatus( Except.Message );
      return;
      }
    }


  }
}


