Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers

Namespace AcurSoft.Data.Filtering

    Public Class CriteriaToTreeProcessorBaseEx
        Inherits CriteriaToTreeProcessor

        Public Sub New(ByVal nodesFactory As INodesFactory, ByVal skippedHolder As IList(Of CriteriaOperator))
            MyBase.New(nodesFactory, skippedHolder)
        End Sub
    End Class

End Namespace
