Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.Data.Filtering

    Public Class NodeCommaElementEx
        Inherits NodeEditableElementEx
        Public Const CommaText As String = ", "
        Public Sub New(ByVal node As ClauseNodeEx)
            MyBase.New(node, ElementType.None, CommaText)
        End Sub
    End Class
End Namespace
