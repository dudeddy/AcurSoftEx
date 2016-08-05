Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views
Imports DevExpress.Utils.Controls
Imports AcurSoft.XtraGrid.Views.Grid

Namespace AcurSoft.XtraGrid.Views.Base

    Public Class GridViewOptionsFilterEx
        Inherits ColumnViewOptionsFilter

        Private _ViewEx As GridViewEx

        Public Sub New(viewEx As GridViewEx)
            MyBase.New()
            _ViewEx = viewEx
            _AccentInsensitive = True
        End Sub


        Private _AccentInsensitive As Boolean
        Public Property AccentInsensitive() As Boolean
            Get
                Return _AccentInsensitive
            End Get
            Set(value As Boolean)
                If value = _AccentInsensitive Then Return
                Dim prevValue As Boolean = _AccentInsensitive
                _AccentInsensitive = value
                OnChanged(New BaseOptionChangedEventArgs("AccentInsensitive", prevValue, value))
            End Set
        End Property
    End Class
End Namespace
