
Namespace WPF

    Public Class TimeDisplay
        Implements System.ComponentModel.INotifyPropertyChanged
        Implements IDisposable

#Region " Declarations "

        Private _bolDisposedValue As Boolean = False
        Private _datNow As DateTime = DateTime.Now
        Private WithEvents _objTimer As New System.Timers.Timer

#End Region

#Region " Properties "

        Public ReadOnly Property Now() As DateTime
            Get
                Return _datNow
            End Get
        End Property

#End Region

#Region " Events "

        Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

#Region " Constructors "

        Public Sub New()
            'TODO developers if you users really NEED the seconds displayed, this is real easy to change.
            '
            'This will get the clock to change on the next minute
            _objTimer.Interval = (60 - Now.Second) * 1000
            _objTimer.Start()

        End Sub

#End Region

#Region " Methods "

        Private Sub _objTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles _objTimer.Elapsed

            If _objTimer.Interval <> 60000 Then
                'TODO developers if you users really NEED the seconds displayed, this is real easy to change.
                'For now, every 60 seconds raise the event
                _objTimer.Interval = 60000
            End If

            _datNow = DateTime.Now

            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs("Now"))

        End Sub

#End Region

#Region " IDisposable Support "

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)

        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)

            If Not _bolDisposedValue Then

                If disposing Then
                    _objTimer.Dispose()
                End If

            End If

            _bolDisposedValue = True

        End Sub

#End Region

    End Class

End Namespace
