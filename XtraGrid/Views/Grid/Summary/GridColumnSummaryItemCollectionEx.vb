Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns

Namespace AcurSoft.XtraGrid.Views.Grid.Summary


    Public Class GridColumnSummaryItemCollectionEx

        Inherits GridColumnSummaryItemCollection

        Public Sub New(col As GridColumn)
            MyBase.New(col)
        End Sub

        Protected Overrides Function CreateItem() As GridSummaryItem
            Return New GridColumnSummaryItemEx()
        End Function

        Public ReadOnly Property ItemEx(ByVal index As Integer) As GridColumnSummaryItemEx
            Get
                Return TryCast(MyBase.Item(index), GridColumnSummaryItemEx)
            End Get
        End Property

    End Class
End Namespace
