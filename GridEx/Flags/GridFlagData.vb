
Namespace AcurSoft.XtraGrid.Views.Grid.GridFlags

    Public Class GridFlagData
        Public Sub New(ByVal value As Object, index As Integer)
            Me.Value = value
            Me.Index = index
        End Sub
        Public Property Color() As Color = Color.Blue
        Public ReadOnly Property Value() As Object
        Public ReadOnly Property Index() As Integer

    End Class
End Namespace
