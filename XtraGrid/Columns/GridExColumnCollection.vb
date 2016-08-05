Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Namespace AcurSoft.XtraGrid.Columns

    Public Class GridExColumnCollection
        Inherits GridColumnCollection
        Public ReadOnly Property LastColumnVisibleIndex As Integer
            Get
                Return Convert.ToInt32(Me.LongCount(Function(q) q.VisibleIndex >= 0)) - 1
            End Get
        End Property

        Public ReadOnly Property LastVisibleColumn As GridExColumn
            Get
                Dim lastVisibleIndex As Integer = Me.LastColumnVisibleIndex
                Return DirectCast(Me.FirstOrDefault(Function(q) q.VisibleIndex = lastVisibleIndex), GridExColumn)
            End Get
        End Property

        Public Sub New(ByVal view As ColumnView)
            MyBase.New(view)
        End Sub

        Protected Overrides Function CreateColumn() As GridColumn
            Return New GridExColumn()
        End Function


        Default Public Shadows ReadOnly Property Item(ByVal fieldName As String) As GridExColumn
            Get
                Return TryCast(ColumnByFieldName(fieldName), GridExColumn)
            End Get
        End Property
        Default Public Shadows ReadOnly Property Item(ByVal index As Integer) As GridExColumn
            Get
                Return CType(List(index), GridExColumn)
            End Get
        End Property

    End Class
End Namespace
