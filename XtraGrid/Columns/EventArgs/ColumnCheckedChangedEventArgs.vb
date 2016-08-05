Imports DevExpress.XtraGrid.Columns
Imports System
Imports System.Collections.Generic
Imports System.Linq
Namespace AcurSoft.XtraGrid.Columns

    Public Class ColumnCheckedChangedEventArgs
        Inherits EventArgs

        Public Sub New(ByVal _col As GridExColumn, ByVal _checked As Boolean)
            Column = _col
            Checked = _checked
        End Sub
        Public Property Checked() As Boolean
        Public Property Column() As GridExColumn

    End Class
End Namespace
